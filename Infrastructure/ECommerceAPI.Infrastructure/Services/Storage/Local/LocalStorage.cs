using ECommerceAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string path, string fileName)
            =>File.Delete($"{path}\\{fileName}");
        

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetFiles().Select(x => x.Name).ToList();

        }    

        public bool HasFile(string path, string fileName)
            =>File.Exists($"{path}\\{fileName}");
        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> uploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();        
            foreach (IFormFile file in files)
            {
                
               await CopyFileAsync($"{uploadPath}\\{file.Name}", file);
                datas.Add((file.Name, $"{path}\\{file.Name}"));
               
            }


            //todo if geçerli değilse exceptionhandler ile hata fırlatılacak!

            return datas;
        }
    }
}
