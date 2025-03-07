﻿using System.Collections.Generic;
using System;
using KekaBot.kiki.Services.Enums;
using System.ComponentModel;

namespace KekaBot.kiki.Services.Models
{
    /// <summary>
    /// Leave employee summary view model.
    /// </summary>
    public class LeaveEmployeeSummaryViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// <value>
        /// The employee identifier.
        /// </value>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        public int PlanId { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets the annual quota.
        /// </summary>
        /// <value>
        /// The annual quota.
        /// </value>
        public TimePeriod AnnualQuota { get; set; }

        /// <summary>
        /// Gets or sets the available balance.
        /// </summary>
        /// <value>
        /// The available balance.
        /// </value>
        public TimePeriod AvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets the actual available balance.
        /// </summary>
        /// <value>
        /// The actual available balance.
        /// </value>
        public TimePeriod ActualAvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets the comp off balance.
        /// </summary>
        /// <value>
        /// The comp off balance.
        /// </value>
        public TimePeriod CompOffBalance { get; set; }

        /// <summary>
        /// Gets or sets the leave type configuration.
        /// </summary>
        /// <value>
        /// The leave type configuration.
        /// </value>
        public LeaveTypeConfig LeaveTypeConfig { get; set; }

    }
}


// <summary>
/// Represents the Time Period.
/// </summary>
public class TimePeriod
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimePeriod"/> class.
    /// </summary>
    public TimePeriod() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimePeriod"/> class.
    /// </summary>
    /// <param name="unit">Unit of Duration.</param>
    public TimePeriod(TimeDuration unit)
    {
        this.Unit = unit;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimePeriod"/> class.
    /// </summary>
    /// <param name="duration">the duration.</param>
    /// <param name="unit">Unit of Duration.</param>
    public TimePeriod(double duration, TimeDuration unit)
    {
        this.Duration = duration;
        this.Unit = unit;
    }

    /// <summary>
    /// Gets or sets the unit.
    /// </summary>
    /// <value>
    /// The unit.
    /// </value>
    public TimeDuration Unit { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    /// <value>
    /// The duration.
    /// </value>
    public double Duration { get; set; }

    public string DurationString
    {
        get { return this.ToString(); }
    }

    /// <summary>
    /// Gets the time period string.
    /// </summary>
    /// <returns>
    /// returns the time period string.
    /// </returns>
    public override string ToString()
    {
        return this.Duration + (this.Unit == TimeDuration.Days ? (Math.Abs(this.Duration) > 1 ? " days" : " day") : this.Unit == TimeDuration.Months ? (Math.Abs(this.Duration) > 1 ? " months" : " month") : this.Unit == TimeDuration.Weeks ? (Math.Abs(this.Duration) > 1 ? " weeks" : " week") : (Math.Abs(this.Duration) > 1 ? " hours" : " hour"));
    }
}

/// <summary>
/// Enum TimeDuration
/// </summary>
public enum TimeDuration
{
    None = 0,
    Hours,
    Days,
    [Description("Week")]
    Weeks,
    [Description("Month")]
    Months,
    [Description("HalfYear")]
    HalfYear,
    [Description("Year")]
    Year,
    [Description("Quarterly")]
    Quarterly,
    Minutes
}


