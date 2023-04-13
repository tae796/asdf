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
    public class BankTermInfoController : ControllerBase
    {
            private readonly JoyTvContext _context;

            public BankTermInfoController(JoyTvContext context)
            {
                _context = context;
            }

            // GET: api/DepositConfirm
            [HttpGet]
            public async Task<ActionResult<IEnumerable<BNK_TERM_INF>>> GetBankTermInfo()
            {
                return await _context.BNK_TERM_INF.ToListAsync();
            }

            // GET: api/DepositConfirm/5
            [HttpGet("Sto_inf_cd={Sto_inf_cd}")]
            public async Task<ActionResult<List<BNK_TERM_INF>>> GetBankTermInfo(string Sto_inf_cd)
            {
                List<BNK_TERM_INF> BankTermInfos = new List<BNK_TERM_INF>();
                try
                {
                    BankTermInfos = _context.BNK_TERM_INF.Where(x => x.Sto_inf_cd == Sto_inf_cd).ToList();
                }
                catch
                {

                    return NotFound();
                }

                return BankTermInfos;
            }


            // POST: api/DepositConfirm
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<BNK_TERM_INF>> PostBankTermInfo(BNK_TERM_INF BankTermInfos)
            {
                List<BNK_TERM_INF> infos = _context.BNK_TERM_INF.Where(x => x.Rent_term_inf_cd == BankTermInfos.Rent_term_inf_cd).ToList();
                if (infos.Count > 0) return BadRequest();
                _context.BNK_TERM_INF.Add(BankTermInfos);
                await _context.SaveChangesAsync();

                // return CreatedAtAction("GetDepositConfirm", new { id = depositConfirm.Id }, depositConfirm);
                return CreatedAtAction("GetBankTermInfo", new { Rent_term_inf_cd = BankTermInfos.Rent_term_inf_cd }, BankTermInfos);
            }


            private bool BankTermInfoExists(string Rent_term_inf_cd)
            {
                return _context.BNK_TERM_INF.Any(e => e.Rent_term_inf_cd == Rent_term_inf_cd);
            }

        }
}

