using Xunit;
using Moq;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;

public class EmployeeServiceTests
{
    [Fact]
    public async Task GetAll_ShouldReturnEmployees()
    {
        var repo = new Mock<IEmployeeRepository>();

        repo.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Employee>
            {
                new Employee { Id = 1, Name = "Jason" }
            });

        var service = new EmployeeService(repo.Object);

        var result = await service.GetAllAsync();

        Assert.Single(result);
    }
}