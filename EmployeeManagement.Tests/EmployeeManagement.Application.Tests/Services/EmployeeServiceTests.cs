using Moq;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Services.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repositoryMock;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _repositoryMock = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedEmployees()
    {
        var employees = new List<Employee>
        {
            new Employee("Jason","jason@email.com","IT",5000),
            new Employee("John","john@email.com","HR",4000)
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(employees);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenFound()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(employee);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Jason", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Employee?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee_AndReturnDto()
    {
        var dto = new EmployeeCreateDto
        {
            Name = "Jason",
            Email = "jason@email.com",
            Department = "IT",
            Salary = 5000
        };

        var result = await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);

        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee_WhenExists()
    {
        var employee = new Employee("Old", "old@email.com", "OldDept", 1000);

        var dto = new EmployeeCreateDto
        {
            Name = "New",
            Email = "new@email.com",
            Department = "IT",
            Salary = 5000
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(employee);

        await _service.UpdateAsync(1, dto);

        _repositoryMock.Verify(r => r.UpdateAsync(employee), Times.Once);

        Assert.Equal("New", employee.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEmployeeNotFound()
    {
        var dto = new EmployeeCreateDto
        {
            Name = "Test",
            Email = "test@email.com",
            Department = "IT",
            Salary = 5000
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository()
    {
        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}