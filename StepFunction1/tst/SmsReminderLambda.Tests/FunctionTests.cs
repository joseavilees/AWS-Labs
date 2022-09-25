using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.TestUtilities;
using SmsReminderLambda.Models;

namespace SmsReminderLambda.Tests;

public class FunctionTests
{
    [Fact]
    public async Task TestSendSms()
    {
        // Arrange
        var functions = new Function();
        var request = new ReminderRequest
        {
            // A verified phone number
            PhoneNumber = "+34123456789",
            Message = "Hi, I'm a reminder :)"
        };
        var context = new TestLambdaContext();

        // Act
        var response = await functions.SendSms(request, context);

        // Assert
        Assert.Equal("Reminder sent by SMS", response);
    }
}