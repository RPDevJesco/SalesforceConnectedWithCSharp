using System.Net;
using Newtonsoft.Json;

using SalesforceConnectedWithCSharp.DataReader;
using SalesforceConnectedWithCSharp.Helpers;
using SalesforceConnectedWithCSharp.SalesforceAPI;

namespace SalesforceConnectedWithCSharp
{
    class Program
    {
        static string? token;
        static string? instanceUrl;
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SalesforceClient client = new SalesforceClient();
            await client.AuthorizeAsync();
            token = client.Token;
            instanceUrl = client.InstanceUrl;
            await DownloadSalesforceData();
        }

        static async Task GetDataFromSalesforce()
        {
            SalesforceCRUD crudOperations = new SalesforceCRUD(token, instanceUrl);
            var account = await crudOperations.GetAsync("SELECT Id, Name FROM Account LIMIT 1");
        }

        static async Task AddDataToSalesforce()
        {
            SalesforceCRUD crudOperations = new SalesforceCRUD(token, instanceUrl);
            var accountData = new Dictionary<string, object>
            {
                { "Name", "John CSharp" },
                { "Industry", "Software" },
                { "AccountNumber", "CC977211-E" }
            };
            // recordId is an optional parameter for UpsertAsync.
            // Without the Id, it will do an insert operation.
            // With an Id, it will update.
            await crudOperations.UpsertAsync("Account", accountData);
        }

        static async Task DeleteDataFromSalesforce(string sObjectName, string recordId)
        {
            SalesforceCRUD crudOperations = new SalesforceCRUD(token, instanceUrl);
            await crudOperations.DeleteAsync(sObjectName, recordId);
        }

        static async Task AddDataToSalesforceWithDataReader()
        {
            ExcelDataReader excelDataReader = new ExcelDataReader(token, instanceUrl);
            await excelDataReader.UpsertExcelData(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.xlsx", "Account");
        }

        static async Task BatchAPIUsage()
        {
            SalesforceBatchAPI batchApiClient = new SalesforceBatchAPI(token, instanceUrl);
            var accountData = new Dictionary<string, object>
            {
                { "Name", "John CSharp2" },
                { "Industry", "Software2" },
                { "AccountNumber", "CC977211-E2" }
            };
            var csvResult = DictionaryToCSV.Convert(accountData);
            await batchApiClient.AsyncBatchJob(
                "Account", 
                BatchOperations.GetOperationDescription(BatchOperations.Operations.Insert),
                csvResult);
        }

        static async Task BatchAPIDataReaderUsage()
        {
            ExcelDataReader excelDataReader = new ExcelDataReader();
            SalesforceBatchAPI batchApiClient = new SalesforceBatchAPI(token, instanceUrl);
            var excelData = await excelDataReader.ParseExcel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.xlsx", "Account");
            var accountData = DictionaryToCSV.Convert(excelData);
            await batchApiClient.AsyncBatchJob(
                "Account",
                BatchOperations.GetOperationDescription(BatchOperations.Operations.Insert),
                accountData);
        }

        static async Task DownloadSalesforceData()
        {
            SalesforceFileDownloader fileDownloader = new SalesforceFileDownloader(token, instanceUrl);

            var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SalesforceData");
            Directory.CreateDirectory(directoryPath);

            var filename = "Accounts.json";
            await fileDownloader.DownloadAndSaveDataAsJsonAsync(directoryPath, filename, SOQLGeneration.GenerateSoqlForAccount());
        }
    }
}