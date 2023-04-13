using JoyTvApi.Models;
using JoyTvApi.Models.AgencyTotal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyTvApi.Controllers.AgencyTotal
{
	[Route("api/[controller]")]
	[Produces("application/json")]
	[ApiController]
	public class CouponController : ControllerBase
	{
		private readonly JoyTvContext _context;

		public CouponController(JoyTvContext context)
		{
			_context = context;
		}

		// GET: api/DepositConfirm
		[HttpGet]
		public IActionResult GetCoupon()
		{
			return Ok(new { results = _context.COUP_INF.ToList() });
		}

		// GET: api/DepositConfirm/5
		[HttpGet("{Memb_inf_cd},{Pnt_bnk_inf_cd}")]
		public IActionResult GetCoupon(String Memb_inf_cd, String Pnt_bnk_inf_cd)
		{
			List<COUP_INF> memberInfos = new List<COUP_INF>();

			memberInfos = _context.COUP_INF.Where(x => x.Memb_inf_cd == Memb_inf_cd && x.Pnt_bnk_inf_cd == Pnt_bnk_inf_cd && x.Use_yn =="N" && x.Expr_dd.Value > DateTime.Now).ToList();

			return Ok(new { results = memberInfos });
		}

		[HttpGet("Pnt_bnk_inf_Cd={Pnt_bnk_inf_cd}&Datetime={dateTime}")]
		public IActionResult GetUseCoupon(String Pnt_bnk_inf_cd, DateTime dateTime)
		{
			
			List<COUP_INF> couponInfos = new List<COUP_INF>();
			MEMB_MO_INF tmp = new MEMB_MO_INF();
			string date = dateTime.ToString("yyyy-MM-dd");
			date = date + '%';
			couponInfos = _context.COUP_INF.FromSqlRaw("select * from COUP_INF where PNT_BNK_INF_CD={0} and USE_YN='Y' and USE_DDT like {1}", Pnt_bnk_inf_cd,date).ToList();
			List<JObject> json = new List<JObject>();
			
			String name;
			


			for (int i = 0; i < couponInfos.Count; i++)
			{
				JObject jsontmp = new JObject();
				tmp = _context.MEMB_MO_INF.Where(x => x.Memb_inf_cd == couponInfos[i].Memb_inf_cd).FirstOrDefault();
				name = tmp.Phn;

				jsontmp.Add("coup_titl", couponInfos[i].Coup_titl);
				jsontmp.Add("use_ddt", couponInfos[i].Use_ddt);
				jsontmp.Add("Name",name);

				json.Add(jsontmp);

			}


			return Ok(new { results = json });




		}


		// GET: api/DepositConfirm/5
		[HttpPost("GetGift/Coup_inf_Cd={Coup_inf_cd}&Memb_inf_cd={Memb_inf_cd}")]
		public async Task<ActionResult<COUP_INF>> GetGiftCoupon(String Coup_inf_cd, String Memb_inf_cd)
		{
			List<COUP_INF> couponinfo = new List<COUP_INF>();
			List<CAMP_INF> couponInfos = new List<CAMP_INF>();
			COUP_INF coupon = new COUP_INF();
			COUP_INF tmp = new COUP_INF();

			

			couponinfo = _context.COUP_INF.Where(x => x.Coup_inf_cd == Coup_inf_cd).ToList();
			couponInfos = _context.CAMP_INF.Where(x => x.Coup_titl == couponinfo[0].Coup_titl).ToList();

			tmp = _context.COUP_INF.Where(x => x.Memb_inf_cd == Memb_inf_cd && x.Coup_titl == couponInfos[0].Coup_titl).FirstOrDefault();
			if (tmp != null) return BadRequest();

			coupon.Coup_inf_cd = _context.COUP_INF2.FromSqlRaw("select GET_KEY('COUPON') as COUP_INF_CD").FirstOrDefault().COUP_INF_CD;

			coupon.Coup_titl = couponInfos[0].Coup_titl;
			coupon.Pnt_bnk_inf_cd = couponInfos[0].Pnt_bnk_inf_cd;
			coupon.Memb_inf_cd = Memb_inf_cd;
			coupon.Isue_ddt = DateTime.Now;
			coupon.Expr_dd = couponInfos[0].Expr_dd;
			coupon.Use_yn = "N";
			coupon.Disp_yn = "Y";

			_context.COUP_INF.Add(coupon);
			await _context.SaveChangesAsync();

			BNK_MEMB_INF bankInfo = new BNK_MEMB_INF();
			BNK_MEMB_INF tmp2 = new BNK_MEMB_INF();

			bankInfo = _context.BNK_MEMB_INF.Where(x => x.Pnt_bnk_inf_cd == coupon.Pnt_bnk_inf_cd && x.Memb_inf_cd == Memb_inf_cd).FirstOrDefault();
			if (bankInfo == null)
			{

				tmp2.Memb_inf_cd = Memb_inf_cd;
				tmp2.Pnt_bnk_inf_cd = coupon.Pnt_bnk_inf_cd;
				tmp2.Memb_Grd_cd = "MG2013112709340800002";
				tmp2.Mast_pnt = 0;
				tmp2.Tot_buy = 0;
				tmp2.Tot_vist = 0;
				tmp2.Tot_pnt = 0;
				tmp2.Tot_coup = 1;
				tmp2.Fir_inp_ddt = DateTime.Now;
				tmp2.Las_mod_ddt = DateTime.Now;
				tmp2.Las_grd_mod_ddt = DateTime.Now;
				tmp2.Fir_inp_pers = "MOBILE";
				tmp2.Las_mod_pers = "MOBILE";
				tmp2.Las_grd_mod_pers = "MOBILE";



				_context.BNK_MEMB_INF.Add(tmp2);
				await _context.SaveChangesAsync();
				CreatedAtAction("GetBankInfo", new { Memb_inf_cd = tmp2.Memb_inf_cd }, tmp2);
			}
			else
			{

				bankInfo.Tot_coup = bankInfo.Tot_coup + 1;
				await _context.SaveChangesAsync();
			}


			return CreatedAtAction("GetCoupon", new { Coup_inf_cd = coupon.Coup_inf_cd }, coupon);

		}


		[HttpGet("CouponCount/Pnt_bnk_inf_cd={Pnt_bnk_inf_cd}&Datetime={dateTime}")]
		public IActionResult GetCouponcount(String Pnt_bnk_inf_cd, DateTime dateTime)
		{
			List<COUP_INF> memberInfos = new List<COUP_INF>();
			string date = dateTime.ToString("yyyy-MM-dd");
			date = date + '%';
			return Ok(new { results = _context.COUP_INF.FromSqlRaw("select * from COUP_INF where PNT_BNK_INF_CD={0} and USE_YN='Y' and USE_DDT like {1}", Pnt_bnk_inf_cd, date).ToList().Count() });
		}

		[HttpGet("UserCount/Pnt_bnk_inf_cd={Pnt_bnk_inf_cd}&Datetime={dateTime}")]
		public IActionResult GetUsercount(String Pnt_bnk_inf_cd, DateTime dateTime)
		{
			List<COUP_INF> memberInfos = new List<COUP_INF>();
			int usercount = 0;
			string date = dateTime.ToString("yyyy-MM-dd");
			date = date + '%';
			try
			{
				memberInfos = _context.COUP_INF.FromSqlRaw("select * from COUP_INF where PNT_BNK_INF_CD={0} and USE_YN='Y' and USE_DDT like {1}", Pnt_bnk_inf_cd, date).ToList();
				usercount = memberInfos.GroupBy(x => x.Memb_inf_cd).ToList().Count();
			}
			catch
			{

			}


			return Ok(new { results = usercount });

		}


		// POST: api/DepositConfirm
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("campaign_id={campaign_id}&Memb_inf_cd={Memb_inf_cd}")]
		public async Task<ActionResult<COUP_INF>> PostCoupon(int campaign_id, string Memb_inf_cd)
		{
			COUP_INF coupon = new COUP_INF();
			CAMP_INF couponInfos = _context.CAMP_INF.Where(x => x.Camp_inf_cd == campaign_id).OrderBy(x => x.Camp_inf_cd).FirstOrDefault();
			string[] asd = new string[4];
            asd[0] = "PB2017092017024600687";
            asd[1] = "PB2016120118590600590";
            asd[2] = "PB2018081014485700767";
            asd[3] = "PB2017022017083100627";
			asd[4] = "PB2017042815170000648";
			string contain = couponInfos.Coup_titl;

			
			if (couponInfos == null) return BadRequest();

			if(contain.Contains("텐퍼센트")){

				for(int i = 0 ; i<asd.Length ;i++ ){
					coupon.Coup_inf_cd = _context.COUP_INF2.FromSqlRaw("select GET_KEY('COUPON') as COUP_INF_CD").FirstOrDefault().COUP_INF_CD;
					coupon.Coup_titl = couponInfos.Coup_titl;
					coupon.Pnt_bnk_inf_cd = asd[i];
					coupon.Memb_inf_cd = Memb_inf_cd;
					coupon.Isue_ddt = DateTime.Now;
					coupon.Expr_dd = couponInfos.Expr_dd;
					coupon.Use_yn = "N";
					coupon.Disp_yn = "Y";

					_context.COUP_INF.Add(coupon);
					await _context.SaveChangesAsync();
				}
			}
			else if(contain.Contains("가로수길")){

				for(int i = 0 ; i<asd.Length ;i++ ){
					coupon.Coup_inf_cd = _context.COUP_INF2.FromSqlRaw("select GET_KEY('COUPON') as COUP_INF_CD").FirstOrDefault().COUP_INF_CD;
					coupon.Coup_titl = couponInfos.Coup_titl;
					coupon.Pnt_bnk_inf_cd = asd[i];
					coupon.Memb_inf_cd = Memb_inf_cd;
					coupon.Isue_ddt = DateTime.Now;
					coupon.Expr_dd = couponInfos.Expr_dd;
					coupon.Use_yn = "N";
					coupon.Disp_yn = "Y";

					_context.COUP_INF.Add(coupon);
					await _context.SaveChangesAsync();
				}
			}

			else{
				coupon.Coup_inf_cd = _context.COUP_INF2.FromSqlRaw("select GET_KEY('COUPON') as COUP_INF_CD").FirstOrDefault().COUP_INF_CD;


				coupon.Coup_titl = couponInfos.Coup_titl;
				coupon.Pnt_bnk_inf_cd = couponInfos.Pnt_bnk_inf_cd;
				coupon.Memb_inf_cd = Memb_inf_cd;
				coupon.Isue_ddt = DateTime.Now;
				coupon.Expr_dd = couponInfos.Expr_dd;
				coupon.Use_yn = "N";
				coupon.Disp_yn = "Y";

				_context.COUP_INF.Add(coupon);
				await _context.SaveChangesAsync();
			}


			BNK_MEMB_INF bankInfo = new BNK_MEMB_INF();
			BNK_MEMB_INF tmp2 = new BNK_MEMB_INF();

			bankInfo = _context.BNK_MEMB_INF.Where(x => x.Pnt_bnk_inf_cd == coupon.Pnt_bnk_inf_cd && x.Memb_inf_cd == Memb_inf_cd).FirstOrDefault();
			if (bankInfo == null)
			{
				if(contain.Contains("텐퍼센트")){
					for(int i = 0 ; i<asd.Length ;i++ ){
						tmp2.Memb_inf_cd = Memb_inf_cd;
						tmp2.Pnt_bnk_inf_cd = asd[i];
						tmp2.Memb_Grd_cd = "MG2013112709340800002";
						tmp2.Mast_pnt = 0;
						tmp2.Tot_buy = 0;
						tmp2.Tot_vist = 0;
						tmp2.Tot_pnt = 0;
						tmp2.Tot_coup = 1;
						tmp2.Fir_inp_ddt = DateTime.Now;
						tmp2.Las_mod_ddt = DateTime.Now;
						tmp2.Las_grd_mod_ddt = DateTime.Now;
						tmp2.Fir_inp_pers = "MOBILE";
						tmp2.Las_mod_pers = "MOBILE";
						tmp2.Las_grd_mod_pers = "MOBILE";
						_context.BNK_MEMB_INF.Add(tmp2);
						await _context.SaveChangesAsync();
						CreatedAtAction("GetBankInfo", new { Memb_inf_cd = tmp2.Memb_inf_cd }, tmp2);
					}
				}
				else if(contain.Contains("가로수길")){
					for(int i = 0 ; i<asd.Length ;i++ ){
						tmp2.Memb_inf_cd = Memb_inf_cd;
						tmp2.Pnt_bnk_inf_cd = asd[i];
						tmp2.Memb_Grd_cd = "MG2013112709340800002";
						tmp2.Mast_pnt = 0;
						tmp2.Tot_buy = 0;
						tmp2.Tot_vist = 0;
						tmp2.Tot_pnt = 0;
						tmp2.Tot_coup = 1;
						tmp2.Fir_inp_ddt = DateTime.Now;
						tmp2.Las_mod_ddt = DateTime.Now;
						tmp2.Las_grd_mod_ddt = DateTime.Now;
						tmp2.Fir_inp_pers = "MOBILE";
						tmp2.Las_mod_pers = "MOBILE";
						tmp2.Las_grd_mod_pers = "MOBILE";
						_context.BNK_MEMB_INF.Add(tmp2);
						await _context.SaveChangesAsync();
						CreatedAtAction("GetBankInfo", new { Memb_inf_cd = tmp2.Memb_inf_cd }, tmp2);
					}
				}
				else{

					tmp2.Memb_inf_cd = Memb_inf_cd;
					tmp2.Pnt_bnk_inf_cd = coupon.Pnt_bnk_inf_cd;
					tmp2.Memb_Grd_cd = "MG2013112709340800002";
					tmp2.Mast_pnt = 0;
					tmp2.Tot_buy = 0;
					tmp2.Tot_vist = 0;
					tmp2.Tot_pnt = 0;
					tmp2.Tot_coup = 1;
					tmp2.Fir_inp_ddt = DateTime.Now;
					tmp2.Las_mod_ddt = DateTime.Now;
					tmp2.Las_grd_mod_ddt = DateTime.Now;
					tmp2.Fir_inp_pers = "MOBILE";
					tmp2.Las_mod_pers = "MOBILE";
					tmp2.Las_grd_mod_pers = "MOBILE";
					_context.BNK_MEMB_INF.Add(tmp2);
					await _context.SaveChangesAsync();
					CreatedAtAction("GetBankInfo", new { Memb_inf_cd = tmp2.Memb_inf_cd }, tmp2);
				}

				
				
			}
			else
			{

				bankInfo.Tot_coup = bankInfo.Tot_coup + 1;
				await _context.SaveChangesAsync();
			}


			return CreatedAtAction("GetCoupon", new { Coup_inf_cd = coupon.Coup_inf_cd }, coupon);
		}
		[HttpPost("/api/[Controller]/AllCoupon/Memb_inf_cd={Memb_inf_cd}")]
		public async Task<ActionResult<COUP_INF>> PostAllCoupon(string Memb_inf_cd)
		{

			List<CAMP_INF> memberInfos = new List<CAMP_INF>();
			List<CAMP_INF> result = new List<CAMP_INF>();
			List<COUP_INF> couponinfos = new List<COUP_INF>();
			int couponcount;
			COUP_INF tmp = new COUP_INF();
			COUP_INF coupon = new COUP_INF();
			memberInfos = _context.CAMP_INF.Where(x => x.Expr_yn == "N").ToList();
			couponcount = memberInfos.Count;


			for (int i = 0; i < couponcount; i++)
			{
				tmp = _context.COUP_INF.Where(x => x.Coup_titl == memberInfos[i].Coup_titl && x.Memb_inf_cd == Memb_inf_cd).FirstOrDefault();
				if (tmp == null)
				{
					coupon.Coup_inf_cd = _context.COUP_INF2.FromSqlRaw("select GET_KEY('COUPON') as COUP_INF_CD").FirstOrDefault().COUP_INF_CD;

					
					coupon.Coup_titl = memberInfos[i].Coup_titl;
					coupon.Pnt_bnk_inf_cd = memberInfos[i].Pnt_bnk_inf_cd;
					coupon.Memb_inf_cd = Memb_inf_cd;
					coupon.Isue_ddt = DateTime.Now;
					coupon.Expr_dd = memberInfos[i].Expr_dd;
					coupon.Use_yn = "N";
					coupon.Disp_yn = "Y";

					_context.COUP_INF.Add(coupon);
					await _context.SaveChangesAsync();


					BNK_MEMB_INF bankInfo = new BNK_MEMB_INF();
					BNK_MEMB_INF tmp2 = new BNK_MEMB_INF();

					bankInfo = _context.BNK_MEMB_INF.Where(x => x.Pnt_bnk_inf_cd == memberInfos[i].Pnt_bnk_inf_cd && x.Memb_inf_cd == Memb_inf_cd).FirstOrDefault();
					if (bankInfo == null)
					{

						tmp2.Memb_inf_cd = Memb_inf_cd;
						tmp2.Pnt_bnk_inf_cd = memberInfos[i].Pnt_bnk_inf_cd;
						tmp2.Memb_Grd_cd = "MG2013112709340800002";
						tmp2.Mast_pnt = 0;
						tmp2.Tot_buy = 0;
						tmp2.Tot_vist = 0;
						tmp2.Tot_pnt = 0;
						tmp2.Tot_coup = 1;
						tmp2.Fir_inp_ddt = DateTime.Now;
						tmp2.Las_mod_ddt = DateTime.Now;
						tmp2.Las_grd_mod_ddt = DateTime.Now;
						tmp2.Fir_inp_pers = "MOBILE";
						tmp2.Las_mod_pers = "MOBILE";
						tmp2.Las_grd_mod_pers = "MOBILE";

						_context.BNK_MEMB_INF.Add(tmp2);
						await _context.SaveChangesAsync();
						CreatedAtAction("GetBankInfo", new { Memb_inf_cd = tmp2.Memb_inf_cd }, tmp2);
					}
					else
					{

						bankInfo.Tot_coup = bankInfo.Tot_coup + 1;
						await _context.SaveChangesAsync();
					}



				}
				else
				{

				}
			}



			return CreatedAtAction("GetCoupon", new { Coup_inf_cd = coupon.Coup_inf_cd }, coupon);
		}

		[HttpPost]
		public async Task<ActionResult<COUP_INF>> PostCoupon(COUP_INF coupon)
		{
			List<COUP_INF> infos = _context.COUP_INF.Where(x => x.Coup_inf_cd == coupon.Coup_inf_cd).ToList();
			if (infos.Count > 0) return BadRequest();
			_context.COUP_INF.Add(coupon);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCoupon", new { Coup_inf_cd = coupon.Coup_inf_cd }, coupon);
		}

		[HttpPut("Coup_inf_cd={coup_inf_cd}")]
		public async Task<IActionResult> UseCoupon(string coup_inf_cd)
		{
			if(coup_inf_cd.Substring(0,2) =="CP") { 
				COUP_INF infos = new COUP_INF();
				infos = _context.COUP_INF.Where(x => x.Coup_inf_cd == coup_inf_cd).FirstOrDefault();
				if (infos == null) return Ok(new { results = 0 });
				else
				{
					if (infos.Use_yn == "Y")
					{
						return Ok(new { results = 2 });
					}
					else
					{
						infos.Use_yn = "Y";
						infos.Use_ddt = DateTime.Now;
					}
				}

				await _context.SaveChangesAsync();
				return Ok(new { results = 1 });

			}
			else if (coup_inf_cd.Substring(0,2) == "MI")
			{
				SINSA_MEMB_INF sinsa = new SINSA_MEMB_INF();
				sinsa = _context.SINSA_MEMB_INF.FromSqlRaw("select * from SINSA_MEMB_INF where MEMB_INF_CD={0}", coup_inf_cd).FirstOrDefault();
				if (sinsa == null) return Ok(new { results = 10 }); // 신가협 회원 X
				else
				{
					
					SINSA_HST tmp = new SINSA_HST();
					tmp.INP_DDT = DateTime.Now;
					tmp.MEMB_INF_CD = sinsa.MEMB_INF_CD;
					tmp.PNT_BNK_INF_CD = sinsa.PNT_BNK_INF_CD;

					_context.SINSA_HST.Add(tmp);
					await _context.SaveChangesAsync();

					if (sinsa.GRADE == 1) // 조합원
					{
						return Ok(new { results = 11 });
					}
					else if (sinsa.GRADE == 2) //일반 회원
					{
						return Ok(new { results = 12 });
					}
					else
					{
						return Ok(new { results = 10 }); // 오류
					}
				}
			}
			else
			{
				return Ok(new { results = 0 });
			}
		}

		[HttpGet("SINSA/{pnt_bnk_inf_cd}")]
		public IActionResult GetSinsa(String Pnt_bnk_inf_cd)
		{
			SINSA_MEMB_INF infos = new SINSA_MEMB_INF();


			infos = _context.SINSA_MEMB_INF.Where(x => x.PNT_BNK_INF_CD == Pnt_bnk_inf_cd && x.GRADE == 1).FirstOrDefault();
			if (infos != null) Ok(new { results = 1 }); // 신가협 조합원 매장

			infos = _context.SINSA_MEMB_INF.Where(x => x.PNT_BNK_INF_CD == Pnt_bnk_inf_cd && x.GRADE == 2).FirstOrDefault();
			if (infos != null) Ok(new { results = 2 }); // 신가협 회원 매장

			infos = _context.SINSA_MEMB_INF.Where(x => x.PNT_BNK_INF_CD == Pnt_bnk_inf_cd).FirstOrDefault();
			if (infos == null) Ok(new { results = 0 }); // 신가협 매장 X

			return Ok(new { results = 0 });// 오류
		}


	}
}
