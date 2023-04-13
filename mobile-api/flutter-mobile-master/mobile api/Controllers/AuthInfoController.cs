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
    public class AuthInfoController : ControllerBase
    {
        private readonly JoyTvContext _context;

        public AuthInfoController(JoyTvContext context)
        {
            _context = context;
        }



        [HttpGet("Auth")]
        public IActionResult GetVerificationCode()
        {
            return Ok(new { results = _context.AUTH_INF.ToList() });
        }

        [HttpGet("{phnnum},{Authnum}")]
        public IActionResult GetPhonenum(String phnnum, String Authnum)
        {
            List<AUTH_INF> authInfos = new List<AUTH_INF>();

                authInfos = _context.AUTH_INF.Where(x => x.Phn_num == phnnum && x.Authnum == Authnum).ToList();
            return Ok(new { results = authInfos });
        }



        [HttpPost("phnnum={phnnum}")]
        public async Task<ActionResult<AUTH_INF>> PostVerificationCode(String phnnum)
        {

            //  4자리 랜덤숫자를 생성하고 DB에 넣는 로직
            MEMB_MO_INF member = new MEMB_MO_INF();
            member = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).FirstOrDefault();

            if (member != null)
            {
                AUTH_INF authinfo = new AUTH_INF();
                authinfo.Id = 0;
                return authinfo;
            }
            else {
                Random rand = new Random();
                String numStr = ""; //난수가 저장될 변수
                for (int i = 0; i < 4; i++)
                {

                    //0~9 까지 난수 생성
                    String ran = rand.Next(0, 10).ToString();
                    numStr += ran;

                }

                AUTH_INF authinfo = new AUTH_INF();

                authinfo.Authnum = numStr;
                authinfo.Phn_num = phnnum;
                authinfo.SendDate = DateTime.Now;

                _context.AUTH_INF.Add(authinfo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVerificationCode", new { id = authinfo.Id }, authinfo);
            }
             


            // 인증번호가 무조건 1111
            /*MEMB_MO_INF member = new MEMB_MO_INF();
            member = _context.MEMB_MO_INF.Where(x => x.Phn == phnnum).FirstOrDefault();

            if (member != null)
            {
                AUTH_INF authinfo = new AUTH_INF();
                authinfo.Id = 0;
                return authinfo;
            }
            else
            {

                AUTH_INF authinfo = new AUTH_INF();
                authinfo.Authnum = "1111";
                authinfo.Phn_num = phnnum;

                _context.AUTH_INF.Add(authinfo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVerificationCode", new { id = authinfo.Id }, authinfo);
            }
*/
        }

        private bool CheckPhoneNum(string authnum)
        {
            if (authnum.Contains("1111"))
            {
                return true;
            }

            return false;
        }

    }
}