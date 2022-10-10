using System.Net;
using System.Net.Http.Json;
using DynamoDb1.Commands.CustomerCommands;
using DynamoDb1.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb1.IntegrationTests.Controllers.CustomerController;

public class CreateCustomerControllerTests : IClassFixture<ApiFactory>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly HttpClient _httpClient;

    public CreateCustomerControllerTests()
    {
        var apiFactory = new ApiFactory();

        _httpClient = apiFactory.CreateClient();

        _customerRepository = apiFactory.Services.GetService<ICustomerRepository>()!;
    }

    [Fact]
    public async Task CreateCustomerTest()
    {
        var createCustomerCommand = new CreateCustomerCommand
        {
            FirstName = "Antonio",
            LastName = "Garcia",
            Age = 30
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Customer",
            createCustomerCommand);

        var customerCreatedId = await response.Content.ReadFromJsonAsync<Guid>();
        var customerCreated = await _customerRepository
            .GetAsync(customerCreatedId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        customerCreated.Should().NotBeNull();
        customerCreated!.FirstName.Should().Be("Antonio");
        customerCreated.LastName.Should().Be("Garcia");
        customerCreated.Age.Should().Be(30);
        customerCreated.IsAdult.Should().BeTrue();
    }
}