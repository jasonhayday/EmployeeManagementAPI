namespace EmployeeManagement.Application.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Department { get; set; }

    public decimal Salary { get; set; }
}