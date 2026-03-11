namespace EmployeeManagement.Application.DTOs.Tests;

public class EmployeeResponseDtoTests
{
    [Fact]
    public void Properties_Should_Set_And_Get_Correctly()
    {
        // Arrange
        var dto = new EmployeeResponseDto
        {
            Id = 1,
            Name = "Jason",
            Email = "jason@email.com",
            Department = "IT",
            Salary = 5000
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Jason", dto.Name);
        Assert.Equal("jason@email.com", dto.Email);
        Assert.Equal("IT", dto.Department);
        Assert.Equal(5000, dto.Salary);
    }
}