namespace KekaBot.kiki.PolicyData;

public static class AttendancePolicies
{
    public const string CaptureScheme = @"Below are the details of capture scheme assigned to you

Bio-Metric & Web Clock-In
Your attendance is automatically tracked using biometric device(s)

Your attendance is tracked using web clock-in, i.e you have to log in to Keka website and mark your attendance (Browser Only).

Remote Punches/Clock-In
Your attendance is tracked from clock-in/out done using Keka mobile app

Continuous punch (every 60 mins interval) to track your location has been enabled. This service will start when you clock-in using Keka mobile app.

Approval is required for all Remote Clock-in requests.

Work from Home (WFH) & On-duty (OD)
You can request for only full day WFH

You are required to clock-in/out when doing WFH. In case of late clock-in, no clock-in, or less effective/gross hours clocked, the system will penalise based on penalisation policy assigned to you.

Approval is required for all WFH requests.

You can request for only full day OD.

You are required to clock-in/out when doing OD. In case of late clock-in, no clock-in, or less effective/gross hours clocked, the system will penalise based on penalisation policy assigned to you.

Approval is required for all OD requests.

Regularization & Partial Day
In case of attendance discrepancy, you are allowed to adjust attendance logs .

In case of penalisation due to attendance discrepancy, you are allowed to request regularisation .

You are allowed to apply for below partial work day request(s), based on number of cumulative (total) hours in a period

Partial day is allowed for a cumulative hours of 50000 minutes in a Week.

Late Arrival: You are allowed to request for maximum 50000 minutes (each instance) of late arrival (after shift start time)
Partial day request cannot be made sooner than 1 day(s) .

You are allowed to request for past dated partial work day .

Approval is required for all Attendance Adjustment / Regularization / Partial Day requests.";

    public const string PenalisationPolicy = @"Below are the details of your Penalisation Policy

Penalisation policy is effective Jul 26, 2023

No Attendance
You will be penalized 1 day(s) of Unpaid Leave(Loss of Pay) for every single missing attendance day

You have a buffer period of 2 day(s) to regularize your attendance before the penalization happens.

Late Arrival
You won't be penalized for any late arrival incidents.

Work Hours
There is no penalization for number of work hours you spend in office.

Missing Swipes
There is no penalization for Missing Swipes, but it is recommended to regularize your attendance.";
}
