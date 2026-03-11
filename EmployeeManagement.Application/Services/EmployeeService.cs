using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();

        return employees.Select(e => new EmployeeResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department,
            Salary = e.Salary
        });
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return null;

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Department = employee.Department,
            Salary = employee.Salary
        };
    }

    public async Task<EmployeeResponseDto> CreateAsync(EmployeeCreateDto dto)
    {
        var employee = new Employee(
            dto.Name,
            dto.Email,
            dto.Department,
            dto.Salary
        );

        await _repository.AddAsync(employee);

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Department = employee.Department,
            Salary = employee.Salary
        };
    }

    public async Task UpdateAsync(int id, EmployeeCreateDto dto)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            throw new Exception("Employee not found");

        employee.SetName(dto.Name);
        employee.SetEmail(dto.Email);
        employee.SetDepartment(dto.Department);
        employee.UpdateSalary(dto.Salary);

        await _repository.UpdateAsync(employee);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}