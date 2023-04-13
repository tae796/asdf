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
    public class BankInfoController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public BankInfoController(JoyTvContext context)
        {
            _context = context;
        }

        // GET: api/DepositConfirm
        [HttpGet]
        //public string GetBankinfo()
        //{
        //    string json = JsonConvert.SerializeObject(new { results = _context.BankInfo.ToListAsync() });
        //    return json;
        //}
        public IActionResult GetBankInfo()
        {
            List<BNK_MEMB_INF> bankinfos = new List<BNK_MEMB_INF>();
            bankinfos = _context.BNK_MEMB_INF.ToList();

            return Ok(new {results = _context.BNK_MEMB_INF.ToList()});
        }

        // GET: api/DepositConfirm/5
        [HttpGet("{Memb_inf_cd},{Pnt_bnk_inf_cd}")]
        public IActionResult GetBankinfo(String Memb_inf_cd, String Pnt_bnk_inf_cd)
        {
            List<BNK_MEMB_INF> bankInfos = new List<BNK_MEMB_INF>();
            
            bankInfos = _context.BNK_MEMB_INF.Where(x => x.Memb_inf_cd == Memb_inf_cd && x.Pnt_bnk_inf_cd == Pnt_bnk_inf_cd).ToList();

            string json = JsonConvert.SerializeObject(new { results = bankInfos });

            return Ok(new { results = bankInfos });
        }

        // GET: api/DepositConfirm/5
        [HttpGet("{Memb_inf_cd}")]
        public IActionResult GetBankinfo(String Memb_inf_cd)
        {
            List<BNK_MEMB_INF> bankInfos = new List<BNK_MEMB_INF>();
            List<BankStoInfo> bankstoinfos = new List<BankStoInfo>();

            bankInfos = _context.BNK_MEMB_INF.FromSqlRaw("select * from BNK_MEMB_INF where MEMB_INF_CD = {0} and PNT_BNK_INF_CD IN(Select PNT_BNK_INF_CD from BNK_TERM_INF where STO_INF_CD IN(Select A.STO_INF_CD from STO_INF A JOIN CONT_INF B where A.STO_INF_CD = B.STO_INF_CD and B.CONT_ED_DD > now())) order by LAS_MOD_DDT desc", Memb_inf_cd).ToList();

            int bankcount = bankInfos.Count();
            for(int i=0; i < bankcount; i++)
			{
                
                STO_INF sto_tmp = new STO_INF();
                List<BNK_TERM_INF> change = new List<BNK_TERM_INF>();
                change = _context.BNK_TERM_INF.FromSqlRaw("select * from BNK_TERM_INF where PNT_BNK_INF_CD={0} group by STO_INF_CD",bankInfos[i].Pnt_bnk_inf_cd).ToList();

                List<COUP_INF> coupon = new List<COUP_INF>();
                coupon = _context.COUP_INF.FromSqlRaw("select * from COUP_INF where PNT_BNK_INF_CD={0} and MEMB_INF_CD={1} and USE_YN='N' and EXPR_DD>NOW()", bankInfos[i].Pnt_bnk_inf_cd,Memb_inf_cd).ToList();
                int coupon_count = coupon.Count();

                if (bankInfos[i].Pnt_bnk_inf_cd== "PB2016120118590600590") // ∆»¿œªÔ øπø‹√≥∏Æ
                {
                    STO_INF tmp2 = new STO_INF();
                    tmp2 = _context.STO_INF.FromSqlRaw("select * from STO_INF where STO_INF_CD='SI2016120118522600757'").FirstOrDefault();
                    BankStoInfo tmp = new BankStoInfo();
                    tmp.Memb_inf_cd = bankInfos[i].Memb_inf_cd;
                    tmp.Pnt_bnk_inf_cd = bankInfos[i].Pnt_bnk_inf_cd;
                    tmp.Memb_Grd_cd = bankInfos[i].Memb_Grd_cd;
                    tmp.Mast_pnt = bankInfos[i].Mast_pnt;
                    tmp.Tot_pnt = bankInfos[i].Tot_pnt;
                    tmp.Tot_buy = bankInfos[i].Tot_buy;
                    tmp.Tot_vist = bankInfos[i].Tot_vist;
                    tmp.Tot_coup = coupon_count;
                    tmp.Las_grd_mod_ddt = bankInfos[i].Las_grd_mod_ddt;
                    tmp.Las_grd_mod_pers = bankInfos[i].Las_grd_mod_pers;
                    tmp.Fir_inp_pers = bankInfos[i].Fir_inp_pers;
                    tmp.Fir_inp_ddt = bankInfos[i].Fir_inp_ddt;
                    tmp.Las_mod_pers = bankInfos[i].Las_mod_pers;
                    tmp.Las_mod_ddt = bankInfos[i].Las_mod_ddt;
                    tmp.Sto_inf_cd = "SI2016120118522600757";
                    tmp.Sto_nm = tmp2.Sto_nm;
                    bankstoinfos.Add(tmp);
                }
                else
                {
                    for (int j = 0; j < change.Count; j++)
                    {
                        BankStoInfo tmp = new BankStoInfo();
                        STO_INF tmp2 = new STO_INF();
                        tmp2 = _context.STO_INF.FromSqlRaw("select * from STO_INF where STO_INF_CD IN(select STO_INF_CD from CONT_INF where CONT_ED_DD>now()) and STO_INF_CD ={0} and STO_INF_CD IN (select STO_INF_CD from PNT_HST where MEMB_INF_CD={1})", change[j].Sto_inf_cd, Memb_inf_cd).FirstOrDefault();
                        if (tmp2 == null)
                        {

                        }
                        else
                        {
                            tmp.Memb_inf_cd = bankInfos[i].Memb_inf_cd;
                            tmp.Pnt_bnk_inf_cd = bankInfos[i].Pnt_bnk_inf_cd;
                            tmp.Memb_Grd_cd = bankInfos[i].Memb_Grd_cd;
                            tmp.Mast_pnt = bankInfos[i].Mast_pnt;
                            tmp.Tot_pnt = bankInfos[i].Tot_pnt;
                            tmp.Tot_buy = bankInfos[i].Tot_buy;
                            tmp.Tot_vist = bankInfos[i].Tot_vist;
                            tmp.Tot_coup = coupon_count;
                            tmp.Las_grd_mod_ddt = bankInfos[i].Las_grd_mod_ddt;
                            tmp.Las_grd_mod_pers = bankInfos[i].Las_grd_mod_pers;
                            tmp.Fir_inp_pers = bankInfos[i].Fir_inp_pers;
                            tmp.Fir_inp_ddt = bankInfos[i].Fir_inp_ddt;
                            tmp.Las_mod_pers = bankInfos[i].Las_mod_pers;
                            tmp.Las_mod_ddt = bankInfos[i].Las_mod_ddt;
                            tmp.Sto_inf_cd = tmp2.Sto_inf_cd;
                            tmp.Sto_nm = tmp2.Sto_nm;
                            bankstoinfos.Add(tmp);
                        }


                    }
                }

                


            }

            return Ok(new { results = bankstoinfos });
        }


        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BNK_MEMB_INF>> PostBankInfo(BNK_MEMB_INF memberInfo)
        {
           
            List<BNK_MEMB_INF> infos = _context.BNK_MEMB_INF.Where(x => x.Memb_inf_cd == memberInfo.Memb_inf_cd && x.Pnt_bnk_inf_cd == memberInfo.Pnt_bnk_inf_cd).ToList();
            if (infos.Count > 0) return BadRequest();
            _context.BNK_MEMB_INF.Add(memberInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankinfo", new { Memb_inf_cd = memberInfo.Memb_inf_cd }, memberInfo);
        }

        
    }
}