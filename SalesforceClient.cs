using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SalesforceConnectedWithCSharp
{
    public class SalesforceClient
    {
        public string Token { get; private set; }
        public string InstanceUrl { get; private set; }

        public async Task AuthorizeAsync()
        {
            var response = await AsyncAuthRequest().ConfigureAwait(false);
            Token = response.access_token;
            InstanceUrl = response.instance_url;
        }

        private async Task<AuthResponse> AsyncAuthRequest()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", Constants.CONSUMER_KEY),
                new KeyValuePair<string, string>("client_secret", Constants.CONSUMER_SECRET),
                new KeyValuePair<string, string>("username", Constants.USERNAME),
                new KeyValuePair<string, string>("password", Constants.PASSWORD + Constants.TOKEN)
            });

            using (HttpClient _httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Constants.TOKEN_REQUEST_ENDPOINTURL),
                    Content = content
                };

                var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
                var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"Error: {responseMessage.StatusCode} - {response}");
                }

                return JsonConvert.DeserializeObject<AuthResponse>(response);
            }
        }

        // View operation
        public async Task<string> AsyncGetRequest(string token, string url, string query)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(new Uri(url), Constants.TOKEN_REQUEST_QUERYURL + query)
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        // Insert operation
        public async Task<string> AsyncInsertRequest(string token, string instanceUrl, string objectName, object data)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Construct the request URL
                var requestUri = new Uri(new Uri(instanceUrl), $"{Constants.TOKEN_REQUEST_POSTURL}{objectName}/");

                // Set headers
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Convert the provided data to JSON format
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Make the POST request
                var responseMessage = await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to create record. Status: {responseMessage.StatusCode}, Message: {await responseMessage.Content.ReadAsStringAsync()}");
                }

                // Return the response content (usually the ID of the created object)
                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        // UPDATE operation
        public async Task<string> AsyncUpdateRequest(string token, string instanceUrl, string objectName, string recordId, object data)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestUri = new Uri(new Uri(instanceUrl), $"{Constants.TOKEN_REQUEST_POSTURL}{objectName}/{recordId}");

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(new HttpMethod("POST"), requestUri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
                };

                var responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to update record. Status: {responseMessage.StatusCode}, Message: {await responseMessage.Content.ReadAsStringAsync()}");
                }

                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        // UPSERT operation
        public async Task<string> AsyncUpsertRequest(string token, string instanceUrl, string objectName, object data, string recordId = null)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                // Call the create function if the recordId is not provided
                return await AsyncInsertRequest(token, instanceUrl, objectName, data).ConfigureAwait(false);
            }
            else
            {
                // Call the update function if the recordId is provided
                return await AsyncUpdateRequest(token, instanceUrl, objectName, recordId, data).ConfigureAwait(false);
            }
        }

        // DELETE operation
        public async Task<string> AsyncDeleteRequest(string token, string instanceUrl, string objectName, string recordId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestUri = new Uri(new Uri(instanceUrl), $"{Constants.TOKEN_REQUEST_POSTURL}{objectName}/{recordId}");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responseMessage = await httpClient.DeleteAsync(requestUri).ConfigureAwait(false);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to delete record. Status: {responseMessage.StatusCode}, Message: {await responseMessage.Content.ReadAsStringAsync()}");
                }

                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        // UNDELETE operation
        public async Task<string> AsyncUndeleteRequest(string token, string instanceUrl, string[] recordIds)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestUri = new Uri(new Uri(instanceUrl), $"{Constants.TOKEN_REQUEST_POSTURL}undelete");

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(new { ids = recordIds }), Encoding.UTF8, "application/json");

                var responseMessage = await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to undelete records. Status: {responseMessage.StatusCode}, Message: {await responseMessage.Content.ReadAsStringAsync()}");
                }

                return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}