namespace SmsReminderLambda.Models;

public class ReminderRequest
{
    public string PhoneNumber { get; init; }
    public string Message { get; init; }
}