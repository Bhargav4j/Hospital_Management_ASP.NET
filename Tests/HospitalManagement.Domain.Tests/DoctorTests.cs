using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class DoctorTests
{
    [Fact]
    public void Doctor_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor);
        Assert.Equal(0, doctor.Id);
        Assert.Equal(string.Empty, doctor.FirstName);
        Assert.Equal(string.Empty, doctor.LastName);
        Assert.Equal(string.Empty, doctor.Email);
        Assert.Equal(string.Empty, doctor.Password);
        Assert.Equal(string.Empty, doctor.Gender);
        Assert.Equal(string.Empty, doctor.ContactNumber);
        Assert.Equal(string.Empty, doctor.Address);
        Assert.Equal(0, doctor.Experience);
        Assert.Equal(0m, doctor.Salary);
        Assert.Equal(0m, doctor.ChargesPerVisit);
        Assert.Equal(0, doctor.DepartmentId);
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.Qualification);
        Assert.False(doctor.IsActive);
        Assert.Equal(string.Empty, doctor.CreatedBy);
        Assert.Null(doctor.ModifiedBy);
        Assert.Null(doctor.Department);
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
        Assert.NotNull(doctor.Treatments);
        Assert.Empty(doctor.Treatments);
    }

    [Fact]
    public void Doctor_SetId_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedId = 123;

        // Act
        doctor.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, doctor.Id);
    }

    [Fact]
    public void Doctor_SetFirstName_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedFirstName = "John";

        // Act
        doctor.FirstName = expectedFirstName;

        // Assert
        Assert.Equal(expectedFirstName, doctor.FirstName);
    }

    [Fact]
    public void Doctor_SetLastName_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedLastName = "Doe";

        // Act
        doctor.LastName = expectedLastName;

        // Assert
        Assert.Equal(expectedLastName, doctor.LastName);
    }

    [Fact]
    public void Doctor_SetEmail_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedEmail = "john.doe@hospital.com";

        // Act
        doctor.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, doctor.Email);
    }

    [Fact]
    public void Doctor_SetPassword_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedPassword = "SecurePassword123";

        // Act
        doctor.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, doctor.Password);
    }

    [Fact]
    public void Doctor_SetGender_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedGender = "Male";

        // Act
        doctor.Gender = expectedGender;

        // Assert
        Assert.Equal(expectedGender, doctor.Gender);
    }

    [Fact]
    public void Doctor_SetDateOfBirth_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedDob = new DateTime(1985, 5, 15);

        // Act
        doctor.DateOfBirth = expectedDob;

        // Assert
        Assert.Equal(expectedDob, doctor.DateOfBirth);
    }

    [Fact]
    public void Doctor_SetContactNumber_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedContact = "+1234567890";

        // Act
        doctor.ContactNumber = expectedContact;

        // Assert
        Assert.Equal(expectedContact, doctor.ContactNumber);
    }

    [Fact]
    public void Doctor_SetAddress_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedAddress = "123 Main St, City";

        // Act
        doctor.Address = expectedAddress;

        // Assert
        Assert.Equal(expectedAddress, doctor.Address);
    }

    [Fact]
    public void Doctor_SetExperience_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedExperience = 10;

        // Act
        doctor.Experience = expectedExperience;

        // Assert
        Assert.Equal(expectedExperience, doctor.Experience);
    }

    [Fact]
    public void Doctor_SetExperience_WithZero_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Experience = 0;

        // Assert
        Assert.Equal(0, doctor.Experience);
    }

    [Fact]
    public void Doctor_SetExperience_WithNegativeValue_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Experience = -5;

        // Assert
        Assert.Equal(-5, doctor.Experience);
    }

    [Fact]
    public void Doctor_SetSalary_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedSalary = 75000.50m;

        // Act
        doctor.Salary = expectedSalary;

        // Assert
        Assert.Equal(expectedSalary, doctor.Salary);
    }

    [Fact]
    public void Doctor_SetSalary_WithZero_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Salary = 0m;

        // Assert
        Assert.Equal(0m, doctor.Salary);
    }

    [Fact]
    public void Doctor_SetChargesPerVisit_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedCharges = 150.75m;

        // Act
        doctor.ChargesPerVisit = expectedCharges;

        // Assert
        Assert.Equal(expectedCharges, doctor.ChargesPerVisit);
    }

    [Fact]
    public void Doctor_SetDepartmentId_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedDepartmentId = 5;

        // Act
        doctor.DepartmentId = expectedDepartmentId;

        // Assert
        Assert.Equal(expectedDepartmentId, doctor.DepartmentId);
    }

    [Fact]
    public void Doctor_SetSpecialization_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedSpecialization = "Cardiology";

        // Act
        doctor.Specialization = expectedSpecialization;

        // Assert
        Assert.Equal(expectedSpecialization, doctor.Specialization);
    }

    [Fact]
    public void Doctor_SetQualification_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedQualification = "MD, PhD";

        // Act
        doctor.Qualification = expectedQualification;

        // Assert
        Assert.Equal(expectedQualification, doctor.Qualification);
    }

    [Fact]
    public void Doctor_SetCreatedDate_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedDate = DateTime.UtcNow;

        // Act
        doctor.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, doctor.CreatedDate);
    }

    [Fact]
    public void Doctor_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ModifiedDate = null;

        // Assert
        Assert.Null(doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedDate = DateTime.UtcNow;

        // Act
        doctor.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.IsActive = true;

        // Assert
        Assert.True(doctor.IsActive);
    }

    [Fact]
    public void Doctor_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.IsActive = false;

        // Assert
        Assert.False(doctor.IsActive);
    }

    [Fact]
    public void Doctor_SetCreatedBy_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedCreatedBy = "admin";

        // Act
        doctor.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, doctor.CreatedBy);
    }

    [Fact]
    public void Doctor_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ModifiedBy = null;

        // Assert
        Assert.Null(doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedModifiedBy = "admin2";

        // Act
        doctor.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_SetDepartment_StoresValue()
    {
        // Arrange
        var doctor = new Doctor();
        var department = new Department { Id = 1, Name = "Cardiology" };

        // Act
        doctor.Department = department;

        // Assert
        Assert.NotNull(doctor.Department);
        Assert.Equal(1, doctor.Department.Id);
        Assert.Equal("Cardiology", doctor.Department.Name);
    }

    [Fact]
    public void Doctor_Appointments_CanAddItems()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment = new Appointment { Id = 1 };

        // Act
        doctor.Appointments.Add(appointment);

        // Assert
        Assert.Single(doctor.Appointments);
        Assert.Contains(appointment, doctor.Appointments);
    }

    [Fact]
    public void Doctor_Treatments_CanAddItems()
    {
        // Arrange
        var doctor = new Doctor();
        var treatment = new Treatment { Id = 1 };

        // Act
        doctor.Treatments.Add(treatment);

        // Assert
        Assert.Single(doctor.Treatments);
        Assert.Contains(treatment, doctor.Treatments);
    }

    [Fact]
    public void Doctor_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var doctor = new Doctor();
        var department = new Department { Id = 1, Name = "Surgery" };

        // Act
        doctor.Id = 100;
        doctor.FirstName = "Jane";
        doctor.LastName = "Smith";
        doctor.Email = "jane.smith@hospital.com";
        doctor.Password = "Pass123";
        doctor.Gender = "Female";
        doctor.DateOfBirth = new DateTime(1990, 3, 20);
        doctor.ContactNumber = "+9876543210";
        doctor.Address = "456 Oak Ave";
        doctor.Experience = 8;
        doctor.Salary = 85000m;
        doctor.ChargesPerVisit = 200m;
        doctor.DepartmentId = 1;
        doctor.Specialization = "General Surgery";
        doctor.Qualification = "MBBS, MS";
        doctor.CreatedDate = DateTime.UtcNow;
        doctor.ModifiedDate = DateTime.UtcNow;
        doctor.IsActive = true;
        doctor.CreatedBy = "system";
        doctor.ModifiedBy = "admin";
        doctor.Department = department;

        // Assert
        Assert.Equal(100, doctor.Id);
        Assert.Equal("Jane", doctor.FirstName);
        Assert.Equal("Smith", doctor.LastName);
        Assert.Equal("jane.smith@hospital.com", doctor.Email);
        Assert.Equal("Pass123", doctor.Password);
        Assert.Equal("Female", doctor.Gender);
        Assert.Equal(new DateTime(1990, 3, 20), doctor.DateOfBirth);
        Assert.Equal("+9876543210", doctor.ContactNumber);
        Assert.Equal("456 Oak Ave", doctor.Address);
        Assert.Equal(8, doctor.Experience);
        Assert.Equal(85000m, doctor.Salary);
        Assert.Equal(200m, doctor.ChargesPerVisit);
        Assert.Equal(1, doctor.DepartmentId);
        Assert.Equal("General Surgery", doctor.Specialization);
        Assert.Equal("MBBS, MS", doctor.Qualification);
        Assert.True(doctor.IsActive);
        Assert.Equal("system", doctor.CreatedBy);
        Assert.Equal("admin", doctor.ModifiedBy);
        Assert.NotNull(doctor.Department);
    }
}
