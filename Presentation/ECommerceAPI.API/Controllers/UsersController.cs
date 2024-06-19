using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly Mediator _mediator;

        public UsersController(Mediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IActionResult> Create()
        {

        }
       
    }
}
