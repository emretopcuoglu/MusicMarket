using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoreHealthCheck.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(new List<string> { "" });
        }
    }
}
