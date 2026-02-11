using HospitalManagement.Domain.DTOs;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class ClinicDtoTests
{
    [Fact]
    public void ClinicDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var clinicDto = new ClinicDto();

        // Assert
        Assert.Equal(0, clinicDto.Id);
        Assert.Equal(string.Empty, clinicDto.Name);
        Assert.Equal(string.Empty, clinicDto.Address);
        Assert.Equal(string.Empty, clinicDto.PhoneNo);
    }

    [Fact]
    public void ClinicDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var clinicDto = new ClinicDto();

        // Act
        clinicDto.Id = 1;
        clinicDto.Name = "City Clinic";
        clinicDto.Address = "123 Medical St";
        clinicDto.PhoneNo = "555-1234";
        clinicDto.Description = "General clinic";

        // Assert
        Assert.Equal(1, clinicDto.Id);
        Assert.Equal("City Clinic", clinicDto.Name);
        Assert.Equal("123 Medical St", clinicDto.Address);
        Assert.Equal("555-1234", clinicDto.PhoneNo);
        Assert.Equal("General clinic", clinicDto.Description);
    }

    [Fact]
    public void ClinicDto_Description_CanBeNull()
    {
        // Arrange & Act
        var clinicDto = new ClinicDto();

        // Assert
        Assert.Null(clinicDto.Description);
    }
}

public class ClinicCreateDtoTests
{
    [Fact]
    public void ClinicCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var clinicCreateDto = new ClinicCreateDto();

        // Assert
        Assert.Equal(string.Empty, clinicCreateDto.Name);
        Assert.Equal(string.Empty, clinicCreateDto.Address);
        Assert.Equal(string.Empty, clinicCreateDto.PhoneNo);
    }

    [Fact]
    public void ClinicCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var clinicCreateDto = new ClinicCreateDto();

        // Act
        clinicCreateDto.Name = "New Clinic";
        clinicCreateDto.Address = "456 Health Ave";
        clinicCreateDto.PhoneNo = "555-5678";
        clinicCreateDto.Description = "Specialty clinic";

        // Assert
        Assert.Equal("New Clinic", clinicCreateDto.Name);
        Assert.Equal("456 Health Ave", clinicCreateDto.Address);
        Assert.Equal("555-5678", clinicCreateDto.PhoneNo);
        Assert.Equal("Specialty clinic", clinicCreateDto.Description);
    }
}

public class ClinicUpdateDtoTests
{
    [Fact]
    public void ClinicUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var clinicUpdateDto = new ClinicUpdateDto();

        // Assert
        Assert.Equal(string.Empty, clinicUpdateDto.Name);
        Assert.Equal(string.Empty, clinicUpdateDto.Address);
        Assert.Equal(string.Empty, clinicUpdateDto.PhoneNo);
    }

    [Fact]
    public void ClinicUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var clinicUpdateDto = new ClinicUpdateDto();

        // Act
        clinicUpdateDto.Name = "Updated Clinic";
        clinicUpdateDto.Address = "789 Care Blvd";
        clinicUpdateDto.PhoneNo = "555-9012";
        clinicUpdateDto.Description = "Updated description";

        // Assert
        Assert.Equal("Updated Clinic", clinicUpdateDto.Name);
        Assert.Equal("789 Care Blvd", clinicUpdateDto.Address);
        Assert.Equal("555-9012", clinicUpdateDto.PhoneNo);
        Assert.Equal("Updated description", clinicUpdateDto.Description);
    }
}
