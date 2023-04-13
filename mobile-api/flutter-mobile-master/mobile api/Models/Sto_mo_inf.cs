using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    
    public class STO_MO_INF
    {
    
        [Key]
        public string Id { get; set; }
        public string Password { get; set; }
        public string Sto_inf_cd { get; set; }
        public string Pnt_bnk_inf_cd {get; set;} 

    }
}
