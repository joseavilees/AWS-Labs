using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoDb1.Domain.Person;

namespace DynamoDb1.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ILogger<CustomerRepository> _logger;
    private readonly IAmazonDynamoDB _dynamoDb;

    private const string TableName = "Customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDb, ILogger<CustomerRepository> logger)
    {
        _dynamoDb = dynamoDb;
        _logger = logger;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = TableName,
            Key = GetPrimaryKey(id),
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.GetItemAsync(request);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(GetAsync), response.ConsumedCapacity.CapacityUnits);

        if (response.Item.Count == 0) return null;

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<Customer>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<Customer>> FindByFirstName(string firstName)
    {
        // PartiQL example
        var query = $"SELECT * FROM {TableName} WHERE FirstName = '{firstName}'";

        var request = new ExecuteStatementRequest
        {
            Statement = query,
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.ExecuteStatementAsync(request);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(FindByFirstName), response.ConsumedCapacity.CapacityUnits);

        var documents = response
            .Items
            .Select(Document.FromAttributeMap)
            .ToList();

        var output = JsonSerializer.Deserialize<IEnumerable<Customer>>(documents.ToJson());

        return output ?? Enumerable.Empty<Customer>();
    }

    public async Task<IEnumerable<Customer>> FindByLastName(string lastName)
    {
        var request = new ScanRequest
        {
            TableName = TableName,
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":lastName", new AttributeValue {S = lastName}}
            },
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                {"#lastName", "LastName"}
            },
            FilterExpression = "#lastName = :lastName",
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.ScanAsync(request);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(FindByLastName), response.ConsumedCapacity.CapacityUnits);

        var documents = response
            .Items
            .Select(Document.FromAttributeMap)
            .ToList();

        var output = JsonSerializer.Deserialize<IEnumerable<Customer>>(documents.ToJson());

        return output ?? Enumerable.Empty<Customer>();
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var customerAsJson = JsonSerializer.Serialize(customer);
        var item = Document
            .FromJson(customerAsJson)
            .ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = item,
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(CreateAsync), response.ConsumedCapacity.CapacityUnits);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerAsJson = JsonSerializer.Serialize(customer);
        var item = Document
            .FromJson(customerAsJson)
            .ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = item,
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(UpdateAsync), response.ConsumedCapacity.CapacityUnits);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = TableName,
            Key = GetPrimaryKey(id),
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);

        _logger.LogInformation("Executed {Method} Consumed units: {Units}",
            nameof(DeleteAsync), response.ConsumedCapacity.CapacityUnits);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    private static Dictionary<string, AttributeValue> GetPrimaryKey(Guid id)
    {
        return new Dictionary<string, AttributeValue>
        {
            {"Id", new AttributeValue {S = id.ToString()}}
        };
    }
}