// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Kiki.Dialogs
{
    public class LeaveDateResolverDialog : CancelAndHelpDialog
    {
        private const string StartDatePromptMsgText = "When would you like your leave to start?";
        private const string EndDatePromptMsgText = "When would you like your leave to end?";
        private const string RepromptMsgText = "I'm sorry, please enter a full leave date including Day, Month, and Year (07-03-2025).";

        public LeaveDateResolverDialog(string id = null)
            : base(id ?? nameof(LeaveDateResolverDialog))
        {
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt), DateTimePromptValidator));

            var waterfallSteps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static Task<bool> DateTimePromptValidator(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            if (promptContext.Recognized.Succeeded)
            {
                var timex = promptContext.Recognized.Value[0].Timex.Split('T')[0];
                var isDefinite = new TimexProperty(timex).Types.Contains(Constants.TimexTypes.Definite);
                return Task.FromResult(isDefinite);
            }
            return Task.FromResult(false);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = stepContext.Parent.Context.Activity.Text;

            var promptMessageText = !DateOnly.TryParse(timex, out DateOnly parsedDate) ? StartDatePromptMsgText : EndDatePromptMsgText;
            
            if (parsedDate == DateOnly.MinValue)
            {
                promptMessageText = !DateOnly.TryParse(timex, out DateOnly date) ? StartDatePromptMsgText : EndDatePromptMsgText;
            }

            // var promptMessageText = StartDatePromptMsgText;

            var promptMessage = MessageFactory.Text(promptMessageText, promptMessageText, InputHints.ExpectingInput);
            var repromptMessage = MessageFactory.Text(RepromptMsgText, RepromptMsgText, InputHints.ExpectingInput);

            if (!string.IsNullOrEmpty(timex))
            {
                return await stepContext.PromptAsync(
                    nameof(DateTimePrompt),
                    new PromptOptions
                    {
                        Prompt = promptMessage,
                        RetryPrompt = repromptMessage,
                    }, cancellationToken);
            }

            var timexProperty = new TimexProperty(timex);
            if (!timexProperty.Types.Contains(Constants.TimexTypes.Definite))
            {
                return await stepContext.PromptAsync(
                    nameof(DateTimePrompt),
                    new PromptOptions { Prompt = repromptMessage },
                    cancellationToken);
            }

            return await stepContext.NextAsync(new List<DateTimeResolution> { new DateTimeResolution { Timex = timex } }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timex = ((List<DateTimeResolution>)stepContext.Result)[0].Timex;
            return await stepContext.EndDialogAsync(timex, cancellationToken);
        }
    }
}
