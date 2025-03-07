using System.Collections.Generic;

namespace KekaBot.kiki.Services.Models
{
    public class LeavePlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeavePlan"/> class.
        /// </summary>
        public LeavePlan()
        {
            this.Configuration = new List<LeaveTypeConfig>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of leave plan.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public List<LeaveTypeConfig> Configuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is default].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is default]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is IsYearStartDateBasedOnDOJ].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is IsYearStartDateBasedOnDOJ]; otherwise, <c>false</c>.
        /// </value>
        public bool IsYearStartDateBasedOnDOJ { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show keka leave policy explanation].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show keka leave policy explanation]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowKekaLeavePolicyExplanation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [add custom leave policy document].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add custom leave policy document]; otherwise, <c>false</c>.
        /// </value>
        public bool AddCustomLeavePolicyDocument { get; set; }

        /// <summary>
        /// Gets or sets the leave plan identifier
        /// </summary>
        /// <value>
        /// The leave plan identifier
        /// </value>
        public string Identifier { get; set; }
    }
}