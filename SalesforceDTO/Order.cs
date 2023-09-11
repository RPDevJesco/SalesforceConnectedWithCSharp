namespace SalesforceConnectedWithCSharp.SalesforceDTO
{
    public class Order
    {
        public string OwnerId { get; set; }
        public string ContractId { get; set; }
        public string AccountId { get; set; }
        public string Pricebook2Id { get; set; }
        public string OpportunityId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string CustomerAuthorizedById { get; set; }
        public DateTime CustomerAuthorizedDate { get; set; }
        public string CompanyAuthorizedById { get; set; }
        public DateTime CompanyAuthorizedDate { get; set; }
        public string Type { get; set; }
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
        public string Name { get; set; }
        public DateTime PoDate { get; set; }
        public string PoNumber { get; set; }
        public string OrderReferenceNumber { get; set; }
        public string BillToContactId { get; set; }
        public string ShipToContactId { get; set; }
    }
}