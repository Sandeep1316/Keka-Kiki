using KekaBot.kiki.Services.Enums;
using System.Collections.Generic;

namespace KekaBot.kiki.Services.Models
{
    /// <summary>
    /// Class to represent ticket assignment settings.
    /// </summary>
    public class TicketAssignmentSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketAssignmentSettings"/> class.
        /// </summary>
        public TicketAssignmentSettings()
        {
            this.DefaultAssigneeIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets the ticket assignment.
        /// </summary>
        /// <value>
        /// The ticket assignment.
        /// </value>
        public TicketAssignmentType TicketAssignmentType { get; set; }

        /// <summary>
        /// Gets or sets the default assignee id.
        /// </summary>
        /// <value>
        /// The default assignee id.
        /// </value>
        public List<int> DefaultAssigneeIds { get; set; }
    }
}

