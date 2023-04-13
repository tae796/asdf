using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class BNK_MEMB_INF
    {
        public string Memb_inf_cd { get; set; }
        public string Pnt_bnk_inf_cd { get; set; }
        public string Memb_Grd_cd { get; set; }
        public int Mast_pnt { get; set; }
        public int Tot_pnt { get; set; }
        public int Tot_buy { get; set; }
        public int Tot_vist { get; set; }
        public int Tot_coup { get; set; }
        public DateTime Las_grd_mod_ddt { get; set; }
        public string Las_grd_mod_pers { get; set; }
        public string Fir_inp_pers { get; set; }
        public DateTime Fir_inp_ddt { get; set; }
        public string Las_mod_pers { get; set; }
        public DateTime Las_mod_ddt { get; set; }
    }
}
