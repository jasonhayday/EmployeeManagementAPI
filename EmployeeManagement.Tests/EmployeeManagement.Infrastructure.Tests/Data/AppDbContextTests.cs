using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data.Tests;

public class AppDbContextTests
{
    private DbContextOptions<AppDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Should_Create_DbContext()
    {
        var options = CreateOptions();

        using var context = new AppDbContext(options);

        Assert.NotNull(context);
        Assert.NotNull(context.Employees);
    }

    [Fact]
    public async Task Should_Add_Employee_To_Database()
    {
        var options = CreateOptions();

        using var context = new AppDbContext(options);

        var employee = new Employee(
            "Jason",
            "jason@mail.com",
            "IT",
            5000
        );

        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();

        var result = await context.Employees.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal("Jason", result!.Name);
    }

    [Fact]
    public async Task Should_Read_Employees_From_Database()
    {
        var options = CreateOptions();

        using (var context = new AppDbContext(options))
        {
            context.Employees.Add(
                new Employee("Jason", "jason@mail.com", "IT", 5000)
            );

            context.Employees.Add(
                new Employee("Alice", "alice@mail.com", "HR", 4000)
            );

            await context.SaveChangesAsync();
        }

        using (var context = new AppDbContext(options))
        {
            var employees = await context.Employees.ToListAsync();

            Assert.Equal(2, employees.Count);
        }
    }
}