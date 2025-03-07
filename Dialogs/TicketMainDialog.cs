using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Kiki.CognitiveModels;
using System.Net.Sockets;

namespace Kiki.Dialogs
{
    public class TicketMainDialog : ComponentDialog
    {
        private readonly TicketRecognizer _luisRecognizer;
        private readonly ILogger _logger;

        public TicketMainDialog(TicketRecognizer luisRecognizer, TicketDialog ticketDialog, ILogger<TicketMainDialog> logger)
            : base(nameof(TicketMainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(ticketDialog);

            var waterfallSteps = new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. Please check your settings."), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }

            var messageText = stepContext.Options?.ToString() ?? "How can I assist you with your tickets today?\nSay 'Create a new ticket', 'Check my ticket status', or 'Update ticket status'.";
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(messageText) }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                return await stepContext.BeginDialogAsync(nameof(TicketDialog), new TicketDetails() { Action = "Add" }, cancellationToken);
            }

            var luisResult = await _luisRecognizer.RecognizeAsync<Ticket>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case Ticket.Intent.CreateTicket:
                    var ticketDetails = new TicketDetails()
                    {
                        TicketType = luisResult.TicketDetails.TicketType,
                        TicketTitle = luisResult.TicketDetails.TicketTitle,
                        IssueDescription = luisResult.TicketDetails.Description,
                        Priority = luisResult.TicketDetails.Priority,
                        Action = "Add"
                    };
                    return await stepContext.BeginDialogAsync(nameof(TicketDialog), ticketDetails, cancellationToken);

                case Ticket.Intent.GetTicketStatus:
                    return await HandleGetTicketStatusAsync(stepContext, luisResult, cancellationToken);

                case Ticket.Intent.UpdateTicketStatus:
                    return await HandleUpdateTicketStatusAsync(stepContext, luisResult, cancellationToken);

                default:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, I didn't understand your request."), cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleGetTicketStatusAsync(WaterfallStepContext stepContext, Ticket luisResult, CancellationToken cancellationToken)
        {
            var ticketId = luisResult.TicketDetails.TicketId;
            if (string.IsNullOrEmpty(ticketId))
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please provide a valid ticket ID."), cancellationToken);
            }
            else
            {
                var statusMessage = $"Your ticket {ticketId} is currently in 'In Progress' status."; // Replace with API call
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(statusMessage), cancellationToken);
            }
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleUpdateTicketStatusAsync(WaterfallStepContext stepContext, Ticket luisResult, CancellationToken cancellationToken)
        {
            var ticketId = luisResult.TicketStatusDetails.TicketId;
            var newStatus = luisResult.TicketStatusDetails.Status;

            if (string.IsNullOrEmpty(ticketId) || string.IsNullOrEmpty(newStatus))
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Please provide a valid ticket ID and new status."), cancellationToken);
            }
            else
            {
                var updateMessage = $"Your ticket {ticketId} status has been updated to '{newStatus}'."; // Replace with actual API call
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(updateMessage), cancellationToken);
            }
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = "What else can I assist you with?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}