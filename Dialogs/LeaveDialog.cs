using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KekaBot.kiki.Dialogs;
using KekaBot.kiki.Services;
using KekaBot.kiki.Services.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Kiki.Dialogs
{
    public class LeaveDialog : CancelAndHelpDialog
    {
        private string LeaveTypeStepMsgText = "What type of leave would you like to apply for?";
        private const string ReasonStepMsgText = "Please provide a reason for your leave.";
        private KekaServiceClient KekaServiceClient;

        public LeaveDialog(KekaServiceClient kekaServiceClient)
            : base(nameof(LeaveDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new LeaveDateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
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

            this.KekaServiceClient = kekaServiceClient;
        }

        private async Task<DialogTurnResult> LeaveTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            var response = await this.KekaServiceClient.GetEmployeeLeaves();
            var employeeLeaves = response.Data;
            var choices = new List<Choice>();

            if (string.IsNullOrEmpty(leaveDetails.LeaveType) || !employeeLeaves.LeavePlan.Configuration.Any(_ => string.Equals(_.LeaveType.Name, leaveDetails.LeaveType, StringComparison.InvariantCultureIgnoreCase)))
            {
                LeaveTypeStepMsgText += Environment.NewLine + "Available Leave Types are: " + Environment.NewLine;
                foreach (var leavePlanConfig in employeeLeaves.LeavePlan.Configuration)
                {
                    employeeLeaves.LeaveSummaries.ForEach(_ =>
                    {
                        if (_.TypeId == leavePlanConfig.LeaveType.Id)
                        {
                            LeaveTypeStepMsgText += leavePlanConfig.LeaveType.Name + " - Balance: " + _.AvailableBalance.DurationString + Environment.NewLine;
                            choices.Add(new Choice(leavePlanConfig.LeaveType.Name));
                        }
                    });
                }

                var promptMessage = MessageFactory.Text(LeaveTypeStepMsgText, LeaveTypeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions { Prompt = promptMessage, Choices = choices }, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.LeaveType, cancellationToken);
        }

        private async Task<DialogTurnResult> StartDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.LeaveType = ((FoundChoice)stepContext.Result).Value;

            if (string.IsNullOrEmpty(leaveDetails.StartDate) || !DateOnly.TryParse(leaveDetails.StartDate, out DateOnly parsedDate))
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDateResolverDialog), leaveDetails.StartDate, cancellationToken);
            }

            return await stepContext.NextAsync(leaveDetails.StartDate, cancellationToken);
        }

        private async Task<DialogTurnResult> EndDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var leaveDetails = (LeaveDetails)stepContext.Options;
            leaveDetails.StartDate = (string)stepContext.Result;

            if (string.IsNullOrEmpty(leaveDetails.EndDate) || !DateOnly.TryParse(leaveDetails.EndDate, out DateOnly parsedDate))
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

            var messageText = $"Please confirm your leave request:{Environment.NewLine}" +
                              $"Leave Type: {leaveDetails.LeaveType}{Environment.NewLine}" +
                              $"Start Date: {leaveDetails.StartDate}{Environment.NewLine}" +
                              $"End Date: {leaveDetails.EndDate}{Environment.NewLine}" +
                              (!string.IsNullOrEmpty(leaveDetails.Reason) ? $"Reason: {leaveDetails.Reason}{Environment.NewLine}" : "") +
                              $"Is this correct?";

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
                
                leaveDetails.Days = (endDate - startDate).Days + 1;

                var response = await this.KekaServiceClient.GetEmployeeLeaves();
                var employeeLeaves = response.Data;
                int leaveTypeId = 0;

                foreach (var leavePlanConfig in employeeLeaves.LeavePlan.Configuration)
                {
                    if (leavePlanConfig.LeaveType.Name == leaveDetails.LeaveType)
                    {
                        leaveTypeId = leavePlanConfig.LeaveType.Id;
                        break;
                    }
                }

                LeaveRequest leaveRequest = new LeaveRequest
                {
                    FromDate = startDate,
                    ToDate = endDate,
                    Note = leaveDetails.Reason,
                    AvailFloaterLeave = leaveDetails.LeaveType.Equals("Floater Leave"),
                    Selection = new List<LeaveSelection>()
                    {
                        new LeaveSelection
                        {
                            LeaveTypeId = leaveTypeId,
                            Count = leaveDetails.Days
                        }
                    }
                };

                await this.KekaServiceClient.RequestLeave(leaveRequest);
                return await stepContext.EndDialogAsync(leaveDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
