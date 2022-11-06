using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
 
    [ApiController]
    [Route("")]
    public class RegUserController : ControllerBase
    {
        RegUserService _regUserService;
        public RegUserController(RegUserService service)
        {
            _regUserService = service;
        }
        [HttpGet]
        public async Task<List<OnlineStore.Models.RegUser>> Get()
        {
           return await _regUserService.GetAsync();
        }
    }
}
