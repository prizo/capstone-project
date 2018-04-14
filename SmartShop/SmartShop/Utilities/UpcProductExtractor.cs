using Newtonsoft.Json;
using RestSharp;
using SmartShop.Model;
using System;
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

            // Some products do not have offers so return null
            try
            {
                var offers = result.items[0]["offers"];
            }
            catch (Exception)
            {
                return null;
            }

            // Extract data
            if (result.code == "OK" && result.items[0]["offers"].HasValues)
            {
                for (int i = 0; i < result.items[0]["offers"].Count; i++)
                {
                    // Albertsons always shows $0 for price, so skip it
                    if (result.items[0]["offers"][i]["merchant"] == "Albertsons")
                    {
                        continue;
                    }

                    string price = result.items[0]["offers"][i]["price"];

                    Product product = new Product
                    {
                        DataURL = "",
                        Image = result.items[0]["images"][0], // All products will have the same image to remain consistent
                        Name = result.items[0]["offers"][i]["title"],
                        Price = decimal.Parse(price),
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
