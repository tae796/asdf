using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    public class SINSA_MEMB_INF
    {
        [Key]
        public int ID { get; set; }
        public int GRADE { get; set; }
        public string MEMB_INF_CD { get; set; }
        public string PNT_BNK_INF_CD { get; set; }
        public DateTime? FIR_INP_DDT { get; set; }
        public DateTime? END_INP_DDT { get; set; }

    }
}
