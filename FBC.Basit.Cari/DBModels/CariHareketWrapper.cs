namespace FBC.Basit.Cari.DBModels
{
    public class CariHareketWrapper
    {
        public int CariHareketId { get; set; }
        public DateTime? Tarih { get; set; }
        public string? Aciklama { get; set; }
        public string? EvrakNo { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public DateTime? VadeTarihi { get; set; }
        public int CariKartId { get; set; }
        public int? DovizKuruId { get; set; }
        public decimal DovizKuruAlis { get; set; }
        public decimal DovizKuruSatis { get; set; }
        public string DovizCinsi { get; set; }
        public string DovizAdi { get; set; }
        public DateTime DovizGuncellemeTarihi { get; set; }
        public decimal BorcDovizAlis { get => Borc * DovizKuruAlis; }
        public decimal BorcDovizSatis { get => Borc * DovizKuruSatis; }
        public decimal AlacakDovizAlis { get => Alacak * DovizKuruAlis; }
        public decimal AlacakDovizSatis { get => Alacak * DovizKuruSatis; }

        public CariHareket ToCariHareket() => new CariHareket
        {
            CariHareketId = CariHareketId,
            Tarih = Tarih,
            Aciklama = Aciklama,
            EvrakNo = EvrakNo,
            Borc = Borc,
            Alacak = Alacak,
            VadeTarihi = VadeTarihi,
            CariKartId = CariKartId,
            DovizKuruId = DovizKuruId
        };

        public CariHareketWrapper(CariHareket h)
        {
            CariHareketId = h.CariHareketId;
            Tarih = h.Tarih;
            Aciklama = h.Aciklama;
            EvrakNo = h.EvrakNo;
            Borc = h.Borc;
            Alacak = h.Alacak;
            VadeTarihi = h.VadeTarihi;
            CariKartId = h.CariKartId;
            DovizKuruId = h.DovizKuruId;
            if (h.DovizKuru != null)
            {
                DovizKuruAlis = h.DovizKuru.GuncelKurAlis;
                DovizKuruSatis = h.DovizKuru.GuncelKurSatis;
                DovizCinsi = h.DovizKuru.DovizCinsi;
                DovizAdi = h.DovizKuru.DovizAdi;
                DovizGuncellemeTarihi = h.DovizKuru.GuncellemeTarihi;
            }
            else
            {
                DovizKuruAlis = 1;
                DovizKuruSatis = 1;
                DovizCinsi = "TRY";
                DovizAdi = "Türk Lirası";
            }
        }
    }
}
