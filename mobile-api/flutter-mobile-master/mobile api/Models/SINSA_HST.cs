using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    public class SINSA_HST
    {
        [Key]
        public int ID { get; set; }
        public string MEMB_INF_CD { get; set; }
        public string PNT_BNK_INF_CD { get; set; }
        public DateTime? INP_DDT { get; set; }

    }
}
