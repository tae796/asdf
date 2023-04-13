using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    public class MEMB
    {
        [Key]
        public string Memb_inf_cd { get; set; }
    }
}
