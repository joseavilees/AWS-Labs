using System.Text.Json.Serialization;

namespace DynamoDb1.Domain.Person;

public class Customer
{
    public Guid Id { get; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public int Age { get; }

    public bool IsAdult { get; }

    [JsonConstructor]
    public Customer(Guid id, string firstName, string lastName, int age, bool isAdult)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        IsAdult = isAdult;
    }

    public Customer(string firstName, string lastName, int age)
    {
        Id = Guid.NewGuid();

        FirstName = firstName;
        LastName = lastName;
        Age = age;

        IsAdult = age >= 18;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}