using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    
    public class AUTH_INF
    {
        [Key]
        public int Id { get; set; }
        public string Authnum { get; set; }
        public string Phn_num { get; set; }
        public DateTime? SendDate {get; set;} 

    }
}
