using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rent_A_carr.Models
{
    public class kira_bilgileri
    {
        public int kira_bilgileri_ID { get; set; }
        public DateTime? Alis_Tarihi { get; set; }
        public DateTime? Teslim_Tarihi { get; set; }
        public string Toplam_Tutar { get; set; }
        public int Musteri_ID { get; set; }
        public int arac_ID { get; set; }
        public string Marka { get; set; }
    }
}