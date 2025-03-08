using KekaBot.kiki.Services;
using Kiki.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using KekaBot.kiki.IntentRecognition;
using System.Linq;

namespace KekaBot.kiki.Dialogs;

public class LeavePolicyDialog : CancelAndHelpDialog
{
    private string LeaveTypeStepMsgText = "What type of leave policy would you like me to explain?";
    private KekaServiceClient KekaServiceClient;
    private IntentRecognizer Recognizer;

    public LeavePolicyDialog(KekaServiceClient kekaServiceClient, IntentRecognizer recognizer)
        : base(nameof(LeavePolicyDialog))
    {
        AddDialog(new TextPrompt(nameof(TextPrompt)));
        AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
        AddDialog(new LeaveDateResolverDialog());
        var waterfallSteps = new WaterfallStep[]
        {
                LeaveTypeStepAsync,
                FinalStepAsync,
        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

        InitialDialogId = nameof(WaterfallDialog);

        this.KekaServiceClient = kekaServiceClient;
        this.Recognizer = recognizer;
    }

    private async Task<DialogTurnResult> LeaveTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var leaveDetails = (LeaveDetails)stepContext.Options;

        if (string.IsNullOrEmpty(leaveDetails.LeaveType))
        {
            var response = await this.KekaServiceClient.GetEmployeeLeaves();
            var employeeLeaves = response.Data;

            LeaveTypeStepMsgText += Environment.NewLine + "Available Leave Types are: " + Environment.NewLine;
            foreach (var leavePlanConfig in employeeLeaves.LeavePlan.Configuration)
            {
                employeeLeaves.LeaveSummaries.ForEach(_ =>
                {
                    if (_.TypeId == leavePlanConfig.LeaveType.Id)
                    {
                        LeaveTypeStepMsgText += leavePlanConfig.LeaveType.Name + Environment.NewLine;
                    }
                });
            }

            var promptMessage = MessageFactory.Text(LeaveTypeStepMsgText, LeaveTypeStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        return await stepContext.NextAsync(leaveDetails.LeaveType, cancellationToken);
    }

    private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace((string)stepContext.Result))
        {
            var leaveType = (string)stepContext.Result;
            var policyDoc = this.KekaServiceClient.GetLeavePolicy(leaveType);
            var summaries = this.Recognizer.Client.AbstractiveSummarize(Azure.WaitUntil.Completed, new List<string> { policyDoc }, cancellationToken: cancellationToken, options: new Azure.AI.TextAnalytics.AbstractiveSummarizeOptions { SentenceCount = 4 });
            var summary = string.Empty;
            await foreach (var result in summaries.Value.AsPages().ConfigureAwait(false).WithCancellation(cancellationToken))
            {
                foreach (var item in result.Values)
                {
                    summary += item[0].Summaries.FirstOrDefault().Text;
                }
            }
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(summary), cancellationToken);
            return await stepContext.EndDialogAsync(leaveType, cancellationToken);
        }

        return await stepContext.EndDialogAsync(null, cancellationToken);
    }
}
