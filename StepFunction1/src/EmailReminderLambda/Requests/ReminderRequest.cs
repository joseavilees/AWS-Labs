namespace EmailReminderLambda.Requests;

public class ReminderRequest
{
    public string Email { get; init; }
    public string Message { get; init; }
}