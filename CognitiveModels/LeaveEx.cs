using System.Linq;

namespace Kiki.CognitiveModels
{
    // Extends the partial LeaveApplication class with methods and properties that simplify accessing entities in the LUIS results
    public partial class Leave
    {
        public (string EmployeeName, string EmployeeId) EmployeeEntities
        {
            get
            {
                var employeeName = Entities?._instance?.EmployeeName?.FirstOrDefault()?.Text;
                var employeeId = Entities?.EmployeeId?.FirstOrDefault();
                return (employeeName, employeeId);
            }
        }

        public (string LeaveType, int? Days) LeaveDetails
        {
            get
            {
                var leaveType = Entities?.LeaveType?.FirstOrDefault();
                var days = Entities?.Days?.FirstOrDefault();
                return (leaveType, days != null ? int.Parse(days) : (int?)null);
            }
        }

        public string LeaveStartDate
            => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];

        public string LeaveEndDate
            => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];

        public string LeaveRequestId
            => Entities?.LeaveRequestId?.FirstOrDefault();

        public string EmployeeId
            => Entities?.EmployeeId?.FirstOrDefault();
    }
}