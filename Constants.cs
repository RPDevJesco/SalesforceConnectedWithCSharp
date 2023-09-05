namespace SalesforceConnectedWithCSharp
{
    class Constants
    {
        // We simplify the salesforce version to be it's own string to make it so updates are easier.
        public const string SalesforceVersionNumber = "58.0";
        public const string CONSUMER_KEY = "ConnectedAppConsumerKey";
        public const string CONSUMER_SECRET = "ConnectedAppConsumerSecret";
        public const string USERNAME = "SalesforceLoginUserName";
        public const string TOKEN = "SalesforceSecuritytoken";
        public const string PASSWORD = "SalesforceLoginPassword";
        // Use https://login.salesforce.com/services/oauth2/token if production or Developer Edition (Not a sandbox)
        // use https://test.salesforce.com/services/oauth2/token if sandbox / scratch org
        // the below endpoint is the developer free edition endpoint.
        public const string TOKEN_REQUEST_ENDPOINTURL = "https://na18-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        public static string TOKEN_REQUEST_QUERYURL = "/services/data/v" + SalesforceVersionNumber + "/query?q=";
        public static string TOKEN_REQUEST_POSTURL = "/services/data/v" + SalesforceVersionNumber + "/sobjects/";
        public static string JOB_ENDPOINT = $"/services/async/" + SalesforceVersionNumber + "/job";
        public static string JOB_STATUS = $"/services/data/v" + SalesforceVersionNumber + "/jobs/ingest/";
    }
}
