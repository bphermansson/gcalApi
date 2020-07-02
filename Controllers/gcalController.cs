using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GcalData;

namespace gcalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GcalItemsController : ControllerBase
    {
        public GcalItemsController()
        {
            //_context = context;
        }

        [HttpGet("{gcal}")]
        //public  Task<IActionResult> gcaldata()
//<ActionResult<IEnumerable<User>>> gcaldata(string poolId)
        public IActionResult gcaldata()
              //  public ActionResult<String<string>> gcaldata()

        {
            //fetchGcal fetchedgcaldata = new fetchGcal();
            string v = fetchGcal.gcalfetch();
            var gdata = v;
            Console.WriteLine("gdata: " + gdata);
            List<string> googleItems = new List<string>{};
            googleItems.Add(gdata.ToString());
            return Ok(gdata);
        }
    }
}