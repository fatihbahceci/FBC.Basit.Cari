using FBC.Basit.Cari;
using FBC.Basit.Cari.DBModels;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using HtmlAgilityPack;

namespace FBC.Basit.Cari
{
    public static class Helper
    {
        static Helper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
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
        public static async Task<IEnumerable<DovizKuru>> AltinInDenAlAsync()
        {
            string url = "https://altin.in/";
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();
            string htmlContent = Encoding.GetEncoding("ISO-8859-9").GetString(contentBytes);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);
            //new DovizKuru { DovizCinsi = "M_CUMHURIYET_ALTINI", DovizAdi = "Cumhuriyet Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_TAM_ALTIN", DovizAdi = "Tam Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_YARIM_ALTIN", DovizAdi = "Yarım Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_CEYREK_ALTIN", DovizAdi = "Çeyrek Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_RESAT_ALTINI", DovizAdi = "Reşat Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_KULPLU_RESAT_ALTINI", DovizAdi = "Kulplu Reşat Altını", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_22_AYAR_ALTIN", DovizAdi = "22 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_18_AYAR_ALTIN", DovizAdi = "18 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_14_AYAR_ALTIN", DovizAdi = "14 Ayar Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_GREMSE_ALTIN", DovizAdi = "Gremse Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_ATA_ALTIN", DovizAdi = "Ata Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_HAS_ALTIN", DovizAdi = "Has Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_HAMIT_ALTIN", DovizAdi = "Hamit Altın", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_GUMUS", DovizAdi = "Gümüş", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih },
            //new DovizKuru { DovizCinsi = "M_PLATIN", DovizAdi = "Platin", GuncelKurAlis = 1, GuncelKurSatis = 1, GuncellemeTarihi = tarih }

            // Extract data for each type of precious metal
            var metals = new[]
            {
            new {Kod = "XAU", Name = "Gram Altın", BuyXPath = "//div[@class='kurlar' and @title='Gram Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gram Altın']//li[@class='midrow satis']" },
            new {Kod = "M_CUMHURIYET_ALTINI", Name = "Cumhuriyet Altını", BuyXPath = "//div[@class='kurlar' and @title='Cumhuriyet Altını']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Cumhuriyet Altını']//li[@class='midrow satis']" },
            new {Kod = "M_TAM_ALTIN", Name = "Tam Altın", BuyXPath = "//div[@class='kurlar' and @title='Tam Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Tam Altın']//li[@class='midrow satis']" },
            new {Kod = "M_YARIM_ALTIN", Name = "Yarım Altın", BuyXPath = "//div[@class='kurlar' and @title='Yarım Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Yarım Altın']//li[@class='midrow satis']" },
            new {Kod = "M_CEYREK_ALTIN", Name = "Çeyrek Altın", BuyXPath = "//div[@class='kurlar' and @title='Çeyrek Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Çeyrek Altın']//li[@class='midrow satis']" },
            new {Kod = "M_RESAT_ALTINI", Name = "Reşat Altın", BuyXPath = "//div[@class='kurlar' and @title='Reşat Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Reşat Altın']//li[@class='midrow satis']" },
            new {Kod = "M_22_AYAR_ALTIN", Name = "22 Ayar Bilezik", BuyXPath = "//div[@class='kurlar' and @title='22 Ayar Bilezik']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='22 Ayar Bilezik']//li[@class='midrow satis']" },
            new {Kod = "M_18_AYAR_ALTIN", Name = "18 Ayar Altın", BuyXPath = "//div[@class='kurlar' and @title='18 Ayar Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='18 Ayar Altın']//li[@class='midrow satis']" },
            new {Kod = "M_14_AYAR_ALTIN", Name = "14 Ayar Altın", BuyXPath = "//div[@class='kurlar' and @title='14 Ayar Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='14 Ayar Altın']//li[@class='midrow satis']" },
            new {Kod = "M_GREMSE_ALTIN", Name = "Gremse Altın", BuyXPath = "//div[@class='kurlar' and @title='Gremse Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gremse Altın']//li[@class='midrow satis']" },
            new {Kod = "M_HAS_ALTIN", Name = "Hamit Altın", BuyXPath = "//div[@class='kurlar' and @title='Hamit Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Hamit Altın']//li[@class='midrow satis']" },
            new {Kod = "M_GUMUS", Name = "Gümüş", BuyXPath = "//div[@class='kurlar' and @title='Gümüş']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gümüş']//li[@class='midrow satis']" },
            new {Kod = "M_PLATIN", Name = "Platin", BuyXPath = "//div[@class='kurlar' and @title='Platin']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Platin']//li[@class='midrow satis']" }
        };
            var r = new List<DovizKuru>();
            foreach (var metal in metals)
            {
                var buyNode = document.DocumentNode.SelectSingleNode(metal.BuyXPath);
                var sellNode = document.DocumentNode.SelectSingleNode(metal.SellXPath);

                string buyPrice = buyNode?.InnerText.Trim() ?? "0.00";
                string sellPrice = sellNode?.InnerText.Trim() ?? "0.0";

                Console.WriteLine($"{metal.Name} - Buy: {buyPrice}, Sell: {sellPrice}");
                //yield return new DovizKuru
                r.Add(new DovizKuru
                {
                    DovizCinsi = metal.Kod,
                    DovizAdi = metal.Name,
                    GuncelKurAlis = Convert.ToDecimal(buyPrice, CultureInfo.InvariantCulture),
                    GuncelKurSatis = Convert.ToDecimal(sellPrice, CultureInfo.InvariantCulture),
                    GuncellemeTarihi = DateTime.Now
                });
            }
            return r;
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