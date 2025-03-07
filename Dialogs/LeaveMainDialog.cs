// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

using Kiki.CognitiveModels;

namespace Kiki.Dialogs
{
    public class LeaveMainDialog : ComponentDialog
    {
        private readonly LeaveRecognizer _luisRecognizer;
        private readonly ILogger _logger;

        public LeaveMainDialog(LeaveRecognizer luisRecognizer, LeaveDialog leaveDialog, ILogger<LeaveMainDialog> logger)
            : base(nameof(LeaveMainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(leaveDialog);

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
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            var messageText = stepContext.Options?.ToString() ?? "How can I assist you with your leave today?\nSay something like 'Apply for leave from July 10 to July 15' or 'Check my leave balance'.";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                return await stepContext.BeginDialogAsync(nameof(LeaveDialog), new LeaveDetails(), cancellationToken);
            }

            var luisResult = await _luisRecognizer.RecognizeAsync<Leave>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case Leave.Intent.ApplyLeave:
                    var leaveDetails = new LeaveDetails()
                    {
                        EmployeeId = luisResult.EmployeeEntities.EmployeeId,
                        LeaveType = luisResult.LeaveDetails.LeaveType,
                        StartDate = luisResult.LeaveStartDate,
                        Days = luisResult.LeaveDetails.Days ?? 0,
                    };
                    return await stepContext.BeginDialogAsync(nameof(LeaveDialog), leaveDetails, cancellationToken);

                case Leave.Intent.CancelLeave:
                    var leaveRequestId = luisResult.Entities.LeaveRequestId?.FirstOrDefault();
                    var cancelMessage = string.IsNullOrEmpty(leaveRequestId)
                        ? "I couldn't find a valid leave request ID. Can you please provide one?"
                        : $"Your leave request {leaveRequestId} has been canceled.";

                    return await HandleCancelLeaveAsync(stepContext, luisResult, cancellationToken);
                    
                case Leave.Intent.GetLeaveBalance:
                    var employeeId = luisResult.Entities.EmployeeId?.FirstOrDefault();
                    var balanceMessage = string.IsNullOrEmpty(employeeId)
                        ? "I couldn't find a valid employee ID. Can you provide one?"
                        : $"Your current leave balance is X days.";
                    return await HandleGetLeaveBalanceAsync(stepContext, luisResult, cancellationToken);

                default:
                    var didntUnderstandMessageText = $"Sorry, I didn't understand. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is LeaveDetails result)
            {
                var messageText = $"Your leave request for {result.LeaveType} from {result.StartDate} for {result.Days} days has been recorded.";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            var promptMessage = "What else can I assist you with?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleCancelLeaveAsync(WaterfallStepContext stepContext, Leave luisResult, CancellationToken cancellationToken)
        {
            var leaveRequestId = luisResult.Entities.LeaveRequestId?.FirstOrDefault();

            if (string.IsNullOrEmpty(leaveRequestId))
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("I couldn't find a valid leave request ID. Can you please provide one?"), cancellationToken);
            }
            else
            {
                // Simulate calling the leave management system (Replace this with actual API/DB call)
                var cancelMessage = $"Your leave request {leaveRequestId} has been canceled successfully.";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(cancelMessage), cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleGetLeaveBalanceAsync(WaterfallStepContext stepContext, Leave luisResult, CancellationToken cancellationToken)
        {
            var employeeId = luisResult.Entities.EmployeeId?.FirstOrDefault();

            if (string.IsNullOrEmpty(employeeId))
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("I couldn't find a valid employee ID. Can you provide one?"), cancellationToken);
            }
            else
            {
                // Replace this with actual API/DB call
                var leaveHistory = new List<string>
                {
                    "Sick Leave - Jan 10 to Jan 12",
                    "Vacation Leave - Feb 5 to Feb 10",
                    "Work From Home - Mar 3"
                };

                var leaveMessage = $"Here are your applied leaves:\n- {string.Join("\n- ", leaveHistory)}";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(leaveMessage), cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

    }
}
