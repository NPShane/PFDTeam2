using System.ComponentModel.DataAnnotations;

namespace PFDTeam2.Models
{
    public class Feedback
    {
        [Display(Name = "Feedback ID")]
        public int FeedbackId { get; set; }

        [Display(Name = "Text")]
        public string? Text { get; set; }

        [Display(Name = "Relevance")]
        public bool IsRelevant { get; set; } // 0 Irrelevant 1 Relevant

        [Display(Name = "Positivity")]
        public bool IsPositive { get; set; } // 0 Negative  1 positive

        [Display(Name = "Action Taken")]
        public bool ActionTaken { get; set; } // 0 not yet 1 responded

        [Display(Name = "Response")]
        public string? Response { get; set; }
    }
}
