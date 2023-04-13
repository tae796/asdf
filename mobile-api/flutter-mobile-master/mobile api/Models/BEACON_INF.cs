using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    
    public class BEACON_INF
    {
    
        [Key]
        public string Beacon_inf_cd { get; set; }
        public string Sto_inf_cd { get; set; }
    }
}
