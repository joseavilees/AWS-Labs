using Amazon;
using Amazon.DynamoDBv2;
using DynamoDb1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Test workaround https://github.com/dotnet/aspnetcore/issues/38435
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IAmazonDynamoDB>(_ =>
{
    var isLocal = builder.Configuration.GetValue<bool>("Db:IsLocal");
    var configuration = new AmazonDynamoDBConfig
    {
        LogResponse = true,
        DisableLogging = false,
        LogMetrics = true
    };

    if (isLocal)
    {
        configuration.ServiceURL = builder.Configuration.GetValue<string>("Db:ServiceURL");
    }
    else
    {
        configuration.RegionEndpoint = RegionEndpoint.USEast1;
    }

    return new AmazonDynamoDBClient(configuration);
});

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();