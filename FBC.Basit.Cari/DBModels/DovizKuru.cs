using System.ComponentModel.DataAnnotations;

namespace FBC.Basit.Cari.DBModels
{
    public class DovizKuru
    {
        [Key]
        public int DovizKuruId { get; set; }
        public string DovizCinsi { get; set; }
        public string DovizAdi { get; set; }
        public decimal GuncelKurAlis { get; set; }
        public decimal GuncelKurSatis { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
    }
}
