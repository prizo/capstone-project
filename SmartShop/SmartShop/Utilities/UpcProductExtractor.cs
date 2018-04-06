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
                for (int i = 0; i < result.items[0]["offers"].Count; i++)
                {
                    // Albertsons always shows $0 for price, so skip it
                    if (result.items[0]["offers"][i]["merchant"] == "Albertsons")
                    {
                        continue;
                    }

                    string s = result.items[0]["offers"][i]["price"];

                    Product product = new Product
                    {
                        DataURL = "",
                        // All products will have the same image to remain consistent
                        Image = result.items[0]["images"][0],
                        Name = result.items[0]["offers"][i]["title"],
                        Price = decimal.Parse(s),
                        Seller = result.items[0]["offers"][i]["merchant"],
                        PriceSeller = "$" + result.items[0]["offers"][i]["price"] + " " + result.items[0]["offers"][i]["merchant"],
                        Link = result.items[0]["offers"][i]["link"],
                        Details = result.items[0]["description"]
                    };

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
