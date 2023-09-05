using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesforceConnectedWithCSharp.SalesforceAPI
{
    public class SalesforceFileDownloader
    {
        private string _accessToken;
        private string _instanceUrl;
        private HttpClient _httpClient;

        public SalesforceFileDownloader(string accessToken, string instanceUrl)
        {
            _accessToken = accessToken;
            _instanceUrl = instanceUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
        }

        public async Task<IEnumerable<JObject>> GetAttachmentsAsync(string query)
        {
            var endpointUrl = $"{_instanceUrl}{Constants.TOKEN_REQUEST_QUERYURL}{query}";
            var response = await _httpClient.GetAsync(endpointUrl);
            var json = await response.Content.ReadAsStringAsync();
            var jobject = JObject.Parse(json);
            return jobject["records"].ToObject<IEnumerable<JObject>>();
        }

        public async Task<IEnumerable<JObject>> GetFilesAsync(string query)
        {
            var endpointUrl = $"{_instanceUrl}{Constants.TOKEN_REQUEST_QUERYURL}{query}";
            var response = await _httpClient.GetAsync(endpointUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create job. Response: {response}");
            }
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"json: {json}");
            var jobject = JObject.Parse(json);
            return jobject["records"].ToObject<IEnumerable<JObject>>();
        }

        public async Task DownloadAndSaveDataAsJsonAsync(string directoryPath, string filename, string query)
        {
            var records = await GetFilesAsync(query);

            // Serialize the records with indented formatting
            var jsonData = JsonConvert.SerializeObject(records, Formatting.Indented);

            var fullPath = Path.Combine(directoryPath, filename);
            await File.WriteAllTextAsync(fullPath, jsonData);
        }

        public async Task DownloadAndSaveAttachmentAsync(string attachmentId, string savePath)
        {
            var response = await _httpClient.GetAsync($"{_instanceUrl}{Constants.TOKEN_REQUEST_POSTURL}");
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"fileBytes: {fileBytes}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create job. Response: {response}");
            }
            await File.WriteAllBytesAsync(savePath, fileBytes);
        }

        public async Task DownloadAndSaveFileAsync(string contentUrl, string savePath)
        {
            var response = await _httpClient.GetAsync($"{_instanceUrl}{contentUrl}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to create job. Response: {response}");
            }
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"fileBytes: {fileBytes}");
            await File.WriteAllBytesAsync(savePath, fileBytes);
        }

        public async Task BatchDownloadFilesAndAttachments(string directoryPath, string query)
        {
            var attachments = await GetAttachmentsAsync(query);
            foreach (var attachment in attachments)
            {
                var attachmentId = attachment["Id"].ToString();
                var fileName = attachment["Name"].ToString();
                var savePath = Path.Combine(directoryPath, fileName);
                await DownloadAndSaveAttachmentAsync(attachmentId, savePath);
            }

            var files = await GetFilesAsync(query);
            foreach (var file in files)
            {
                if (file["ContentUrl"] == null) break;
                var fileUrl = file["ContentUrl"].ToString();
                var fileName = file["Title"].ToString();
                var savePath = Path.Combine(directoryPath, fileName);
                await DownloadAndSaveFileAsync(fileUrl, savePath);
            }
        }
    }
}