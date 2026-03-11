namespace EmployeeManagement.Application.DTOs;

public class EmployeeCreateDto
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Department { get; set; }

    public decimal Salary { get; set; }
}