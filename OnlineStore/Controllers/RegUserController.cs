using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DAL.DBCollections;

namespace OnlineStore.Controllers
{
 
    [ApiController]
    [Route("")]
    public class RegUserController : ControllerBase
    {
        RegUserCollcetion _regUserService;
        public RegUserController(RegUserCollcetion service)
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
