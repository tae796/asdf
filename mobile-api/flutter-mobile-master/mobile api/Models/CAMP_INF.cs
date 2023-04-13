using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class CAMP_INF
    {
        [Key]
        public int Camp_inf_cd { get; set; }
        public string Coup_titl { get; set; }
        public DateTime? Isue_ddt { get; set; }
        public DateTime? Expr_dd { get; set; }
        public string imgurl { get; set; }
        public string Subject { get; set; }
        public string Msg { get; set; }
        public string Tmpl_cd { get; set; }
        public string Sender_key { get; set; }
        public string Pnt_bnk_inf_cd { get; set; }
        public string Sto_inf_cd { get; set; }
        public int Coup_amount { get; set; }
        public string Expr_yn { get; set; }

    }
}
