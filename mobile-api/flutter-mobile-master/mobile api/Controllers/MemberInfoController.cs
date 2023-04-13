using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoyTvApi.Models;
using JoyTvApi.Models.AgencyTotal;
using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;

namespace JoyTvApi.Controllers.AgencyTotal
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class MemberInfoController : ControllerBase
    {


        private readonly JoyTvContext _context;
        private readonly ILogger _logger;
        public MemberInfoController(JoyTvContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("MemberInfoController");
        }

        // GET: api/DepositConfirm
        [HttpGet]
        public IActionResult GetDepositConfirm()
        {
            string json = JsonConvert.SerializeObject(new { results = _context.MEMB_MO_INF.ToListAsync() });
            return Ok(new { results = _context.MEMB_MO_INF.ToList() });
        }


        // GET: api/DepositConfirm/5
        [HttpGet("{phnnum},{password}")]
        public IActionResult GetMemberinfo(String phnnum, String password)
        {
            _logger.LogInformation("Debug Test Login");

            MEMB_MO_INF memberInfos = new MEMB_MO_INF();
            MEMB_MO_INF2 memberInfos2 = new MEMB_MO_INF2();
            memberInfos = _context.MEMB_MO_INF.FromSqlRaw("select * from MEMB_MO_INF where Phn ={0} and Password =PASSWORD({1})", phnnum, password).FirstOrDefault();              
            memberInfos2.Email = memberInfos.Email;
            memberInfos2.Memb_inf_cd  = memberInfos.Memb_inf_cd ;
            memberInfos2.Birth  = memberInfos.Birth ;
            memberInfos2.Exp_ddt  = memberInfos.Exp_ddt ;
            memberInfos2.Last_ddt = memberInfos.Last_ddt;
            memberInfos2.Password = memberInfos.Password;
            memberInfos2.Phn  = memberInfos.Phn ;
            memberInfos2.Reg_ddt  = memberInfos.Reg_ddt ;
            memberInfos2.Sex  = memberInfos.Sex ;
            memberInfos2.Auth_uid  = memberInfos.Auth_uid ;
            memberInfos2.Name  = memberInfos.Name ;
            memberInfos2.Pr_ddt  = memberInfos.Pr_ddt ;
            memberInfos2.Ag_ddt  = memberInfos.Ag_ddt ;
            memberInfos2.Kakao_key  = memberInfos.Kakao_key ;
            memberInfos2.Email_yn  = memberInfos.Email_yn ;
            memberInfos2.Beacon_yn   = memberInfos.Beacon_yn  ;
            memberInfos2.Quit_yn   = memberInfos.Quit_yn ;
            memberInfos2.Token   = memberInfos.Token  ;
               // Where(x => x.Phn == phnnum && x.Password == password).ToList();


            memberInfos2.Token = GetToken(memberInfos2).Token;
            SINSA_MEMB_INF sinsa = new SINSA_MEMB_INF();
            sinsa = _context.SINSA_MEMB_INF.Where(x => x.MEMB_INF_CD == memberInfos.Memb_inf_cd).FirstOrDefault();
            if(sinsa != null){
                memberInfos2.grade = sinsa.GRADE;
                memberInfos2.Fir_inp_ddt = sinsa.FIR_INP_DDT;
                memberInfos2.End_inp_ddt = sinsa.END_INP_DDT;
            }
            else {
                memberInfos2.grade = 0;
                memberInfos2.Fir_inp_ddt = DateTime.Now;
                memberInfos2.End_inp_ddt = DateTime.Now;
            }
            


            return Ok(new { results = memberInfos2 });
        }

    
        // GET: api/DepositConfirm/5
        [HttpGet("{phnnum}")]
        public IActionResult GetPhonenum(String phnnum)
        {
            List<MEMB_MO_INF> memberInfos = new List<MEMB_MO_INF>();

            memberInfos = _context.MEMB_MO_INF.FromSqlRaw("select * from MEMB_MO_INF where Phn = {0}", phnnum).ToList();

            //Where(x => x.Phn == phnnum).ToList();

            return Ok(new { results = memberInfos });
        }


        [HttpGet("/kakao/{kakaokey}")]
        public IActionResult GetKakaokey(String kakaokey)
        {
            List<MEMB_MO_INF> memberInfos = new List<MEMB_MO_INF>();

            memberInfos = _context.MEMB_MO_INF.Where(x => x.Kakao_key == kakaokey).ToList();

            return Ok(new { results = memberInfos });
        }
/*
        [HttpGet("/apple/{applekey}")]
        public IActionResult GetApplekey(String applekey)
        {
            List<MEMB_MO_INF> memberInfos = new List<MEMB_MO_INF>();

            memberInfos = _context.MemberInfo.Where(x => x.Apple_key == applekey).ToList();

            return Ok(new { results = memberInfos });
        }*/

        [HttpPost("email")]
        public async Task<IActionResult> PostMail(String email)
        {
            List<MEMB_MO_INF> memberInfos = new List<MEMB_MO_INF>();

            memberInfos = _context.MEMB_MO_INF.Where(x => x.Email == email).ToList();

            if(memberInfos.Count == 0)
            {
                return Ok(new { results = false });
            }
            else
            {
                var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var Charsarr = new char[10];
                var random = new Random();

                for (int i = 0; i < Charsarr.Length; i++)
                {
                    Charsarr[i] = characters[random.Next(characters.Length)];
                }

                var resultString = new String(Charsarr);
                Console.WriteLine(resultString);


                MailMessage mail = new MailMessage();

                try
                {

                    // 보내는 사람 메일, 이름, 인코딩(UTF-8)
                    mail.From = new MailAddress("tae796@naver.com", "flutter_mobile", System.Text.Encoding.UTF8);
                    // 받는 사람 메일
                    mail.To.Add(email);
                    // 메일 제목
                    mail.Subject = "flutter_mobile password";
                    // 본문 내용
                    mail.Body = resultString;
                    // 본문 내용 포멧의 타입 (true의 경우 Html 포멧으로)
                    mail.IsBodyHtml = true;
                    // 메일 제목과 본문의 인코딩 타입(UTF-8)
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    // smtp 서버 주소
                    SmtpClient SmtpServer = new SmtpClient("smtp.naver.com");
                    // smtp 포트
                    SmtpServer.Port = 587;
                    // smtp 인증
                    SmtpServer.Credentials = new System.Net.NetworkCredential("tae796", "akakan3977~~");
                    // SSL 사용 여부
                    SmtpServer.EnableSsl = true;
                    // 발송
                    SmtpServer.Send(mail);

                    MEMB_MO_INF tmp = new MEMB_MO_INF();
                    tmp = _context.MEMB_MO_INF.Where(x => x.Email == email).FirstOrDefault();
                    _context.Database.ExecuteSqlRaw("UPDATE MEMB_MO_INF SET Password =  PASSWORD({0}) where Email = {1}", resultString, email);
                    await _context.SaveChangesAsync();


                }
                catch
                {
                    return Ok(new { results = false });
                }
                finally
                {
                    // 첨부 파일 Stream 닫기
                    foreach (var attach in mail.Attachments)
                    {
                        attach.ContentStream.Close();
                    }


                }
                return Ok(new { results = true });
            }

            
            

        }



        [HttpPut("phnnum={phnnum}&password={password}")]
        public async Task<IActionResult> ChangePassword(string phnnum, string password)
        {
            MEMB_MO_INF infos = new MEMB_MO_INF();
            infos = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).FirstOrDefault();

			try { 
            if(password == null)
			{

			}
            else
			{
                    string pwtmp = "";

                    byte[] keyArray = Encoding.UTF8.GetBytes(password);
                    SHA1Managed enc = new SHA1Managed();
                    byte[] encodedKey = enc.ComputeHash(enc.ComputeHash(keyArray));
                    StringBuilder myBuilder = new StringBuilder(encodedKey.Length);

                    foreach (byte b in encodedKey)
                        myBuilder.Append(b.ToString("X2"));

                    pwtmp = "*" + myBuilder.ToString();


                    infos.Password = pwtmp;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new { results = true }); 
            }
			catch
			{
                return Ok(new { results = false }); 
			}
        }



        [HttpPut("email/phnnum={phnnum}")]
        public async Task<IActionResult> ToggleMemberEmail(string phnnum)
        {
            MEMB_MO_INF infos = new MEMB_MO_INF();
            infos = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).FirstOrDefault();

            try
			{

			
            if (infos.Email_yn == "Y")
			{
                infos.Email_yn = "N";
                await _context.SaveChangesAsync();
            }
            else
			{
                infos.Email_yn = "Y";
                await _context.SaveChangesAsync();
            }
            return Ok(new { results = true }); 
            }
			catch
			{
                return Ok(new { results = false });
            }
        }

        [HttpPut("beacon/phnnum={phnnum}")]
        public async Task<IActionResult> ToggleMemberBeacon(string phnnum)
        {
            MEMB_MO_INF infos = new MEMB_MO_INF();
            infos = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).FirstOrDefault();

            try { 
            if (infos.Beacon_yn == "Y")
            {
                infos.Beacon_yn = "N";
                await  _context.SaveChangesAsync();
            }
            else
            {
                infos.Beacon_yn = "Y";
                await _context.SaveChangesAsync();
            }
            return Ok(new { results = true });
            }

			catch
			{
                return Ok(new { results = false });
            }
        }





        // PUT: api/DepositConfirm/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutDepositConfirm(MEMB_MO_INF depositConfirm)
        {
            MEMB_MO_INF member = new MEMB_MO_INF();
            member = _context.MEMB_MO_INF.Where(x => x.Email == depositConfirm.Email).FirstOrDefault();

            try
            {
                
                if (depositConfirm.Memb_inf_cd != null)
                {
                    member.Memb_inf_cd = depositConfirm.Memb_inf_cd;
                }
                if (depositConfirm.Password != null)
                {
                    member.Password = depositConfirm.Password;
                }
                if (depositConfirm.Phn != null)
                {
                    member.Phn = depositConfirm.Phn;
                }
                if (depositConfirm.Birth != DateTime.MinValue)
				{
                    member.Birth = depositConfirm.Birth;
				}
                if (depositConfirm.Sex != null)
                {
                    member.Sex = depositConfirm.Sex;
                }
                if (depositConfirm.Auth_uid != null)
                {
                    member.Auth_uid = depositConfirm.Auth_uid;
                }
                if (depositConfirm.Name != null)
                {
                    member.Name = depositConfirm.Name;
                }
                if (depositConfirm.Ag_ddt != null)
                {
                    member.Ag_ddt = depositConfirm.Ag_ddt;
                }
                if (depositConfirm.Kakao_key != null)
                {
                    member.Kakao_key = depositConfirm.Kakao_key;
                }
                /*if (depositConfirm.Apple_key != null)
                {
                    member.Apple_key = depositConfirm.Apple_key;
                }*/
                if (depositConfirm.Email_yn != null)
                {
                    member.Email_yn = depositConfirm.Email_yn;
                }
                if (depositConfirm.Beacon_yn != null)
                {
                    member.Beacon_yn = depositConfirm.Beacon_yn;
                }
                if (depositConfirm.Quit_yn != null)
                {
                    member.Quit_yn = depositConfirm.Quit_yn;
                }
                if (depositConfirm.Token != null)
                {
                    member.Token = depositConfirm.Token;
                }


               // _context.Entry(depositConfirm).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositConfirmExists(depositConfirm.Email))
                {
                    return Ok(new { results = false }); ;
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { results = true }); ;
        }

        // POST: api/DepositConfirm
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MEMB_MO_INF>> PostMemberInfo(MEMB_MO_INF memberInfo)
        {
            List<MEMB_INF> infos = _context.MEMB_INF.Where(x => x.Phn_num == memberInfo.Phn).ToList();
            MEMB_INF tmpmember = new MEMB_INF();
            MEMB_MO_INF member = new MEMB_MO_INF();
            if (infos.Count == 0)
            {
                member.Memb_inf_cd = _context.MEMB.FromSqlRaw("select GET_KEY('MEMBER') as Memb_inf_cd").FirstOrDefault().Memb_inf_cd;
                tmpmember.Memb_inf_cd = member.Memb_inf_cd;
                tmpmember.Phn_num = memberInfo.Phn;
                tmpmember.Fir_inp_ddt = DateTime.Now;
                tmpmember.Las_mod_ddt = DateTime.Now;
                tmpmember.Las_mod_pers = "";
                _context.MEMB_INF.Add(tmpmember);
                await _context.SaveChangesAsync();

            }
            else {

                member.Memb_inf_cd = infos[0].Memb_inf_cd;
                    
            }

            string pwtmp = "";
            byte[] keyArray;
            if (memberInfo.Password ==null)
                keyArray = Encoding.UTF8.GetBytes(memberInfo.Kakao_key);
            else
                keyArray = Encoding.UTF8.GetBytes(memberInfo.Password);
            SHA1Managed enc = new SHA1Managed();
            byte[] encodedKey = enc.ComputeHash(enc.ComputeHash(keyArray));
            StringBuilder myBuilder = new StringBuilder(encodedKey.Length);

            foreach (byte b in encodedKey)
                myBuilder.Append(b.ToString("X2"));

            pwtmp = "*" + myBuilder.ToString();






            member.Password = pwtmp;
            member.Phn = memberInfo.Phn;
            member.Birth = memberInfo.Birth;
            member.Email = memberInfo.Email;
            member.Sex = memberInfo.Sex;
            member.Auth_uid = memberInfo.Auth_uid;
            member.Name = memberInfo.Name;
            member.Ag_ddt = memberInfo.Ag_ddt;
            member.Kakao_key = memberInfo.Kakao_key;
            member.Email_yn = memberInfo.Email_yn;
            member.Beacon_yn = memberInfo.Beacon_yn;
            member.Quit_yn = memberInfo.Quit_yn;
            member.Token = memberInfo.Token;

            _context.MEMB_MO_INF.Add(member);
           
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepositConfirm", new { id = member.Phn }, member);
        }



        [HttpDelete("{phnnum}")]
        public async Task<IActionResult> Watiting(string phnnum)
        {
            List<MEMB_MO_INF> infos = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).ToList();
            _context.MEMB_MO_INF.RemoveRange(infos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        private bool DepositConfirmExists(string phnnum)
        {
            return _context.MEMB_MO_INF.Any(e => e.Phn == phnnum);
        }

        private bool CheckPhoneNum(string phnnum)
        {
            if (phnnum.Contains("1111"))
            {
                return true;
            }

            return false;
        }
		
		private LoginInfo GetToken(MEMB_MO_INF2 member) 
		{
            var claims = new List<Claim>();
            claims.Add(new Claim("username",member.Name));
            
            var issuer = "http://www.flutterup.net";
            var audience = "http://www.flutterup.net";
            var signingKey = "1234-5@4-3213@011-3948200192";  //  some long id
            
            var token = JwtHelper.GetJwtToken(
                member.Name,
                signingKey,
                issuer,
                audience,
                DateTime.Now.AddHours(3),
                claims.ToArray());
            
			// string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            LoginInfo login = new LoginInfo();
            login.Token = new JwtSecurityTokenHandler().WriteToken(token);
            login.Expires = token.ValidTo;
			
			return login;
		}
	
    }
}
