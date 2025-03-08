using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using KekaBot.kiki.Services;
using KekaBot.kiki.Dialogs;

namespace Kiki.Dialogs
{
    public class TaskDialog : CancelAndHelpDialog
    {
        private const string TaskNameStepMsgText = "Would you like to add or update a task, start by giving the task name here..";
        private const string InvalidTaskNameMsgText = "Invalid task name. Please enter a valid task name:";
        private const string AddTaskDetailsStepMsgText = "Please provide the details of your task.";
        private const string UpdateTaskDetailsStepMsgText = "Would you like to update the details of your task?";
        private const string AddTaskStatusStepMsgText = "What is the status of this task?";
        private const string UpdateTaskStatusStepMsgText = "Please provide the updated task status";
        private readonly KekaServiceClient _kekaServiceClient;
        private TaskService TaskService = new TaskService();

        public TaskDialog(KekaServiceClient kekaServiceClient)
            : base(nameof(TaskDialog))
        {
            _kekaServiceClient = kekaServiceClient;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            var waterfallSteps = new WaterfallStep[]
            {
                TaskNameStepAsync,
                TaskFetchStepAsync,
                TaskDescriptionStepAsync,
                ProcessTaskDescriptionStepAsync,
                TaskStatusStepAsync,
                ProcessTaskStatusStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> TaskNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options ?? new TaskDetails();

            string taskName = (string)stepContext.Result;

            if (string.IsNullOrEmpty(taskName))
            {
                var promptMessage = MessageFactory.Text(TaskNameStepMsgText, TaskNameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            // Validate the task name
            bool isValid = TaskService.ValidateTaskName(taskName);

            if (!isValid)
            {
                var retryMessage = MessageFactory.Text(InvalidTaskNameMsgText, InvalidTaskNameMsgText, InputHints.ExpectingInput);
                return await stepContext.ReplaceDialogAsync(nameof(TaskNameStepAsync), taskDetails, cancellationToken);
            }

            taskDetails.TaskName = taskName;
            return await stepContext.NextAsync(taskDetails.TaskName, cancellationToken);
        }

        private async Task<DialogTurnResult> TaskFetchStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;
            taskDetails.TaskName = (string)stepContext.Result;

            if (taskDetails.ActionType == "UpdateTask" || taskDetails.ActionType == "GetTask")
            {
                if (string.IsNullOrEmpty(taskDetails.TaskName))
                {
                    taskDetails = TaskService.GetTask(taskDetails.TaskId);

                    var messageText = $"These are the details of the task:{Environment.NewLine}" +
                                      $"Name: {taskDetails.TaskName}{Environment.NewLine}" +
                                      $"Description: {taskDetails.Description}{Environment.NewLine}" +
                                      $"Status: {taskDetails.Status.ToString()}{Environment.NewLine}" +
                                      (!string.IsNullOrEmpty(taskDetails.DueDate) ? $"Due Date: {taskDetails.DueDate}{Environment.NewLine}" : "") +
                                      "Is this correct?";

                    var taskMessage = MessageFactory.Text(messageText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = taskMessage }, cancellationToken);
                }
            }

            return await stepContext.NextAsync(taskDetails.TaskId, cancellationToken);
        }

        private async Task<DialogTurnResult> TaskDescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;
            taskDetails.TaskId = (string)stepContext.Result;

            if (string.IsNullOrEmpty(taskDetails.Description))
            {
                var promptMessage = MessageFactory.Text(AddTaskDetailsStepMsgText, AddTaskDetailsStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            var confirmMessage = MessageFactory.Text(UpdateTaskDetailsStepMsgText, UpdateTaskDetailsStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = confirmMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessTaskDescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;

            if (string.IsNullOrEmpty(taskDetails.Description))
            {
                taskDetails.Description = (string)stepContext.Result;
                return await stepContext.NextAsync(taskDetails.Description, cancellationToken);
            }

            bool confirmResult = (bool)stepContext.Result;
            if (confirmResult)
            {
                var promptMessage = MessageFactory.Text("Please provide the updated task description.", "Please provide the updated task description.", InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(taskDetails.Description, cancellationToken);
        }

        private async Task<DialogTurnResult> TaskStatusStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;
            taskDetails.Description = (string)stepContext.Result;

            if (taskDetails.Status == null)
            {
                var promptMessage = MessageFactory.Text(AddTaskStatusStepMsgText, AddTaskStatusStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            var confirmMessage = MessageFactory.Text("Would you like to update the task status?", "Would you like to update the task status?", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = confirmMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessTaskStatusStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;

            if (taskDetails.Status == null)
            {
                Enum.TryParse((string)stepContext.Result, true, out Status status);
                taskDetails.Status = status;
                return await stepContext.NextAsync(taskDetails.Status, cancellationToken);
            }

            bool confirmResult = (bool)stepContext.Result;
            if (confirmResult)
            {
                var promptMessage = MessageFactory.Text(UpdateTaskStatusStepMsgText, UpdateTaskStatusStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(taskDetails.Status, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var taskDetails = (TaskDetails)stepContext.Options;
            taskDetails.Status = (Status)stepContext.Result;

            var messageText = $"Please confirm your task details:{Environment.NewLine}" +
                              $"Task Name: {taskDetails.TaskName}{Environment.NewLine}" +
                              (!string.IsNullOrEmpty(taskDetails.Description) ? $"Description: {taskDetails.Description}{Environment.NewLine}" : "") +
                              $"Status: {(taskDetails.Status != null ? taskDetails.Status.ToString() : "None")}{Environment.NewLine}" +
                              "Is this correct?";

            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var taskDetails = (TaskDetails)stepContext.Options;

                switch (taskDetails.ActionType)
                {
                    case "AddTask":
                        TaskService.AddTask(taskDetails);
                        break;
                    case "UpdateTask":
                        TaskService.UpdateTask(taskDetails);
                        break;
                    case "GetTask":
                        var task = TaskService.GetTask(taskDetails.TaskName);
                        await stepContext.Context.SendActivityAsync($"Task Details: {task.Description}", cancellationToken: cancellationToken);
                        break;
                }

                return await stepContext.EndDialogAsync(taskDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}

