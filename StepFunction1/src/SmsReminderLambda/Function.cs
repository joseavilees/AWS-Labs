using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SmsReminderLambda.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SmsReminderLambda;

public class Function
{
    private const string SenderId = "Foo-ID";

    public async Task<string> SendSms(ReminderRequest request, ILambdaContext context)
    {
        var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.USEast1);
        var publishRequest = new PublishRequest
        {
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "AWS.SNS.SMS.SenderID", new MessageAttributeValue
                    {
                        StringValue = SenderId,
                        DataType = "String"
                    }
                }
            },
            Message = request.Message,
            PhoneNumber = request.PhoneNumber
        };

        await client.PublishAsync(publishRequest);

        return "Reminder sent by SMS";
    }
}