namespace ClinicManagement.Application.DTOs;

public class DepartmentDto
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateDepartmentDto
{
    public string DeptName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateDepartmentDto
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
