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
            var leaveDetails = (LeaveDetails)stepContext.Options;

            if (string.IsNullOrEmpty(leaveDetails.LeaveType))
            {
                var promptMessage = MessageFactory.Text(LeaveTypeStepMsgText, LeaveTypeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.LeaveType, cancellationToken);
        }

        private async Task<DialogTurnResult> StartDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.LeaveType = (string)stepContext.Result;

            if (string.IsNullOrEmpty(leaveDetails.StartDate))
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDateResolverDialog), leaveDetails.StartDate, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.StartDate, cancellationToken);
        }

        private async Task<DialogTurnResult> EndDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.StartDate = (string)stepContext.Result;

            if (string.IsNullOrEmpty(leaveDetails.EndDate))
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDateResolverDialog), leaveDetails.EndDate, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.EndDate, cancellationToken);
        }

        private async Task<DialogTurnResult> ReasonStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.EndDate = (string)stepContext.Result;

            if (string.IsNullOrEmpty(leaveDetails.Reason))
            {
                var promptMessage = MessageFactory.Text(ReasonStepMsgText, ReasonStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.Reason, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.Reason = (string)stepContext.Result;

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
                var leaveDetails = (LeaveDetails)stepContext.Options;

                DateTime startDate = DateTime.Parse(leaveDetails.StartDate);
                DateTime endDate = DateTime.Parse(leaveDetails.EndDate);
                
                leaveDetails.Days = (endDate - startDate).Days;

                return await stepContext.EndDialogAsync(leaveDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
