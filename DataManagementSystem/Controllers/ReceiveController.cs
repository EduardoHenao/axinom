using DataManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReceiveController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] JsonNode tree)
        {
            return Ok();
        }
    }
}
