using System.Net;

namespace SalesforceConnectedWithCSharp
{
    public static class QueryRequests
    {
        /// <summary>
        /// Retrieves the size of the lists returned with all data from the SOQL query in JSON format.
        /// </summary>
        /// <param name="token">response access token</param>
        /// <param name="url">response instance url</param>
        /// <returns>String</returns>
        public async static Task<string> AsyncAccountQuery(string token, string url)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                // Create SOQL query to fetch account data.
                var query = "SELECT Id, Name FROM Account LIMIT 10";
                var encodedQuery = WebUtility.UrlEncode(query);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    // Use Salesforce's REST API endpoint for querying data
                    RequestUri = new Uri($"{url}{Constants.TOKEN_REQUEST_QUERYURL}{encodedQuery}")
                };

                request.Headers.Add("Authorization", $"Bearer {token}");
                var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
                var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Error querying Salesforce: {response}");
                    return null;
                }

                Console.WriteLine(response);

                return response;
                // eg:
                // { "totalSize":10,
                // "done":true,
                // "records":[
                // { "attributes":
                // { "type":"Account","url":"/services/data/v58.0/sobjects/Account/001Dn00000FzRkQIAV"},
                // "Id":"001Dn00000FzRkQIAV","Name":"Sample Account for Entitlements"},
            }
        }
    }
}
