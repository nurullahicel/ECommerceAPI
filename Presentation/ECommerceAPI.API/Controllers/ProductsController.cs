using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Abstractions.Storage.Local;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;

using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStorageService _storageService;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment, IStorageService storageService,
            IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository ınvoiceFileReadRepository, IInvoiceFileWriteRepository ınvoiceFileWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService = storageService;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = ınvoiceFileReadRepository;
            _invoiceFileWriteRepository = ınvoiceFileWriteRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            #region Dummy Datas
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new(){Id=Guid.NewGuid(),Name="Product 1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10},
            //    new(){Id=Guid.NewGuid(),Name="Product 2",Price=200,CreatedDate=DateTime.UtcNow,Stock=12},
            //    new(){Id=Guid.NewGuid(),Name="Product 3",Price=300,CreatedDate=DateTime.UtcNow,Stock=14},
            //});
            //await _productWriteRepository.SaveAsync();
            #endregion
            var totalProductCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();

            return Ok(new
            {
                Products = products,
                TotalProductCount = totalProductCount
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id, false);
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {

            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            var datas=await _storageService.uploadAsync("resource/files", Request.Form.Files);
            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage=_storageService.StorageName

            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
            #region OldUpload
            //var datas = await _localStorage.uploadAsync("resource/files", Request.Form.Files);
            //#region FileUploadExample
            ////await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile() { 
            ////FileName=d.fileName,
            ////Path=d.path

            ////}).ToList());
            //// await _productImageFileWriteRepository.SaveAsync();

            ////await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            ////{
            ////    FileName = d.fileName,
            ////    Path = d.path,
            ////    Price=new Random().Next()

            ////}).ToList());
            ////await _invoiceFileWriteRepository.SaveAsync();
            //#endregion

            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ECommerceAPI.Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.pathOrContainerName

            //}).ToList());
            //await _fileWriteRepository.SaveAsync();
            #endregion

        }
    }
}
