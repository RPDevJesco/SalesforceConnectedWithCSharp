using SalesforceConnectedWithCSharp.SalesforceAPI;
using SalesforceConnectedWithCSharp.DataReader;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SalesforceConnectedWithCSharp.SalesforceAPI
{
    public class SalesforceBatchAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _instanceUrl;

        public SalesforceBatchAPI(string token, string instanceUrl)
        {
            _token = token;
            _instanceUrl = instanceUrl;
            _httpClient = new HttpClient();
            // Set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Add("X-SFDC-Session", _token);
        }

        private async Task<string> CreateJobAsync(string objectName, string operation)
        {
            string url = $"{_instanceUrl}{Constants.JOB_ENDPOINT}";
            var jobRequest = new
            {
                ops = operation,
                sObjectName = objectName,
                contentType = "CSV"
            };

            var namingStrategy = new DictionaryNamingStrategy();
            namingStrategy.Add("ops", "operation");
            namingStrategy.Add("sObjectName", "object");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                }
            };
            var serializedContent = JsonConvert.SerializeObject(jobRequest, settings);
            Console.WriteLine($"Serialized Request: {serializedContent}");
            var content = new StringContent(JsonConvert.SerializeObject(jobRequest, settings), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseBody}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create job. Response: {responseBody}");
            }
            JObject jsonResponse = JObject.Parse(responseBody);
            return jsonResponse["id"].ToString();
        }
        private async Task UploadBatchAsync(string jobId, string data)
        {
            string url = $"{_instanceUrl}{Constants.JOB_ENDPOINT}/{jobId}/batch";
            var content = new StringContent(data, Encoding.UTF8, "text/csv");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        private async Task CloseJobAsync(string jobId)
        {
            string url = $"{_instanceUrl}{Constants.JOB_ENDPOINT}/{jobId}";  // Added a slash for safety.
            var content = new StringContent($@"{{""state"":""Closed""}}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);  // Assuming PATCH is the right verb.
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to close job. Response: {responseBody}");
            }
        }

        private async Task<string> GetJobDetails(string jobId)
        {
            string url = $"{_instanceUrl}{Constants.JOB_STATUS}{jobId}";

            Console.WriteLine($"Fetching details from URL: {url}");  // Log the URL you're hitting

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Ensure the response is successful
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to get job details. Response: {responseBody}");
            }

            // Get the JSON content as string
            string jsonContent = await response.Content.ReadAsStringAsync();

            return jsonContent;
        }


        private async Task DisplayJobDetails(string jobId)
        {
            try
            {
                string jobDetails = await GetJobDetails(jobId);
                Console.WriteLine("Job Details:");
                Console.WriteLine(jobDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task AsyncBatchJob(string objectName, string operation, string data)
        {
            string jobId = await CreateJobAsync(objectName, operation);
            await UploadBatchAsync(jobId, data);
            await CloseJobAsync(jobId);
            await DisplayJobDetails(jobId);
        }
        // You can further extend this class with methods for checking batch status, retrieving results, etc.
    }

    // Naming strategy class for custom renaming
    public class DictionaryNamingStrategy : NamingStrategy
    {
        private Dictionary<string, string> renames;

        public DictionaryNamingStrategy()
        {
            renames = new Dictionary<string, string>();
        }

        public void Add(string originalName, string newName)
        {
            renames[originalName] = newName;
        }

        protected override string ResolvePropertyName(string name)
        {
            return renames.TryGetValue(name, out var newName) ? newName : name;
        }
    }
}
