using Newtonsoft.Json;
using RestSharp;
using SmartShop.Model;
using System.Collections.Generic;

namespace SmartShop.Utilities
{
    class UpcProductExtractor
    {
        public static IList<Product> GetProducts(IRestResponse response)
        {
            IList<Product> products = new List<Product>();

            // Deserialize json response
            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
            if (result.code == "OK" && result.items[0]["offers"].HasValues)
            {
                for (int i = 0, j = 0; i < result.items[0]["offers"].Count; i++, j++)
                {
                    string s = result.items[0]["offers"][i]["price"];
                    Product product = new Product
                    {
                        DataURL = "",
                        Name = result.items[0]["offers"][i]["title"],
                        Price = decimal.Parse(s),
                        Seller = result.items[0]["offers"][i]["merchant"],
                        PriceSeller = "$" + result.items[0]["offers"][i]["price"] + " " + result.items[0]["offers"][i]["merchant"],
                        Link = result.items[0]["offers"][i]["link"],
                        Details = result.items[0]["description"]
                    };
                    if (j >= result.items[0]["images"].Count)
                    {
                        j = 0;
                    }
                    product.Image = result.items[0]["images"][j];
                    products.Add(product);
                }

                return products;
            }
            else
            {
                return null;
            }
        }
    }
}
