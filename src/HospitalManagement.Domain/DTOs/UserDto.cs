using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
    public UserType UserType { get; set; }
}

public class UserCreateDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
    public UserType UserType { get; set; }
}

public class UserUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResultDto
{
    public bool Success { get; set; }
    public int UserId { get; set; }
    public UserType UserType { get; set; }
    public string Message { get; set; } = string.Empty;
}
