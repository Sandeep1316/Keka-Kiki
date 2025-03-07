using KekaBot.kiki.Services.Enums;

namespace KekaBot.kiki.Services.Models
{
    public class TicketCategoryFollowerView
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the follower identifier.
        /// </summary>
        /// <value>
        /// The follower identifier.
        /// </value>
        public int? FollowerId { get; set; }

        /// <summary>
        /// Gets or sets the type of the ticket assignee.
        /// </summary>
        /// <value>
        /// The type of the ticket assignee.
        /// </value>
        public TicketAssigneeType TicketAssigneeType { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the profile image url.
        /// </summary>
        /// <value>
        /// The profile image url.
        /// </value>
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle { get; set; }
    }
}
