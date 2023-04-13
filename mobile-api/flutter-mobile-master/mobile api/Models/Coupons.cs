using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class Coupons
    {
        public COUP_INF coupon { get; set; }
        public List<int> campaignIds { get; set; }
        public String Memb_inf_cd { get; set; }
    }
}
