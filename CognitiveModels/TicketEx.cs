using System.Linq;

namespace Kiki.CognitiveModels
{
    public partial class Ticket
    {
        public (string TicketId, string Status) TicketStatusDetails
        {
            get
            {
                var ticketId = Entities?.TicketId?.FirstOrDefault();
                var status = Entities?.Status?.FirstOrDefault();
                return (ticketId, status);
            }
        }

        public (string TicketId, string Status, string TicketType, string Priority, string TicketTitle, string Description) TicketDetails
        {
            get
            {
                var ticketId = Entities?.TicketId?.FirstOrDefault();
                var status = Entities?.Status?.FirstOrDefault();
                var ticketType = Entities?.TicketType?.FirstOrDefault();
                var priority = Entities?.Priority?.FirstOrDefault();
                var title = Entities?.Title?.FirstOrDefault();
                var description = Entities?.Description?.FirstOrDefault();
                return (ticketId, status, ticketType, priority, title, description);
            }
        }

        public string TicketId
            => Entities?.TicketId?.FirstOrDefault();

        public string TicketType
            => Entities?.TicketType?.FirstOrDefault();

        public string Status
            => Entities?.Status?.FirstOrDefault();

        public string Priority
            => Entities?.Priority?.FirstOrDefault();

        public string Description
            => Entities?.Description?.FirstOrDefault();

        public string UserId
            => Entities?.UserId?.FirstOrDefault();

        public string CreatedDate
            => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];

        public string UpdatedDate
            => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];
    }
}
