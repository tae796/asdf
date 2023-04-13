using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    
    public class EVENT_INF
    {
        [Key]
        public int Id { get; set; }
        public string Event_titl { get; set; }
        public string Event_url { get; set; }
        public string Event_msg { get; set; }
        public string imgurl { get; set; }
        public DateTime? Expr_dd {get; set;} 
        public string Expr_yn { get; set; }
    }
}
