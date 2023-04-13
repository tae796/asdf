using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    public class MEMB_INF
    {
        [Key]
        public string Memb_inf_cd { get; set; }
        public string Phn_num { get; set; }
        public string Nm { get; set; }
        public string Email { get; set; }
        public DateTime? Bir_dd { get; set; }
        public DateTime? Fir_inp_ddt { get; set; }
        public DateTime? Las_mod_ddt { get; set; }
        public string Las_mod_pers { get; set; }
    }
}
