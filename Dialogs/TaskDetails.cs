using KekaBot.kiki.Bots;

namespace KekaBot.kiki.Dialogs
{
    public class TaskDetails : BaseDialog
    {
        public override string ActionType
        {
            get
            {
                return BotIntents.AddTask;
            }
        }

        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string DueDate { get; set; }
        public Status? Status { get; set; }
        public string Priority { get; set; }
    }
}

public enum Status
{
    NotStarted,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}
