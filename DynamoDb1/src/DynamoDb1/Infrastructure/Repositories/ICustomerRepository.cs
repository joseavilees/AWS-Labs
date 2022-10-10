using DynamoDb1.Domain.Person;

namespace DynamoDb1.Infrastructure.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> FindByFirstName(string firstName);
    Task<IEnumerable<Customer>> FindByLastName(string lastName);
    Task<Customer?> GetAsync(Guid id);
    Task<bool> CreateAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Guid id);
}