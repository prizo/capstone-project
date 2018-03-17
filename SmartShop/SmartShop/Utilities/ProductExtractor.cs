using HtmlAgilityPack;
using SmartShop.Model;
using System.Collections.Generic;
using System.Web;

namespace SmartShop.Utilities
{
    class ProductExtractor
    {
        private HtmlDocument htmlDocument = new HtmlDocument();
        private List<Product> products = new List<Product>();
        private int numOfProducts;
        private HtmlNodeCollection itemNodes;
        private HtmlNodeCollection imagNodes;
        private HtmlNodeCollection infoNodes;
        private HtmlNode node;
        private const string baseUri = "http://www.bing.com";

        public IList<Product> ExtractProducts(string document)
        {
            htmlDocument.LoadHtml(HttpUtility.HtmlDecode(document));

            // Get all item nodes
            itemNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='br-item']") ??
                htmlDocument.DocumentNode.SelectNodes("//li[@class='br-item']");

            numOfProducts = itemNodes.Count;

            // Get nodes with images nested inside
            imagNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='cico pa_thImg']") ??
                htmlDocument.DocumentNode.SelectNodes("//div[@class='cico br-pdMainImg']");

            // Get nodes with info nested inside
            infoNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='br-pdInfo']");

            for (int i = 0; i < numOfProducts; i++)
            {
                products.Add(CreateProduct(i));
            }

            return products;
        }

        public void ExtractDetails(Product product, string document)
        {
            htmlDocument.LoadHtml(HttpUtility.HtmlDecode(document));

            product.Image = GetHigherResImage();
            product.Link = GetLink();
            product.Details = GetDetails();
        }

        private Product CreateProduct(int i)
        {
            Product p = new Product
            {
                DataURL = GetDataURL(i),
                Image = GetImage(i),
                Name = GetName(i),
                Price = GetPrice(i),
                Seller = GetSeller(i),
                PriceSeller = GetPrice(i) + " " + GetSeller(i),
                Link = "",
                Details = ""
            };

            return p;
        }

        private string GetDataURL(int i)
        {
            return itemNodes[i].GetAttributeValue("data-url", "");
        }

        private string GetImage(int i)
        {
            string imgUri = imagNodes[i].FirstChild.GetAttributeValue("data-src", null) ??
                imagNodes[i].FirstChild.GetAttributeValue("src", "");
            return string.Concat(baseUri, imgUri);
        }

        private string GetName(int i)
        {
            node = infoNodes[i].SelectSingleNode(".//div[@class='br-productTitle br-title']") ??
                itemNodes[i].SelectSingleNode(".//div[@class='br-pdItemName br-standardText']");
            return node.InnerHtml.Trim(); 
        }

        private string GetPrice(int i)
        {
            node = infoNodes[i].SelectSingleNode(".//span[@class=' br-focusPrice']") ??
                itemNodes[i].SelectSingleNode(".//div[@class='pd-price br-standardPrice promoted']");
            return node.InnerHtml.Trim();
        }

        private string GetSeller(int i)
        {
            node = infoNodes[i].SelectSingleNode(".//span[@class='br-sellers']") ??
                itemNodes[i].SelectSingleNode(".//div[@class='br-sellers']");
            return node.InnerHtml.Trim();
        }

        private string GetHigherResImage()
        {
            node = htmlDocument.DocumentNode.SelectSingleNode(".//div[@class='cico br-expandedImg']");

            string imgUri = node.FirstChild.GetAttributeValue("src", "");

            return string.Concat(baseUri, imgUri);
        }

        private string GetLink()
        {
            node = htmlDocument.DocumentNode.SelectSingleNode(".//div[@class='br-pdHeader']");

            return node.FirstChild.GetAttributeValue("href", "");
        }

        private string GetDetails()
        {
            node = htmlDocument.DocumentNode.SelectSingleNode(".//div[@class='br-pdDetailDesc br-standardText']");

            return node == null ? "" : node.InnerHtml.Trim();
        }
    }
}
