using Microsoft.AspNetCore.Mvc;

namespace PFDTeam2.Models
{
    public class EventModel
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Progress { get; set; } = "not-started";




        public List<GoogleAccount> GoogleAccounts { get; set; }

        public string SelectedGoogleAccount { get; set; }
    }
}
