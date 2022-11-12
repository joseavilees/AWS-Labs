using System.Text.Json;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda1;

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        using var lambdaClient = new AmazonLambdaClient();
        var response = await lambdaClient.InvokeAsync(new InvokeRequest
        {
            FunctionName = "Lambda2",
            Payload = JsonSerializer.Serialize(input)
        });

        using var reader = new StreamReader(response.Payload);
        var textResponse = await reader.ReadToEndAsync();

        return textResponse;
    }
}