using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBC.Basit.Cari.DBModels
{
    public class CariHareket
    {
        [Key]
        public int CariHareketId { get; set; }
        public DateTime? Tarih { get; set; }
        public string? Aciklama { get; set; }
        public string? EvrakNo { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public DateTime? VadeTarihi { get; set; }

        [ForeignKey(nameof(CariKart))]
        public int CariKartId { get; set; }
        public CariKart CariKart { get; set; }

        [ForeignKey(nameof(DovizKuru))]
        public int? DovizKuruId { get; set; }
        public DovizKuru? DovizKuru { get; set; }

        public void Fill(CariHareket h)
        {
            //public int CariHareketId { get; set; }
            //public int CariKartId { get; set; }
            Tarih = h.Tarih;
            Aciklama = h.Aciklama;
            EvrakNo = h.EvrakNo;
            Borc = h.Borc;
            Alacak = h.Alacak;
            VadeTarihi = h.VadeTarihi;
            DovizKuruId = h.DovizKuruId;
        }
    }
}
