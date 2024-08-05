using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBC.Basit.Cari.DBModels
{
    public class DovizKuruHareket
    {
        [Key]
        public int DovizKuruHareketId { get; set; }
        public DateTime Tarih { get; set; }
        public decimal Alis { get; set; }
        public decimal Satis { get; set; }
        // Merkez bankası, altin.in,  elle giriş.
        public string Kaynak { get; set; }

        [ForeignKey(nameof(DovizKuru))]
        public int DovizKuruId { get; set; }
        public DovizKuru DovizKuru { get; set; }
    }
}
