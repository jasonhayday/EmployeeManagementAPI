namespace EmployeeManagement.Application.DTOs;

public class EmployeeCreateDto
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }

    public decimal Salary { get; set; }
}