namespace KekaBot.kiki.Services.Models;

using KekaBot.kiki.Services.Enums;
using System;
using System.Collections.Generic;

/// <summary>
/// Class Ticket category list item.
/// </summary>
public class TicketCategoryListItem
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the parent category identifier.
    /// </summary>
    /// <value>
    /// The parent category identifier.
    /// </value>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets the value indicating whether this instance is parent category.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is parent category; otherwise, <c>false</c>.
    /// </value>
    public bool IsParentCategory { get; set; }

    /// <summary>
    /// Gets or sets the parent category name.
    /// </summary>
    /// <value>
    /// The parent category name.
    /// </value>
    public string ParentCategoryName { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets enable assinees as followers.
    /// </summary>
    /// <value>
    /// Enable assignees as followers.
    /// </value>
    public bool EnableAssigneesAsFollowers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is on hold status enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is on hold status enabled; otherwise, <c>false</c>.
    /// </value>
    public bool IsOnHoldStatusEnabled { get; set; }

    /// <summary>
    /// Gets or sets the ticket assignment settings.
    /// </summary>
    /// <value>
    /// The ticket assignment settings.
    /// </value>
    public TicketAssignmentSettings TicketAssignmentSettings { get; set; }

    /// <summary>
    /// Gets or sets the followers.
    /// </summary>
    /// <value>
    /// The followers.
    /// </value>
    public List<TicketCategoryFollowerView> Followers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is disabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
    /// </value>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the ticket priorities.
    /// </summary>
    /// <value>
    /// The ticket priorities.
    /// </value>
    public List<TicketPriority> AllowedPriorities { get; set; }

    /// <summary>
    /// Gets or sets the default priority.
    /// </summary>
    /// <value>
    /// The default priority.
    /// </value>
    public TicketPriority DefaultPriority { get; set; }

    /// <summary>
    /// Gets or sets the sub categories count.
    /// </summary>
    /// <value>
    /// The sub categories count.
    /// </value>
    public int SubCategoriesCount { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    /// <value>
    /// Category Created On.
    /// </value>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the busness hours identifier.
    /// </summary>
    /// <value>
    /// The busness hours identifier.
    /// </value>
    public Guid BusinessHoursId { get; set; }
}