using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyTvApi.Models.AgencyTotal
{
    // 입금확인
    public class STO_INF
    {
        [Key]
        public string Sto_inf_cd { get; set; }
        public string Sto_typ_cd { get; set; }
        public string Lics_num { get; set; }
        public string Sto_nm { get; set; }
        public string Sto_addr { get; set; }
        public string Sto_phn_num { get; set; }
        public string Own_nm { get; set; }
        public string Own_phn_num { get; set; }
        public string Own_email { get; set; }
        public string Sto_mng_nm { get; set; }
        public string Sto_mng_phn_num { get; set; }
        public string Sto_mng_email { get; set; }
        public string Fir_inp_pers { get; set; }
        public DateTime Fir_inp_ddt { get; set; }
        public string Las_mod_pers { get; set; }
        public DateTime Las_mod_ddt { get; set; }
        public string Kakao_yellow_id { get; set; }
        public string Kakao_template_cd { get; set; }
        public string Kakao_send_key { get; set; }
        public string Kakao_friend_send_key { get; set; }
        public string Use_yn { get; set; }
        public string Close_yn { get; set; }
        public string Si { get; set; }
        public string Gu { get; set; }
        public string Dong { get; set; }
        public string Info { get; set; }


    }
}
