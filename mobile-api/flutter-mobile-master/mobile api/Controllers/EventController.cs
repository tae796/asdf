using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoyTvApi.Models;
using JoyTvApi.Models.AgencyTotal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace JoyTvApi.Controllers.AgencyTotal
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public EventController(JoyTvContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetEvent()
        {
            List<EVENT_INF> eventInfos = new List<EVENT_INF>();
            eventInfos = _context.EVENT_INF.ToList();
            return Ok(new { results = eventInfos });
        }

        // GET: api/DepositConfirm
        [HttpGet("Running")]
        public IActionResult GetRunningEvent()
        {
            List<EVENT_INF> eventInfos = new List<EVENT_INF>();
            eventInfos = _context.EVENT_INF.Where(x => x.Expr_yn =="N").ToList();

            return Ok(new { results = eventInfos }); ;
        }

        [HttpGet("Expired")]
        public IActionResult GetExpiredEvent()
        {
            List<EVENT_INF> eventInfos = new List<EVENT_INF>();
            eventInfos = _context.EVENT_INF.Where(x => x.Expr_yn == "Y").ToList();

            return Ok(new { results = eventInfos }); ;
        }


        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EVENT_INF>> PostEvent(EVENT_INF eventInfo)
        {
            List<EVENT_INF> infos = _context.EVENT_INF.Where(x => x.Id == eventInfo.Id).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.EVENT_INF.Add(eventInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { Id = eventInfo.Id }, eventInfo);
        }

        
    }
}