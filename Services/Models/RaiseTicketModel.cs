using System.Collections.Generic;

namespace KekaBot.kiki.Services.Models
{
    public class RaiseTicketModel
    {
        public int TicketCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Excerpt { get; set; }
        public int RequestedFor { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public List<int> MentionedEmployeeIds { get; set; } = new List<int>();
    }
}
