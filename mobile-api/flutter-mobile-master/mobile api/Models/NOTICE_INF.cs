using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    
    public class NOTICE_INF
    {
        [Key]
        public int Id { get; set; }
        public string Notice_titl { get; set; }
        public string Notice_url { get; set; }
    }
}
