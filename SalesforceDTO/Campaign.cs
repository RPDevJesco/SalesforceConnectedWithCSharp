namespace SalesforceConnectedWithCSharp.SalesforceDTO
{
    public class Campaign
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ExpectedRevenue { get; set; }
        public decimal BudgetedCost { get; set; }
        public decimal ActualCost { get; set; }
        public double ExpectedResponse { get; set; }
        public double NumberSent { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string CampaignImageId { get; set; }
        public string OwnerId { get; set; }
        public string CampaignMemberRecordTypeId { get; set; }
    }
}