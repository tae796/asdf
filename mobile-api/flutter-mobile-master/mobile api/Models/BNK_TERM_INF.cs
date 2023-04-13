using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class BNK_TERM_INF
    {
        public string Pnt_bnk_inf_cd { get; set; }
        [Key]
        public string Rent_term_inf_cd { get; set; }
        public string Term_inf_cd { get; set; }
        public string Cont_inf_cd { get; set; }
        public string Sto_inf_cd { get; set; }
        public string Del_yn { get; set; }
    }
}
