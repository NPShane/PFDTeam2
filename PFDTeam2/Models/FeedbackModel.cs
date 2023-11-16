namespace PFDTeam2.Models
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public string? Text { get; set; }
        public bool IsRelevant { get; set; }
        public bool ActionTaken { get; set; }
    }
}
