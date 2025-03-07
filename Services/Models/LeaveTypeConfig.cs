namespace KekaBot.kiki.Services.Models
{
    /// <summary>
    /// Class LeaveTypeConfig.
    /// </summary>
    public class LeaveTypeConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveTypeConfig"/> class.
        /// </summary>
        public LeaveTypeConfig()
        {
            this.LeaveType = new LeaveType();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is setup complete.
        /// </summary>
        /// <value><c>true</c> if this instance is setup complete; otherwise, <c>false</c>.</value>
        public bool IsSetupComplete { get; set; }

        /// <summary>
        /// Gets or sets the type of the leave.
        /// </summary>
        /// <value>The type of the leave.</value>
        public LeaveType LeaveType { get; set; }
    }
}
