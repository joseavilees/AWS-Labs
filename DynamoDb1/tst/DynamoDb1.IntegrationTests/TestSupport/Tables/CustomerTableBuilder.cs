using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb1.IntegrationTests.TestSupport.Tables;

public class CustomerTableBuilder
{
    private const string TableName = "Customers";

    public static async Task CreateAsync(IAmazonDynamoDB client)
    {
        var attributeDefinitions = new List<AttributeDefinition>
        {
            new()
            {
                AttributeName = "Id",
                AttributeType = ScalarAttributeType.S
            }
        };

        var tableKeySchema = new List<KeySchemaElement>
        {
            new()
            {
                AttributeName = "Id",
                KeyType = "HASH"
            }
        };

        var createTableRequest = new CreateTableRequest
        {
            TableName = TableName,
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 10,
                WriteCapacityUnits = 10
            },
            AttributeDefinitions = attributeDefinitions,
            KeySchema = tableKeySchema
        };

        await client.CreateTableAsync(createTableRequest);
    }
}