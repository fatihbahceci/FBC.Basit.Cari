// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using System.Text;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.WriteLine("Hello, World!");
string url = "https://altin.in/";
using HttpClient client = new HttpClient();
HttpResponseMessage response = await client.GetAsync(url);
response.EnsureSuccessStatusCode();
byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();
string htmlContent = Encoding.GetEncoding("ISO-8859-9").GetString(contentBytes);

//Console.WriteLine("Hello, World!");
//string url = "https://altin.in/";
//using HttpClient client = new HttpClient();
//HttpResponseMessage response = await client.GetAsync(url);
//response.EnsureSuccessStatusCode();
//string htmlContent = await response.Content.ReadAsStringAsync();

HtmlDocument document = new HtmlDocument();
document.LoadHtml(htmlContent);

// Extract data for each type of precious metal
var metals = new[]
{
            new { Name = "Gram Altın", BuyXPath = "//div[@class='kurlar' and @title='Gram Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gram Altın']//li[@class='midrow satis']" },
            new { Name = "Cumhuriyet Altını", BuyXPath = "//div[@class='kurlar' and @title='Cumhuriyet Altını']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Cumhuriyet Altını']//li[@class='midrow satis']" },
            new { Name = "Tam Altın", BuyXPath = "//div[@class='kurlar' and @title='Tam Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Tam Altın']//li[@class='midrow satis']" },
            new { Name = "Yarım Altın", BuyXPath = "//div[@class='kurlar' and @title='Yarım Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Yarım Altın']//li[@class='midrow satis']" },
            new { Name = "Çeyrek Altın", BuyXPath = "//div[@class='kurlar' and @title='Çeyrek Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Çeyrek Altın']//li[@class='midrow satis']" },
            new { Name = "Reşat Altın", BuyXPath = "//div[@class='kurlar' and @title='Reşat Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Reşat Altın']//li[@class='midrow satis']" },
            new { Name = "22 Ayar Bilezik", BuyXPath = "//div[@class='kurlar' and @title='22 Ayar Bilezik']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='22 Ayar Bilezik']//li[@class='midrow satis']" },
            new { Name = "18 Ayar Altın", BuyXPath = "//div[@class='kurlar' and @title='18 Ayar Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='18 Ayar Altın']//li[@class='midrow satis']" },
            new { Name = "14 Ayar Altın", BuyXPath = "//div[@class='kurlar' and @title='14 Ayar Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='14 Ayar Altın']//li[@class='midrow satis']" },
            new { Name = "Gremse Altın", BuyXPath = "//div[@class='kurlar' and @title='Gremse Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gremse Altın']//li[@class='midrow satis']" },
            new { Name = "Hamit Altın", BuyXPath = "//div[@class='kurlar' and @title='Hamit Altın']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Hamit Altın']//li[@class='midrow satis']" },
            new { Name = "Gümüş", BuyXPath = "//div[@class='kurlar' and @title='Gümüş']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Gümüş']//li[@class='midrow satis']" },
            new { Name = "Platin", BuyXPath = "//div[@class='kurlar' and @title='Platin']//li[@class='midrow alis']", SellXPath = "//div[@class='kurlar' and @title='Platin']//li[@class='midrow satis']" }
        };


//// Get gold price
//var goldNode = document.DocumentNode.SelectSingleNode("//div[@id='ons']//h2[@id='ofiy']");
//string goldPrice = goldNode?.InnerText ?? "Not found";

//// Get silver price
//var silverNode = document.DocumentNode.SelectSingleNode("//div[@id='gumus']//h2[@id='gfiy']");
//string silverPrice = silverNode?.InnerText ?? "Not found";

//// Get platinum price
//var platinumNode = document.DocumentNode.SelectSingleNode("//div[@id='platin']//h2[@id='ptfiy']");
//string platinumPrice = platinumNode?.InnerText ?? "Not found";

//Console.WriteLine($"Gold Price: {goldPrice}");
//Console.WriteLine($"Silver Price: {silverPrice}");
//Console.WriteLine($"Platinum Price: {platinumPrice}");
foreach (var metal in metals)
{
    var buyNode = document.DocumentNode.SelectSingleNode(metal.BuyXPath);
    var sellNode = document.DocumentNode.SelectSingleNode(metal.SellXPath);

    string buyPrice = buyNode?.InnerText.Trim() ?? "Not found";
    string sellPrice = sellNode?.InnerText.Trim() ?? "Not found";

    Console.WriteLine($"{metal.Name} - Buy: {buyPrice}, Sell: {sellPrice}");
}