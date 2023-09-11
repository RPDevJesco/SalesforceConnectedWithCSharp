namespace SalesforceNetCore.SalesforceDTO
{
    public class Opportunity
    {
        public string AccountId { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StageName { get; set; }
        public decimal Amount { get; set; }
        public double Probability { get; set; }
        public double TotalOpportunityQuantity { get; set; }
        public DateTime CloseDate { get; set; }
        public string Type { get; set; }
        public string NextStep { get; set; }
        public string LeadSource { get; set; }
        public string ForecastCategoryName { get; set; }
        public string CampaignId { get; set; }
        public string Pricebook2Id { get; set; }
        public string OwnerId { get; set; }
        public string ContractId { get; set; }
        public string DeliveryInstallationStatus__c { get; set; }
        public string TrackingNumber__c { get; set; }
        public string OrderNumber__c { get; set; }
        public string CurrentGenerators__c { get; set; }
        public string MainCompetitors__c { get; set; }
    }
}