using HospitalManagement.Domain.Entities;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class ClinicTests
{
    [Fact]
    public void Clinic_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Equal(0, clinic.Id);
        Assert.Equal(string.Empty, clinic.Name);
        Assert.Equal(string.Empty, clinic.Address);
        Assert.Equal(string.Empty, clinic.PhoneNo);
        Assert.Equal("System", clinic.CreatedBy);
        Assert.False(clinic.IsActive);
    }

    [Fact]
    public void Clinic_SetProperties_ShouldSetValues()
    {
        // Arrange
        var clinic = new Clinic();
        var createdDate = DateTime.UtcNow;

        // Act
        clinic.Id = 1;
        clinic.Name = "City Hospital";
        clinic.Address = "123 Health St";
        clinic.PhoneNo = "555-1234";
        clinic.Description = "General practice clinic";
        clinic.CreatedDate = createdDate;
        clinic.IsActive = true;

        // Assert
        Assert.Equal(1, clinic.Id);
        Assert.Equal("City Hospital", clinic.Name);
        Assert.Equal("123 Health St", clinic.Address);
        Assert.Equal("555-1234", clinic.PhoneNo);
        Assert.Equal("General practice clinic", clinic.Description);
        Assert.Equal(createdDate, clinic.CreatedDate);
        Assert.True(clinic.IsActive);
    }

    [Fact]
    public void Clinic_Description_CanBeNull()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Null(clinic.Description);
    }

    [Fact]
    public void Clinic_Description_CanBeSet()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Description = "Specializes in pediatrics";

        // Assert
        Assert.Equal("Specializes in pediatrics", clinic.Description);
    }

    [Fact]
    public void Clinic_Name_CanBeEmpty()
    {
        // Arrange & Act
        var clinic = new Clinic { Name = string.Empty };

        // Assert
        Assert.Equal(string.Empty, clinic.Name);
    }

    [Fact]
    public void Clinic_Address_CanBeEmpty()
    {
        // Arrange & Act
        var clinic = new Clinic { Address = string.Empty };

        // Assert
        Assert.Equal(string.Empty, clinic.Address);
    }

    [Fact]
    public void Clinic_PhoneNo_CanBeEmpty()
    {
        // Arrange & Act
        var clinic = new Clinic { PhoneNo = string.Empty };

        // Assert
        Assert.Equal(string.Empty, clinic.PhoneNo);
    }

    [Fact]
    public void Clinic_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Null(clinic.ModifiedDate);
    }

    [Fact]
    public void Clinic_ModifiedDate_CanBeSet()
    {
        // Arrange
        var clinic = new Clinic();
        var modifiedDate = DateTime.UtcNow;

        // Act
        clinic.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, clinic.ModifiedDate);
    }

    [Fact]
    public void Clinic_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Null(clinic.ModifiedBy);
    }

    [Fact]
    public void Clinic_ModifiedBy_CanBeSet()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", clinic.ModifiedBy);
    }

    [Fact]
    public void Clinic_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Equal("System", clinic.CreatedBy);
    }
}
