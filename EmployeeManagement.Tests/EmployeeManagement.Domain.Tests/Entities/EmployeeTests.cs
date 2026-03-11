using EmployeeManagement.Domain.Entities;
using Xunit;

namespace EmployeeManagement.Domain.Entities.Tests;

public class EmployeeTests
{
    [Fact]
    public void Constructor_ShouldCreateEmployee_WhenDataIsValid()
    {
        var employee = new Employee(
            "Jason",
            "jason@email.com",
            "IT",
            5000
        );

        Assert.Equal("Jason", employee.Name);
        Assert.Equal("jason@email.com", employee.Email);
        Assert.Equal("IT", employee.Department);
        Assert.Equal(5000, employee.Salary);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetName_InvalidValue_ShouldThrowArgumentException(string? name)
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        Assert.Throws<ArgumentException>(() => employee.SetName(name!));
    }

    [Fact]
    public void SetName_ValidValue_ShouldUpdateName()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        employee.SetName("John");

        Assert.Equal("John", employee.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetDepartment_InvalidValue_ShouldThrowArgumentException(string? department)
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        Assert.Throws<ArgumentException>(() => employee.SetDepartment(department!));
    }

    [Fact]
    public void SetDepartment_ValidValue_ShouldUpdateDepartment()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        employee.SetDepartment("HR");

        Assert.Equal("HR", employee.Department);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetEmail_EmptyValue_ShouldThrowArgumentException(string? email)
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        Assert.Throws<ArgumentException>(() => employee.SetEmail(email!));
    }

    [Fact]
    public void SetEmail_InvalidFormat_ShouldThrowArgumentException()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        Assert.Throws<ArgumentException>(() => employee.SetEmail("invalid-email"));
    }

    [Fact]
    public void SetEmail_ValidValue_ShouldUpdateEmail()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        employee.SetEmail("john@email.com");

        Assert.Equal("john@email.com", employee.Email);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void UpdateSalary_InvalidValue_ShouldThrowArgumentException(decimal salary)
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        Assert.Throws<ArgumentException>(() => employee.UpdateSalary(salary));
    }

    [Fact]
    public void UpdateSalary_ValidValue_ShouldUpdateSalary()
    {
        var employee = new Employee("Jason", "jason@email.com", "IT", 5000);

        employee.UpdateSalary(8000);

        Assert.Equal(8000, employee.Salary);
    }
}