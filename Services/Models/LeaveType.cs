using System.Collections.Generic;
using System.ComponentModel;

namespace KekaBot.kiki.Services.Models
{
    /// <summary>
    /// Represents the Model Class Leave Type
    /// </summary>
    public class LeaveType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveType"/> class.
        /// </summary>
        public LeaveType()
        {
            this.Reasons = new List<string>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is paid].
        /// </summary>
        /// <value><c>true</c> if [is paid]; otherwise, <c>false</c>.</value>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is sick].
        /// </summary>
        /// <value><c>true</c> if [is sick]; otherwise, <c>false</c>.</value>
        public bool IsSick { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is statutory.
        /// </summary>
        /// <value><c>true</c> if this instance is statutory; otherwise, <c>false</c>.</value>
        public bool IsStatutory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is restricted to gender.
        /// </summary>
        /// <value><c>true</c> if this instance is restricted to gender; otherwise, <c>false</c>.</value>
        public bool IsRestrictedToGender { get; set; }

        /// <summary>
        /// Gets or sets the restricted gender.
        /// </summary>
        /// <value>The restricted gender.</value>
        public Gender RestrictedGender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is restricted to marital status.
        /// </summary>
        /// <value><c>true</c> if this instance is restricted to marital status; otherwise, <c>false</c>.</value>
        public bool IsRestrictedToMaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the restricted marital status.
        /// </summary>
        /// <value>The restricted marital status.</value>
        public MaritalStatus RestrictedMaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the type of the system leave.
        /// </summary>
        /// <value>The type of the system leave.</value>
        public SystemLeaveType SystemLeaveType { get; set; }

        /// <summary>
        /// Gets or sets the type of the time off.
        /// </summary>
        /// <value>The type of the time off.</value>
        public TimeOffType TimeOffType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [system generated].
        /// </summary>
        /// <value><c>true</c> if [system generated]; otherwise, <c>false</c>.</value>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reason required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reason required; otherwise, <c>false</c>.
        /// </value>
        public bool IsReasonRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show description].
        /// </summary>
        /// <value><c>true</c> if [show description]; otherwise, <c>false</c>.</value>
        public bool ShowLeaveDescription { get; set; }

        /// <summary>
        /// Gets or sets the reasons.
        /// </summary>
        /// <value>
        /// The reasons.
        /// </value>
        public List<string> Reasons { get; set; }
    }
}

/// <summary>
/// Enum Gender
/// </summary>
public enum Gender
{
    [Description("Not Specified")]
    NotSpecified = 0,
    [Description("Male")]
    Male = 1,
    [Description("Female")]
    Female = 2,
    [Description("Non-binary")]
    Nonbinary = 3,
    [Description("Prefer not to respond")]
    PreferNotToRespond = 4,
    [Description("Transgender")]
    Transgender = 5
}

/// <summary>
/// Enum SystemLeaveType
/// </summary>
public enum SystemLeaveType
{
    None = 0,
    Floater = 1,
    Special = 2,
    Compoff = 3
}

public enum TimeOffType
{
    [Description("Regular Leave Type")]
    Regular = 0,
    [Description("Incident Leave Type")]
    Incident = 1,
    [Description("Unpaid Leave Type")]
    Unpaid = 2,
    [Description("Compensatory Leave Type")]
    Compoff = 3
}

/// <summary>
/// Enum Gender
/// </summary>
public enum MaritalStatus
{
    None = 0,
    [Description("Single")]
    Single = 1,
    [Description("Married")]
    Married = 2,
    [Description("Widowed")]
    Widowed = 3,
    [Description("Separated")]
    Separated = 4
}

