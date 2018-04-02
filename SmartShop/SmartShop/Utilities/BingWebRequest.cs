using System.IO;
using System.Net;

namespace SmartShop.Utilities
{
    class BingWebRequest
    {
        private const string accessKey = "7fb9daa926524281b5e35daed1146041";
        private const string uriBase = "https://www.bing.com";

        public static string SendRequest(string query)
        {
            // Construct the URI of the search request
            string uriQuery = uriBase + query;

            // Perform the Web request and get the response
            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return result;
        }
    }
}
