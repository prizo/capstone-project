using RestSharp;

namespace SmartShop.Utilities
{
    class UpcRestRequest
    {
        private const string uriBase = "https://api.upcitemdb.com/prod/trial/";

        public static IRestResponse GetResponse(string query)
        {
            RestClient client = new RestClient(uriBase);

            // Lookup request with GET
            RestRequest request = new RestRequest("lookup", Method.GET);
            request.AddQueryParameter("upc", query);

            IRestResponse response = client.Execute(request);
            
            return response;
        }
    }
}
