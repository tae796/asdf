using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class COUP_INF
    {
        [Key]
        public string Coup_inf_cd { get; set; }
        public string Memb_inf_cd { get; set; }
        public string Pnt_bnk_inf_cd { get; set; }
        public string Bc_coup_inf_cd { get; set; }
        public string Coup_titl { get; set; }
        public string Coup_des { get; set; }
        public DateTime? Isue_ddt { get; set; }
        public DateTime? Use_ddt { get; set; }
        public string Use_yn { get; set; }
        public DateTime? Expr_dd { get; set; }
        public string Disp_yn { get; set; }
        public string Sto_inf_cd { get; set; }

    }
}
