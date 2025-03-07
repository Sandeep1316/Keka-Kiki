using System.Collections.Generic;
using System;

namespace KekaBot.kiki.Services.Models
{
    public class LeaveRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool AvailFloaterLeave { get; set; }
        public string Note { get; set; }
        public string? LeaveReason { get; set; }
        public List<LeaveSelection> Selection { get; set; } = new List<LeaveSelection>();
    }

    public class LeaveSelection
    {
        public int LeaveTypeId { get; set; }
        public int Count { get; set; }
    }
}
