using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories.Tests;

public class EmployeeRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Employees()
    {
        using var context = CreateContext();

        var emp1 = new Employee("John", "john@test.com", "IT", 5000);
        var emp2 = new Employee("Jane", "jane@test.com", "HR", 6000);

        context.Employees.AddRange(emp1, emp2);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Employee_When_Found()
    {
        using var context = CreateContext();

        var emp = new Employee("John", "john@test.com", "IT", 5000);

        context.Employees.Add(emp);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);

        var result = await repository.GetByIdAsync(emp.Id);

        Assert.NotNull(result);
        Assert.Equal("John", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
    {
        using var context = CreateContext();
        var repository = new EmployeeRepository(context);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_Should_Add_Employee()
    {
        using var context = CreateContext();
        var repository = new EmployeeRepository(context);

        var employee = new Employee(
            "Jason",
            "jason@test.com",
            "Engineering",
            7000
        );

        await repository.AddAsync(employee);

        var saved = await context.Employees.FirstOrDefaultAsync();

        Assert.NotNull(saved);
        Assert.Equal("Jason", saved!.Name);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Employee()
    {
        using var context = CreateContext();

        var employee = new Employee(
            "Old Name",
            "old@test.com",
            "IT",
            5000
        );

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);

        employee.SetName("New Name");

        await repository.UpdateAsync(employee);

        var updated = await context.Employees.FindAsync(employee.Id);

        Assert.Equal("New Name", updated!.Name);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Employee_When_Exists()
    {
        using var context = CreateContext();

        var employee = new Employee(
            "John",
            "john@test.com",
            "IT",
            5000
        );

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);

        await repository.DeleteAsync(employee.Id);

        var deleted = await context.Employees.FindAsync(employee.Id);

        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_Should_Do_Nothing_When_NotFound()
    {
        using var context = CreateContext();
        var repository = new EmployeeRepository(context);

        await repository.DeleteAsync(999);

        var count = await context.Employees.CountAsync();

        Assert.Equal(0, count);
    }
}