using KekaBot.kiki.IntentRecognition;
using KekaBot.kiki.Services;
using Kiki.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace KekaBot.kiki.Dialogs;

public class AttendancePolicyDialog : CancelAndHelpDialog
{
    private string PolicyTypeStepMsgText = "What type of attendance policy would you like me to explain?";
    private List<string> PolicyTypes = new List<string> { "Capture Scheme", "Penalisation Policy" };
    private KekaServiceClient KekaServiceClient;
    private IntentRecognizer Recognizer;

    public AttendancePolicyDialog(KekaServiceClient kekaServiceClient, IntentRecognizer recognizer)
        : base(nameof(AttendancePolicyDialog))
    {
        AddDialog(new TextPrompt(nameof(TextPrompt)));
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
        var policyType = (string)stepContext.Options;

        if (string.IsNullOrEmpty(policyType))
        {
            PolicyTypeStepMsgText += Environment.NewLine + "Available Policy Types are: " + Environment.NewLine;
            foreach (var type in this.PolicyTypes)
            {
                PolicyTypeStepMsgText += type + Environment.NewLine;
            }

            var promptMessage = MessageFactory.Text(PolicyTypeStepMsgText, PolicyTypeStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        return await stepContext.NextAsync(policyType, cancellationToken);
    }

    private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace((string)stepContext.Result))
        {
            var policyType = (string)stepContext.Result;
            var policyDoc = this.KekaServiceClient.GetAttendancePolicy(policyType);
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
            return await stepContext.EndDialogAsync(policyType, cancellationToken);
        }

        return await stepContext.EndDialogAsync(null, cancellationToken);
    }
}
