namespace SalesforceNetCore.SalesforceDTO
{
    public class Contact
    {
        public string AccountId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Salutation { get; set; }
        public string OtherStreet { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherPostalCode { get; set; }
        public string OtherCountry { get; set; }
        public double OtherLatitude { get; set; }
        public double OtherLongitude { get; set; }
        public string OtherGeocodeAccuracy { get; set; }
        public string MailingStreet { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingPostalCode { get; set; }
        public string MailingCountry { get; set; }
        public double MailingLatitude { get; set; }
        public double MailingLongitude { get; set; }
        public string MailingGeocodeAccuracy { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string OtherPhone { get; set; }
        public string AssistantPhone { get; set; }
        public string ReportsToId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string AssistantName { get; set; }
        public string LeadSource { get; set; }
        public DateTime Birthdate { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public bool HasOptedOutOfEmail { get; set; }
        public bool HasOptedOutOfFax { get; set; }
        public bool DoNotCall { get; set; }
        public string EmailBouncedReason { get; set; }
        public DateTime? EmailBouncedDate { get; set; }
        public string Jigsaw { get; set; }
        public string CleanStatus { get; set; }
        public string IndividualId { get; set; }
        public string Pronouns { get; set; }
        public string GenderIdentity { get; set; }
        public string Level__c { get; set; }
        public string Languages__c { get; set; }
    }
}