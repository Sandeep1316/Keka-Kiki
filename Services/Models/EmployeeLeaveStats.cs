using System;
using System.Collections.Generic;

namespace KekaBot.kiki.Services.Models
{
    /// <summary>
    /// class employee leave stats
    /// </summary>
    public class EmployeeLeaveStats
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeLeaveStats"/> class.
        /// </summary>
        public EmployeeLeaveStats()
        {
            this.LeavePlan = new LeavePlan();
            this.LeaveSummaries = new List<LeaveEmployeeSummaryViewModel>();
        }

        /// <summary>
        /// Gets or sets the leave summaries.
        /// </summary>
        /// <value>
        /// The leave summaries.
        /// </value>
        public List<LeaveEmployeeSummaryViewModel> LeaveSummaries { get; set; }

        /// <summary>
        /// Gets or sets the leave plan.
        /// </summary>
        /// <value>
        /// The leave plan.
        /// </value>
        public LeavePlan LeavePlan { get; set; }

        /// <summary>
        /// Gets or sets the year start date.
        /// </summary>
        /// <value>
        /// The year start date.
        /// </value>
        public DateTime YearStartDate { get; set; }

        /// <summary>
        /// Gets or sets the compoff requests count.
        /// </summary>
        /// <value>
        /// The compoff requests count.
        /// </value>
        public int CompoffRequestsCount { get; set; }
    }
}
