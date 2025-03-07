// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KekaBot.kiki.Bots;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Kiki.Dialogs
{
    public class LeaveDialog : CancelAndHelpDialog
    {
        private const string LeaveTypeStepMsgText = "What type of leave would you like to apply for? (e.g., Sick Leave, Casual Leave)";
        private const string ReasonStepMsgText = "Please provide a reason for your leave.";

        public LeaveDialog()
            : base(nameof(LeaveDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new LeaveDateResolverDialog());
            var waterfallSteps = new WaterfallStep[]
            {
                LeaveTypeStepAsync,
                StartDateStepAsync,
                EndDateStepAsync,
                ReasonStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> LeaveTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var entities = (List<KekaBot.kiki.IntentRecognition.Entity>)stepContext.Options;
            var leaveDetails = new LeaveDetails();
            var leaveType = entities.Find(e => string.Equals(e.Category, BotEntities.LeaveType, StringComparison.InvariantCultureIgnoreCase))?.Text;

            if (string.IsNullOrEmpty(leaveType))
            {
                var promptMessage = MessageFactory.Text(LeaveTypeStepMsgText, LeaveTypeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            leaveDetails.LeaveType = leaveType;
            return await stepContext.NextAsync(leaveDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> StartDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var entities = (List<KekaBot.kiki.IntentRecognition.Entity>)stepContext.Options;
            var leaveDetails = (LeaveDetails)stepContext.Result;
            var startDate = entities.Find(e => string.Equals(e.Category, BotEntities.StartDate, StringComparison.InvariantCultureIgnoreCase))?.Text;

            if (string.IsNullOrEmpty(leaveDetails.StartDate) && string.IsNullOrEmpty(startDate))
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDateResolverDialog), leaveDetails.StartDate, cancellationToken);
            }

            leaveDetails.StartDate = leaveDetails.StartDate ?? startDate;
            return await stepContext.NextAsync(leaveDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> EndDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var entities = (List<KekaBot.kiki.IntentRecognition.Entity>)stepContext.Options;
            var leaveDetails = (LeaveDetails)stepContext.Result;
            var endDate = entities.Find(e => string.Equals(e.Category, BotEntities.EndDate, StringComparison.InvariantCultureIgnoreCase))?.Text;

            if (string.IsNullOrEmpty(leaveDetails.EndDate) && string.IsNullOrEmpty(endDate))
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDateResolverDialog), leaveDetails.EndDate, cancellationToken);
            }

            leaveDetails.EndDate = leaveDetails.EndDate ?? endDate;
            return await stepContext.NextAsync(leaveDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> ReasonStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var entities = (List<KekaBot.kiki.IntentRecognition.Entity>)stepContext.Options;
            var leaveDetails = (LeaveDetails)stepContext.Result;
            var reason = entities.Find(e => string.Equals(e.Category, BotEntities.LeaveReason, StringComparison.InvariantCultureIgnoreCase))?.Text;

            if (string.IsNullOrEmpty(leaveDetails.Reason) && string.IsNullOrEmpty(reason))
            {
                var promptMessage = MessageFactory.Text(ReasonStepMsgText, ReasonStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            leaveDetails.Reason = leaveDetails.Reason ?? reason;
            return await stepContext.NextAsync(leaveDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Result;

            var messageText = $"Please confirm your leave request:\n" +
                  $"Leave Type: {leaveDetails.LeaveType}\n" +
                  $"Start Date: {leaveDetails.StartDate}\n" +
                  $"End Date: {leaveDetails.EndDate}\n" +
                  (!string.IsNullOrEmpty(leaveDetails.Reason) ? $"Reason: {leaveDetails.Reason}\n" : "") +
                  "Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var leaveDetails = (LeaveDetails)stepContext.Result;

                DateTime startDate = DateTime.Parse(leaveDetails.StartDate);
                DateTime endDate = DateTime.Parse(leaveDetails.EndDate);
                
                leaveDetails.Days = (endDate - startDate).Days;

                return await stepContext.EndDialogAsync(leaveDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
