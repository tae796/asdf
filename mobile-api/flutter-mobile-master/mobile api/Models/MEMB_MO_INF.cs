using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class MEMB_MO_INF
    {
        [Key]
        public string Email { get; set; }
        public string Memb_inf_cd { get; set; }
        public DateTime? Birth { get; set; }
        public DateTime? Exp_ddt { get; set; }
        public DateTime? Last_ddt { get; set; }
        public string Password { get; set; }
        public string Phn { get; set; }
        public DateTime? Reg_ddt { get; set; }
        public string Sex { get; set; }
        public string Auth_uid { get; set; }
        public string Name { get; set; }
        public DateTime? Pr_ddt { get; set; }
        public string Ag_ddt { get; set; }
        public string Kakao_key { get; set; }
        public string Email_yn { get; set; }
        public string Beacon_yn { get; set; }
        public string Quit_yn { get; set; }
		public string Token { get; set; }
    }
}
