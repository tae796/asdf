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
    public class CouponInfoController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public CouponInfoController(JoyTvContext context)
        {
            _context = context;
        }

        /*// GET: api/DepositConfirm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponInfo>>> GetCouponInfo()
        {
            return await _context.CouponInfo.ToListAsync();
        }*/

        // GET: api/DepositConfirm/5
        [HttpGet]
        public IActionResult GetCouponInfo()
        {
            List<CAMP_INF> memberInfos = new List<CAMP_INF>();

                memberInfos = _context.CAMP_INF.Where(x => x.Expr_yn == "N" && x.Expr_dd.Value > DateTime.Now).ToList();

            return Ok(new { results = memberInfos });
        }

        [HttpGet("Memb_inf_cd={Memb_inf_cd}")]
        public IActionResult GetCouponInfo(String Memb_inf_cd)
        {
            List<CAMP_INF> memberInfos = new List<CAMP_INF>();
            List<CAMP_INF> result = new List<CAMP_INF>();
            List<COUP_INF> couponinfos = new List<COUP_INF>();  
            int couponcount;
            COUP_INF coupon = new COUP_INF();

                memberInfos = _context.CAMP_INF.Where(x => x.Expr_yn == "N" && x.Expr_dd.Value > DateTime.Now).ToList();
                couponcount = memberInfos.Count;
                for(int i=0; i<couponcount; i++)
				{
                    coupon = _context.COUP_INF.Where(x => x.Coup_titl == memberInfos[i].Coup_titl && x.Memb_inf_cd == Memb_inf_cd).FirstOrDefault();
                    if(coupon==null)
					{
                        result.Add(_context.CAMP_INF.Where(x => x.Coup_titl == memberInfos[i].Coup_titl).FirstOrDefault());
                    }
                    else
					{
                        
                    }
                }


            return Ok(new { results = result }); ;
        }


        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CAMP_INF>> PostCouponInfo(CAMP_INF coupon)
        {
            List<CAMP_INF> infos = _context.CAMP_INF.Where(x => x.Camp_inf_cd == coupon.Camp_inf_cd).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.CAMP_INF.Add(coupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCouponInfo", new { Campaign_id = coupon.Camp_inf_cd }, coupon);
        }

        
    }
}