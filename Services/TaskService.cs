using KekaBot.kiki.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace KekaBot.kiki.Services
{
    public class TaskService
    {
        static List<TaskDetails> TaskDetails = new List<TaskDetails>();

        public TaskService()
        {
        }

        public bool ValidateTaskName(string taskName)
        {
            return TaskDetails.Select(_ => _.TaskName).Contains(taskName);
        }

        public void AddTask(TaskDetails taskDetails)
        {
            TaskDetails.Add(taskDetails);
        }

        public TaskDetails GetTask(string taskName)
        {
            return TaskDetails.Where(_ => _.TaskName == taskName).First();
        }

        public TaskDetails UpdateTask(TaskDetails taskDetails)
        {
            var task = TaskDetails.Where(_ => _.TaskName == taskDetails.TaskName).First();
            task.Status = taskDetails.Status;
            task.Description = taskDetails.Description;
            task.Priority = taskDetails.Priority;
            return task;
        }
    }
}
