using System.ComponentModel.DataAnnotations;

namespace FBC.Basit.Cari.DBModels
{
    public class CariKart
    {
        [Key]
        public int CariKartId { get; set; }

        public string Isim { get; set; }
        public string? Notlar { get; set; }
        public List<CariHareket> Hareketler { get; set; }
    }
}
