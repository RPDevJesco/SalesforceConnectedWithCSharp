namespace SalesforceConnectedWithCSharp
{
    class Constants
    {
        public const string CONSUMER_KEY = "ConnectedAppConsumerKey";
        public const string CONSUMER_SECRET = "ConnectedAppConsumerSecret";
        public const string USERNAME = "SalesforceLoginUserName";
        public const string TOKEN = "SecurityToken";
        public const string PASSWORD = "SalesforceLoginPassword";
        // Use https://login.salesforce.com/services/oauth2/token if production or Developer Edition (Not a sandbox)
        // use https://test.salesforce.com/services/oauth2/token if sandbox / scratch org
        // the below endpoint is the developer free edition endpoint, change to yours.
        public const string TOKEN_REQUEST_ENDPOINTURL = "https://na17-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        public static string TOKEN_REQUEST_QUERYURL = "/services/data/v58.0/query?q=";
        public static string TOKEN_REQUEST_POSTURL = "/services/data/v58.0/sobjects/";
    }
}