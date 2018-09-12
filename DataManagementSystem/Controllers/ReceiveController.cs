using Microsoft.AspNetCore.Mvc;

namespace DataManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody] JsonResult tree)
        {
            var x = "testString";
        }
    }
}
