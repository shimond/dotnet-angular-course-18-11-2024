using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MyApiAndMore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public ValuesController(ILogger<ValuesController> logger)
        {
                
        }

        [OutputCache]
        public ActionResult GetAll()
        {
            return Ok(DateTime.Now);
        }
    }
}
