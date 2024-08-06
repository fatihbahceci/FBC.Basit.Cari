using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
//using HtmlAgilityPack;
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
}