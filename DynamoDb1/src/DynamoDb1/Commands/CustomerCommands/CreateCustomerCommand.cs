namespace DynamoDb1.Commands.CustomerCommands;

public class CreateCustomerCommand
{
    public string FirstName { get; init; } = default!;

    public string LastName { get; init; } = default!;

    public int Age { get; init; }
}