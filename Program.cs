using System.Net;

namespace SalesforceConnectedWithCSharp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SalesforceClient client = new SalesforceClient();

            await client.AuthorizeAsync();
            //ExcelDataReader excelDataReader = new ExcelDataReader(client.Token, client.InstanceUrl);
            SalesforceCRUD crudOperations = new SalesforceCRUD(client.Token, client.InstanceUrl);

            var account = await crudOperations.GetAsync("SELECT Id, Name FROM Account LIMIT 1");
            var accountData = new Dictionary<string, object>
            {
                { "Name", "John Smith" },
                { "Industry", "Software" },
                { "AccountNumber", "CC977211-E" }
            };
            // recordId is an optional parameter for UpsertAsync.Without the Id, it will do an insert operation, with it, it will update.
            await crudOperations.UpsertAsync("Account", accountData);
            //await crudOperations.DeleteAsync("Account", "001Dn00000erJ1LIAU");
            //await excelDataReader.UpsertDataFromExcel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AccountObjectWithId.xlsx", "Account");
            Console.WriteLine(account);
        }
    }
}