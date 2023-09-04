namespace SalesforceConnectedWithCSharp.SalesforceDTO
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
        public string BillingStreet { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCountry { get; set; }
        public double BillingLatitude { get; set; }
        public double BillingLongitude { get; set; }
        public string BillingGeocodeAccuracy { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingCountry { get; set; }
        public double ShippingLatitude { get; set; }
        public double ShippingLongitude { get; set; }
        public string ShippingGeocodeAccuracy { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string AccountNumber { get; set; }
        public Uri Website { get; set; }
        public string Sic { get; set; }
        public string Industry { get; set; }
        public decimal AnnualRevenue { get; set; }
        public int NumberOfEmployees { get; set; }
        public string Ownership { get; set; }
        public string TickerSymbol { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string Site { get; set; }
        public string OwnerId { get; set; }
        public string Jigsaw { get; set; }
        public string CleanStatus { get; set; }
        public string AccountSource { get; set; }
        public string DunsNumber { get; set; }
        public string Tradestyle { get; set; }
        public string NaicsCode { get; set; }
        public string NaicsDesc { get; set; }
        public string YearStarted { get; set; }
        public string SicDesc { get; set; }
        public string DandbCompanyId { get; set; }
        public string OperatingHoursId { get; set; }
        public string CustomerPriority__c { get; set; }
        public string SLA__c { get; set; }
        public string Active__c { get; set; }
        public double NumberofLocations__c { get; set; }
        public string UpsellOpportunity__c { get; set; }
        public string SLASerialNumber__c { get; set; }
        public DateTime? SLAExpirationDate__c { get; set; }

        public class Address
        {
            // Salesforce Address fields typically include the following subfields:
            public string City { get; set; }
            public string Country { get; set; }
            public string GeocodeAccuracy { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string PostalCode { get; set; }
            public string State { get; set; }
            public string Street { get; set; }
        }
    }
}