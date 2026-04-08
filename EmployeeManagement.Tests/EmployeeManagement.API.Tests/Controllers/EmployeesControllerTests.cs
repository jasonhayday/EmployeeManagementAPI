using EmployeeManagement.API.Controllers;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeManagement.API.Controllers.Tests;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeService> _serviceMock;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _serviceMock = new Mock<IEmployeeService>();
        _controller = new EmployeesController(_serviceMock.Object);
    }

    [Fact]
    public async Task Get_Should_Return_Ok_With_Employees()
    {
        // Arrange
        var employees = new List<EmployeeResponseDto>
        {
            new EmployeeResponseDto
            {
                Id = 1,
                Name = "John",
                Email = "john@test.com",
                Department = "IT",
                Salary = 5000
            },
            new EmployeeResponseDto
            {
                Id = 2,
                Name = "Jane",
                Email = "jane@test.com",
                Department = "HR",
                Salary = 6000
            }
        };

        _serviceMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(employees);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var value = Assert.IsAssignableFrom<IEnumerable<EmployeeResponseDto>>(okResult.Value);

        Assert.Equal(2, value.Count());

        _serviceMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_Should_Return_Ok_With_Created_Employee()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            Name = "Jason",
            Email = "jason@test.com",
            Department = "Engineering",
            Salary = 7000
        };

        var created = new EmployeeResponseDto
        {
            Id = 1,
            Name = dto.Name,
            Email = dto.Email,
            Department = dto.Department,
            Salary = dto.Salary
        };

        _serviceMock
            .Setup(x => x.CreateAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var value = Assert.IsType<EmployeeResponseDto>(okResult.Value);

        Assert.Equal("Jason", value.Name);

        _serviceMock.Verify(x => x.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Update_Should_Return_Ok()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            Name = "Updated",
            Email = "updated@test.com",
            Department = "IT",
            Salary = 8000
        };

        _serviceMock
            .Setup(x => x.UpdateAsync(1, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        Assert.IsType<OkResult>(result);

        _serviceMock.Verify(x => x.UpdateAsync(1, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok()
    {
        // Arrange
        _serviceMock
            .Setup(x => x.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<OkResult>(result);

        _serviceMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}