using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KekaBot.kiki.Services;
using KekaBot.kiki.Services.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Kiki.Dialogs
{
    public class TicketDialog : CancelAndHelpDialog
    {
        private string TicketTypeStepMsgText = "What type of ticket do you want to create or update?";
        private const string TicketTitleStepMsgText = "Please provide a title of your issue.";
        private const string IssueDescriptionStepMsgText = "Please provide a brief description of your issue.";
        private const string TicketIdStepMsgText = "Please provide the ticket ID (if updating or retrieving a ticket).";
        private const string PriorityStepMsgText = "What priority would you like to assign for your issue (Low, Medium, High).";
        private KekaServiceClient KekaServiceClient;
        private IEnumerable<TicketCategoryListItem> ticketCategories;

        public TicketDialog(KekaServiceClient kekaServiceClient)
            : base(nameof(TicketDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            var waterfallSteps = new WaterfallStep[]
            {
                TicketTypeStepAsync,
                TicketIdStepAsync,
                TitleStepAsync,
                IssueDescriptionStepAsync,
                PriorityStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            InitialDialogId = nameof(WaterfallDialog);
            this.KekaServiceClient = kekaServiceClient;
        }

        private async Task<DialogTurnResult> TicketTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;

            if (string.IsNullOrEmpty(ticketDetails.TicketType))
            {
                var response = await this.KekaServiceClient.GetAllTicketCategoriesAsync();
                this.ticketCategories = response.Data;
                TicketTypeStepMsgText += Environment.NewLine + "Possible Options are: " + Environment.NewLine;
                foreach (var ticketCategory in this.ticketCategories.Take(5))
                {
                    TicketTypeStepMsgText += ticketCategory.Name + Environment.NewLine;
                }
                var promptMessage = MessageFactory.Text(TicketTypeStepMsgText, TicketTypeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(ticketDetails.TicketType, cancellationToken);
        }

        private async Task<DialogTurnResult> TicketIdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;
            ticketDetails.TicketType = (string)stepContext.Result;

            if (ticketDetails.Action == "Update" || ticketDetails.Action == "Get")
            {
                if (string.IsNullOrEmpty(ticketDetails.TicketId))
                {
                    var promptMessage = MessageFactory.Text(TicketIdStepMsgText, TicketIdStepMsgText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                }
            }

            return await stepContext.NextAsync(ticketDetails.TicketId, cancellationToken);
        }

        private async Task<DialogTurnResult> TitleStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;
            ticketDetails.TicketId = (string)stepContext.Result;

            if (ticketDetails.Action == "Add" || ticketDetails.Action == "Update")
            {
                if (string.IsNullOrEmpty(ticketDetails.TicketTitle))
                {
                    var promptMessage = MessageFactory.Text(TicketTitleStepMsgText, TicketTitleStepMsgText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                }
            }

            return await stepContext.NextAsync(ticketDetails.TicketTitle, cancellationToken);
        }

        private async Task<DialogTurnResult> IssueDescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;
            ticketDetails.TicketTitle = (string)stepContext.Result;

            if (ticketDetails.Action == "Add" || ticketDetails.Action == "Update")
            {
                if (string.IsNullOrEmpty(ticketDetails.IssueDescription))
                {
                    var promptMessage = MessageFactory.Text(IssueDescriptionStepMsgText, IssueDescriptionStepMsgText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                }
            }

            return await stepContext.NextAsync(ticketDetails.IssueDescription, cancellationToken);
        }

        private async Task<DialogTurnResult> PriorityStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;
            ticketDetails.IssueDescription = (string)stepContext.Result;

            if (ticketDetails.Action == "Add" || ticketDetails.Action == "Update")
            {
                if (string.IsNullOrEmpty(ticketDetails.Priority))
                {
                    var promptMessage = MessageFactory.Text(PriorityStepMsgText, PriorityStepMsgText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                }
            }

            return await stepContext.NextAsync(ticketDetails.Priority, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ticketDetails = (TicketDetails)stepContext.Options;
            ticketDetails.Priority = (string)stepContext.Result;
            ticketDetails.TicketId = new Random().NextInt64().ToString();

            var messageText = $"Please confirm your ticket details:{Environment.NewLine}" +
                  $"Action: {ticketDetails.Action}{Environment.NewLine}" +
                  $"Ticket Type: {ticketDetails.TicketType}{Environment.NewLine}" +
                  (!string.IsNullOrEmpty(ticketDetails.TicketId) ? $"Ticket ID: {ticketDetails.TicketId}{Environment.NewLine}" : "") +
                  (!string.IsNullOrEmpty(ticketDetails.TicketTitle) ? $"Title: {ticketDetails.TicketTitle}{Environment.NewLine}" : "") +
                  (!string.IsNullOrEmpty(ticketDetails.IssueDescription) ? $"Issue: {ticketDetails.IssueDescription}{Environment.NewLine}" : "") +
                  (!string.IsNullOrEmpty(ticketDetails.Priority) ? $"Issue: {ticketDetails.Priority}{Environment.NewLine}" : "") +
                  "Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var ticketDetails = (TicketDetails)stepContext.Options;

                var response = await this.KekaServiceClient.GetAllTicketCategoriesAsync();
                this.ticketCategories = response.Data;

                var ticketCategory = this.ticketCategories.Where(_ => _.Name == ticketDetails.TicketType).FirstOrDefault();

                RaiseTicketModel raiseTicketModel = new RaiseTicketModel
                {
                    TicketCategoryId = ticketCategory.Id,
                    Title = ticketDetails.TicketTitle,
                    Description = ticketDetails.IssueDescription,
                    Priority = ticketDetails.Priority,
                };

                await this.KekaServiceClient.PostTicket(raiseTicketModel);

                return await stepContext.EndDialogAsync(ticketDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }

    public class TicketDetails
    {
        public string Action { get; set; } // Add, Update, Get
        public string TicketType { get; set; }
        public string TicketId { get; set; }
        public string TicketTitle { get; set; }
        public string IssueDescription { get; set; }
        public string Priority { get; set; }
    }
}
