namespace EmployeeManagement.Domain.Entities;

public class Employee
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Department { get; private set; } = null!;

    public decimal Salary { get; private set; }

    private Employee() { }

    public Employee(string name, string email, string department, decimal salary)
    {
        SetName(name);
        SetEmail(email);
        SetDepartment(department);
        UpdateSalary(salary);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        Email = email;
    }

    public void SetDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department cannot be empty");

        Department = department;
    }

    public void UpdateSalary(decimal salary)
    {
        if (salary <= 0)
            throw new ArgumentException("Salary must be greater than zero");

        Salary = salary;
    }
}