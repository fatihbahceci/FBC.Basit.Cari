using System.ComponentModel.DataAnnotations;

namespace FBC.Basit.Cari.DBModels
{
    public class DovizKuru
    {
        [Key]
        public int DovizKuruId { get; set; }
        /// <summary>
        /// TRY, USD, EUR, GBP, JPY, CNY, RUB, ...
        /// </summary>
        public string DovizCinsi { get; set; }
        /// <summary>
        /// Türk Lirası, Amerikan Doları, Euro, İngiliz Sterlini, Japon Yeni, Çin Yuanı, Rus Rublesi, ...
        /// </summary>
        public string DovizAdi { get; set; }
        public decimal GuncelKurAlis { get; set; }
        public decimal GuncelKurSatis { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        public DovizKuru()
        {
            DovizCinsi = "XXX";
            DovizAdi = "XXX Lirası";
            GuncellemeTarihi = DateTime.Now;
        }

        public void Fill(DovizKuru dovizKuru)
        {
            DovizCinsi = dovizKuru.DovizCinsi;
            DovizAdi = dovizKuru.DovizAdi;
            GuncelKurAlis = dovizKuru.GuncelKurAlis;
            GuncelKurSatis = dovizKuru.GuncelKurSatis;
            GuncellemeTarihi = dovizKuru.GuncellemeTarihi;
        }
    }
}
