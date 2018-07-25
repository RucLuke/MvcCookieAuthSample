namespace HelloNetcore.Models
{
    public class ProcessConsentResult
    {
        public string RedirectUrl { get; set; }

        public bool IsRedirect => !string.IsNullOrEmpty(RedirectUrl);

        public string ValidationError { get; set; }

        public ConsentViewModel ViewModel { get; set; }
    }
}
