namespace SalesforceConnectedWithCSharp
{
    class Constants
    {
        public const string CONSUMER_KEY = "3MVG9ux34Ig8G5eokbfRLyIEqyxGz3oAd7NmDG5IUpTFiNfcXN3CVAuob4gWMQ9h2OAegUrQqUPoMFXzkSTsP";
        public const string CONSUMER_SECRET = "3B80859267D7D47D33D0A252750BBBF326182790BA07567408D7915327B5D195";
        public const string USERNAME = "jesse.glover@gamedevmadeeasy.com";
        public const string TOKEN = "6inK4OsywiVV4hMvJeUNIeMl";
        public const string PASSWORD = "GDMEJesc0!@!";
        // Use https://login.salesforce.com/services/oauth2/token if production or Developer Edition (Not a sandbox)
        // use https://test.salesforce.com/services/oauth2/token if sandbox / scratch org
        // the below endpoint is the developer free edition endpoint.
        public const string TOKEN_REQUEST_ENDPOINTURL = "https://gdme2-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        public static string TOKEN_REQUEST_QUERYURL = "/services/data/v58.0/query?q=";
        public static string TOKEN_REQUEST_POSTURL = "/services/data/v58.0/sobjects/";
    }
}
