using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.TestUtilities;
using EmailReminderLambda.Requests;

namespace EmailReminderLambda.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestSendEmail()
    {
        // Arrange
        var functions = new Function();
        var request = new ReminderRequest
        {
            // A verified email
            Email = "sender2@gmail.com",
            Message = "Hi, I'm a reminder :)"
        };
        var context = new TestLambdaContext();

        // Act
        var response = await functions.SendEmail(request, context);

        // Assert
        Assert.Equal("Reminder sent by Email", response);
    }
}