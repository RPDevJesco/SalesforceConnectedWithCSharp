namespace SalesforceConnectedWithCSharp.SalesforceDTO
{
    public class Case
    {
        public string ContactId { get; set; }
        public string AccountId { get; set; }
        public string AssetId { get; set; }
        public string ProductId { get; set; }
        public string EntitlementId { get; set; }
        public string SourceId { get; set; }
        public string BusinessHoursId { get; set; }
        public string ParentId { get; set; }
        public string SuppliedName { get; set; }
        public string SuppliedEmail { get; set; }
        public string SuppliedPhone { get; set; }
        public string SuppliedCompany { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Origin { get; set; }
        public string Subject { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public bool IsEscalated { get; set; }
        public string OwnerId { get; set; }
        public DateTime SlaStartDate { get; set; }
        public bool IsStopped { get; set; }
        public string ServiceContractId { get; set; }
        public string EngineeringReqNumber__c { get; set; }
        public string SLAViolation__c { get; set; }
        public string Product__c { get; set; }
        public string PotentialLiability__c { get; set; }
        public double Case_Age__c { get; set; }
    }
}