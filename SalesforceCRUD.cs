namespace SalesforceConnectedWithCSharp
{
    public class SalesforceCRUD
    {
        private readonly string _token;
        private readonly string _instanceUrl;
        private SalesforceClient _client = new SalesforceClient();

        public SalesforceCRUD(string token, string instanceUrl)
        {
            _token = token;
            _instanceUrl = instanceUrl;
        }

        public async Task<string> GetAsync(string query)
        {
            return await _client.AsyncGetRequest(_token, _instanceUrl, query);
        }

        public async Task<string> CreateAsync(string objectName, object data)
        {
            return await _client.AsyncCreateRequest(_token, _instanceUrl, objectName, data);
        }

        public async Task<string> UpdateAsync(string objectName, string id, object data)
        {
            return await _client.AsyncUpdateRequest(_token, _instanceUrl, objectName, id, data);
        }

        public async Task<string> DeleteAsync(string objectName, string id)
        {
            return await _client.AsyncDeleteRequest(_token, _instanceUrl, objectName, id);
        }
    }
}
