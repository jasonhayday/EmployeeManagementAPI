using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllAsync();

    Task<EmployeeResponseDto?> GetByIdAsync(int id);

    Task<EmployeeResponseDto> CreateAsync(EmployeeCreateDto dto);

    Task UpdateAsync(int id, EmployeeCreateDto dto);

    Task DeleteAsync(int id);
}