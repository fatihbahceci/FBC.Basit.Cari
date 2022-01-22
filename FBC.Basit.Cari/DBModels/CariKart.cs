using System.ComponentModel.DataAnnotations;
/// <summary>
/// https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// </summary>
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
