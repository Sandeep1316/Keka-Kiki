using System.ComponentModel;

namespace KekaBot.kiki.Services.Enums
{
    /// <summary>
    /// Enum Ticket Assignment Type.
    /// </summary>
    public enum TicketAssignmentType
    {
        /// <summary>
        /// The distribute equally.
        /// </summary>
        DistributeEqually,

        /// <summary>
        /// The distribute among selected.
        /// </summary>
        DistributeAmongSelected,
    }

    /// <summary>
    /// Enum follower type.
    /// </summary>
    public enum TicketAssigneeType
    {
        /// <summary>
        /// The employee.
        /// </summary>
        [Description("Employee")]
        Employee = 0,

        /// <summary>
        /// The reporting manager.
        /// </summary>
        [Description("Reporting Manager")]
        ReportingManager,

        /// <summary>
        /// The L2 manager.
        /// </summary>
        [Description("L2 Manager")]
        L2Manager,

        /// <summary>
        /// The Dotted line manager.
        /// </summary>
        [Description("Dotted Line Manager")]
        DottedLineManager,

        /// <summary>
        /// The department he
        /// </summary>
        [Description("Department Head")]
        DepartmentHead,

        /// <summary>
        /// The location head.
        /// </summary>
        [Description("Location Head")]
        LocationHead,
    }

    /// <summary>
    /// Enum Ticket Priority.
    /// </summary>
    public enum TicketPriority
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,

        /// <summary>
        /// The low.
        /// </summary>
        Low,

        /// <summary>
        /// The medium.
        /// </summary>
        Medium,

        /// <summary>
        /// The high.
        /// </summary>
        High,
    }
}
