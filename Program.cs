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

            SalesforceCRUD crudOperations = new SalesforceCRUD(client.Token, client.InstanceUrl);

            var account = await crudOperations.GetAsync("SELECT Id, Name FROM Account LIMIT 1");
            var accountData = new Dictionary<string, object>
            {
                { "Name", "John Doe" },
                { "Industry", "Software" }
            };
            await crudOperations.CreateAsync("Account", accountData);
            
            Console.WriteLine(account);
        }
    }
}