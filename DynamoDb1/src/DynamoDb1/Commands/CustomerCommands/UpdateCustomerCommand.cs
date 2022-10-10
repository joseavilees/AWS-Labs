namespace DynamoDb1.Commands.CustomerCommands;

public class UpdateCustomerCommand
{
    public Guid Id { get; set; }

    public string FirstName { get; init; } = default!;

    public string LastName { get; init; } = default!;
}