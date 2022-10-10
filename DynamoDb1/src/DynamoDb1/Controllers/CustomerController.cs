using DynamoDb1.Commands.CustomerCommands;
using DynamoDb1.Domain.Person;
using DynamoDb1.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDb1.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _customerRepository.GetAsync(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("/FindByFirstName/{firstName}")]
    public async Task<IActionResult> FindByFirstName(string firstName)
    {
        var result = await _customerRepository
            .FindByFirstName(firstName);

        return Ok(result);
    }

    [HttpGet("/FindByLastName/{lastName}")]
    public async Task<IActionResult> FindByLastName(string lastName)
    {
        var result = await _customerRepository
            .FindByLastName(lastName);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand createCommand)
    {
        var customer = new Customer(createCommand.FirstName, createCommand.LastName, createCommand.Age);
        await _customerRepository.CreateAsync(customer);

        return Ok(customer.Id);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _customerRepository.DeleteAsync(id);

        return NoContent();
    }
}