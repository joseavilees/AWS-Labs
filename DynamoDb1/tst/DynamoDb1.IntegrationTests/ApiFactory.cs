using Amazon.DynamoDBv2;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DynamoDb1.IntegrationTests.TestSupport.Tables;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DynamoDb1.IntegrationTests;

class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainersContainer _dbContainer;

    public ApiFactory()
    {
        _dbContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("amazon/dynamodb-local")
            .WithPortBinding(8000)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8000))
            .Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await CreateDb();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.DisposeAsync().AsTask();
    }

    private async Task CreateDb()
    {
        var dynamoDbClient = Services.GetService<IAmazonDynamoDB>();

        await CustomerTableBuilder.CreateAsync(dynamoDbClient!);
    }
}