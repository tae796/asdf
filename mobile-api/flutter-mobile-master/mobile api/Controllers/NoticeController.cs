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

namespace JoyTvApi.Controllers.AgencyTotal
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public NoticeController(JoyTvContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNotice()
        {
            List<NOTICE_INF> noticeInfos = new List<NOTICE_INF>();
            noticeInfos = _context.NOTICE_INF.ToList();

            return Ok(new { results = noticeInfos });
        }


        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NOTICE_INF>> PostNotice(NOTICE_INF noticeInfo)
        {
            List<NOTICE_INF> infos = _context.NOTICE_INF.Where(x => x.Id == noticeInfo.Id).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.NOTICE_INF.Add(noticeInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotice", new { Id = noticeInfo.Id }, noticeInfo);
        }

        
    }
}