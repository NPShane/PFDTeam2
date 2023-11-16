using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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




        [Required(ErrorMessage = "Please select a Google Account")]
        public List<GoogleAccount> GoogleAccount { get; set; }

        public string SelectedGoogleAccount { get; set; }
    }
}
