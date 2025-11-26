using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class AdminController : ControllerBase
    {
        [Authorize]
        [HttpGet(Name = "GetAdminAll")]     
        public  string GetAll()
        {
            return  "Test Admin";
        }
    }
}
