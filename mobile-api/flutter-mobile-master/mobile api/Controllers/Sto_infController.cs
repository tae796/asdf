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

namespace JoyTvApi.Controllers.AgencyTotal
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class Sto_infController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public Sto_infController(JoyTvContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSto_inf()
        {
            List<STO_INF> sto_infInfos = new List<STO_INF>();
            sto_infInfos = _context.STO_INF.ToList();

            return Ok(new { results = sto_infInfos });
        }

        
        [HttpGet("Sto_inf_cd={Sto_inf_cd}")]
        public IActionResult GetRunningSto_inf(string Sto_inf_cd)
        {
            List<STO_INF> sto_infInfos = new List<STO_INF>();
            sto_infInfos = _context.STO_INF.Where(x => x.Sto_inf_cd ==Sto_inf_cd).ToList();

            return Ok(new { results = sto_infInfos });
        }

        [HttpGet("stomoinf")]
        public IActionResult GetSto_mo_inf()
        {
            List<STO_MO_INF> mo_Infos = new List<STO_MO_INF>();
            mo_Infos = _context.STO_MO_INF.ToList();

            return Ok(new { results = mo_Infos });
        }

        [HttpGet("id={id}&password={password}")]
        public IActionResult GetSto_mo_inf(string id, string password)
        {
            List<STO_MO_INF> mo_Infos = new List<STO_MO_INF>();
            mo_Infos = _context.STO_MO_INF.Where(x => x.Id == id && x.Password == password).ToList();

            return Ok(new { results = mo_Infos });
        }

        [HttpGet("Sto_mo_inf_cd={Sto_inf}")]
        public IActionResult GetStoName(string Sto_inf)
        {
            STO_INF sto_Infos = new STO_INF();
            sto_Infos = _context.STO_INF.Where(x => x.Sto_inf_cd == Sto_inf).FirstOrDefault();
            var json = new JObject();

            json.Add("Name", sto_Infos.Sto_nm);


            return Ok(new { results = json });
        }



        [HttpPost("sto_mo_inf")]
        public async Task<ActionResult<STO_MO_INF>> PostSto_mo_inf(STO_MO_INF moInfo)
        {
            List<STO_MO_INF> mo_Infos = _context.STO_MO_INF.Where(x => x.Id == moInfo.Id).ToList();
            if (mo_Infos.Count > 0) return BadRequest();
            _context.STO_MO_INF.Add(moInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSto_mo_inf", new { Id = moInfo.Id }, moInfo);
        }



        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<STO_INF>> PostSto_inf(STO_INF sto_infInfo)
        {
            List<STO_INF> infos = _context.STO_INF.Where(x => x.Sto_inf_cd == sto_infInfo.Sto_inf_cd).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.STO_INF.Add(sto_infInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSto_inf", new { Id = sto_infInfo.Sto_inf_cd }, sto_infInfo);
        }

        
    }
}