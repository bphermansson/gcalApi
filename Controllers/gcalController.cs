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
        [HttpGet("getevents")]
        public IActionResult gcaldata()
        {
            string v = fetchGcal.gcalfetch();
            var gdata = v;
            Console.WriteLine("gdata: " + gdata);
            List<string> googleItems = new List<string>{};
            googleItems.Add(gdata.ToString());
            return Ok(gdata);
        }
        [HttpGet("listcalendars")]
        public string listcalendars()
        {
            var userCals = listCalendars.gcalfetch();
            return userCals;
        }
    }
}