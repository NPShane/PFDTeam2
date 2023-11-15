using Microsoft.AspNetCore.Mvc;

namespace PFDTeam2.Models
{
    public class EventModel
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public List<GoogleAccount> GoogleAccounts { get; set; }

        public string SelectedGoogleAccount { get; set; }
    }
}
