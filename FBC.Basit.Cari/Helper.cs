using FBC.Basit.Cari;
using FBC.Basit.Cari.DBModels;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace FBC.Basit.Cari
{
    [XmlRoot(ElementName = "Currency")]
    public class Currency
    {
        [XmlAttribute("CrossOrder")]
        public int CrossOrder { get; set; }

        [XmlAttribute("Kod")]
        public string Kod { get; set; }

        [XmlAttribute("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement("Unit")]
        public int Unit { get; set; }

        [XmlElement("Isim")]
        public string Isim { get; set; }

        [XmlElement("CurrencyName")]
        public string CurrencyName { get; set; }

        [XmlElement("ForexBuying")]
        public string ForexBuying { get; set; }

        [XmlElement("ForexSelling")]
        public string ForexSelling { get; set; }

        [XmlIgnore]
        public decimal ForexBuyingAsDecimal => string.IsNullOrEmpty(ForexBuying)? 0: decimal.Parse(ForexBuying ?? "0", CultureInfo.InvariantCulture);
        [XmlIgnore]
        public decimal ForexSellingAsDecimal => string.IsNullOrEmpty(ForexSelling) ? 0 : decimal.Parse(ForexSelling ?? "0", CultureInfo.InvariantCulture);

        //[XmlElement("BanknoteBuying")]
        //public decimal? BanknoteBuying { get; set; }

        //[XmlElement("BanknoteSelling")]
        //public decimal? BanknoteSelling { get; set; }

        //[XmlElement("CrossRateUSD")]
        //public decimal? CrossRateUSD { get; set; }

        //[XmlElement("CrossRateOther")]
        //public decimal? CrossRateOther { get; set; }
    }

    [XmlRoot(ElementName = "Tarih_Date")]
    public class TarihDate
    {

        [XmlElement(ElementName = "Currency")]
        public List<Currency> Currency { get; set; }

        [XmlAttribute(AttributeName = "Tarih")]
        public string Tarih { get; set; }

        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlAttribute(AttributeName = "Bulten_No")]
        public string BultenNo { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
    public class Helper
    {
        public static IEnumerable<DovizKuru> MerkezBankasindanAl()
        {
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var xmlContent = response.Content.ReadAsStringAsync().Result;
                XmlSerializer serializer = new XmlSerializer(typeof(TarihDate));
                using (StringReader reader = new StringReader(xmlContent))
                {
                    TarihDate tarihDate = (TarihDate)serializer.Deserialize(reader);


                    // Veriyi yazdır
                    //Console.WriteLine($"Tarih: {tarihDate.Tarih}, Date: {tarihDate.Date}, Bulten No: {tarihDate.BultenNo}");
                    foreach (var currency in tarihDate.Currency)
                    {
                        yield return new DovizKuru
                        {
                            DovizCinsi = currency.Kod,
                            DovizAdi = currency.Isim,
                            GuncelKurAlis = currency.ForexBuyingAsDecimal / currency.Unit,
                            GuncelKurSatis = currency.ForexSellingAsDecimal / currency.Unit,
                            GuncellemeTarihi = DateTime.Now
                        };
                        //Console.WriteLine($"Kod: {currency.Kod}, Currency Code: {currency.CurrencyCode}, Isim: {currency.Isim}");
                        //Console.WriteLine($"Forex Buying: {currency.ForexBuying}, Forex Selling: {currency.ForexSelling}");
                        //Console.WriteLine($"Banknote Buying: {currency.BanknoteBuying}, Banknote Selling: {currency.BanknoteSelling}");
                        //Console.WriteLine($"Cross Rate USD: {currency.CrossRateUSD}, Cross Rate Other: {currency.CrossRateOther}");
                        //Console.WriteLine();
                    }
                }
            }

        }
        public static IEnumerable<DovizKuru> MerkezBankasindanAlEski()
        {
            //https://www.tcmb.gov.tr/kurlar/today.xml
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var xml = response.Content.ReadAsStringAsync().Result;
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var nodes = doc.SelectNodes("//Currency");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        var dovizCinsi = node.SelectSingleNode("Isim")!.InnerText;
                        var dovizAdi = node.SelectSingleNode("CurrencyName")!.InnerText;
                        var alisStr = node.SelectSingleNode("ForexBuying")!.InnerText;
                        if (string.IsNullOrEmpty(alisStr) || alisStr == "0,0000")
                        {
                            continue;
                        }
                        var alis = Convert.ToDecimal(alisStr/*.Replace(".", ",")*/, CultureInfo.InvariantCulture);
                        var satisStr = node.SelectSingleNode("ForexSelling")!.InnerText;
                        if (string.IsNullOrEmpty(satisStr) || satisStr == "0,0000")
                        {
                            continue;
                        }
                        var satis = Convert.ToDecimal(satisStr/*.Replace(".", ",")*/, CultureInfo.InvariantCulture);
                        yield return new DovizKuru
                        {
                            DovizCinsi = dovizCinsi,
                            DovizAdi = dovizAdi,
                            GuncelKurAlis = alis,
                            GuncelKurSatis = satis,
                            GuncellemeTarihi = DateTime.Now
                        };
                    }
                }
            }
        }

    }
}