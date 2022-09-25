using Amazon;
using Amazon.Lambda.Core;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using EmailReminderLambda.Requests;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EmailReminderLambda;

public class Function
{
    // TODO: AWS verified email
    private const string SenderAddress = "sender@gmail.com";

    public async Task<string> SendEmail(ReminderRequest request, ILambdaContext context)
    {
        using var sesClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
        var sesRequest = new SendEmailRequest
        {
            Source = SenderAddress,
            Destination = new Destination
            {
                ToAddresses = new List<string> {request.Email}
            },
            Message = new Message
            {
                Subject = new Content("Reminder"),
                Body = new Body
                {
                    Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = request.Message
                    }
                }
            }
        };

        await sesClient.SendEmailAsync(sesRequest);

        return "Reminder sent by Email";
    }
}