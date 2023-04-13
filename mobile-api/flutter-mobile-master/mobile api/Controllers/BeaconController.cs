using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoyTvApi.Models;
using JoyTvApi.Models.AgencyTotal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JoyTvApi.Controllers.AgencyTotal
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BeaconController : ControllerBase
    {
        private readonly JoyTvContext _context;
        private readonly ILogger _logger;
            
        public BeaconController(JoyTvContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("BeaconController");
        }

        [HttpGet]
        public IActionResult GetBeacon()
        {
            return Ok(new { results = _context.BEACON_INF.ToList() });
        }


        [HttpGet("{beaconudid}")]
        public IActionResult GetBeaconudid(String beaconudid)
        {
            _logger.LogInformation("Debug Test 11");
            _logger.LogInformation("beconudid:" + beaconudid);

            CAMP_INF campinfo = new CAMP_INF();
            STO_INF stoinfo = new STO_INF();
            BEACON_INF beaconInfo = new BEACON_INF();
            String udid = beaconudid.Substring(beaconudid.Length - 8);
            try {
                beaconInfo = _context.BEACON_INF.Where(x => x.Beacon_inf_cd == udid).FirstOrDefault();
            }
			catch
			{
                return Ok(new { results = false });
            }

            try
            {
                campinfo = _context.CAMP_INF.Where(x => x.Sto_inf_cd == beaconInfo.Sto_inf_cd && x.Expr_dd< DateTime.Now && x.Isue_ddt > DateTime.Now).FirstOrDefault();
            }
            catch
            {
                return Ok(new { results = false });
            }


            try
            {
                stoinfo = _context.STO_INF.Where(x => x.Sto_inf_cd == campinfo.Sto_inf_cd).FirstOrDefault();
            }
            catch
            {
                return Ok(new { results = false });
            }


            
            return Ok(new { results = stoinfo });
        }

        [HttpPost]
        public async Task<ActionResult<BEACON_INF>> PostBeacon(BEACON_INF beacon)
        {
            List<BEACON_INF> infos = _context.BEACON_INF.Where(x => x.Beacon_inf_cd == beacon.Beacon_inf_cd).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.BEACON_INF.Add(beacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeacon", new { Beacon_Id = beacon.Beacon_inf_cd }, beacon);
        }



    }
}