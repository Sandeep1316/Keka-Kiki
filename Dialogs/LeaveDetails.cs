using KekaBot.kiki.Bots;

namespace KekaBot.kiki.Dialogs
{
    public class LeaveDetails : BaseDialog
    {
        public override string ActionType
        {
            get
            {
                return BotIntents.ApplyLeave;
            }
        }

        public string EmployeeId { get; set; }
        public string LeaveType { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Days { get; set; }
        public string Reason { get; set; }
    }
}