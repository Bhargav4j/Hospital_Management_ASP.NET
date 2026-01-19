using Xunit;
using ClinicManagement.Application.DTOs;
using System;

namespace ClinicManagement.Tests.Unit.Application.DTOs;

public class DoctorDtoTests
{
    [Fact]
    public void DoctorDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var doctorDto = new DoctorDto();

        // Assert
        Assert.Equal(0, doctorDto.Id);
        Assert.Equal(0, doctorDto.UserId);
        Assert.Equal(string.Empty, doctorDto.Name);
        Assert.Equal(string.Empty, doctorDto.Email);
        Assert.Equal(string.Empty, doctorDto.PhoneNumber);
        Assert.Equal(string.Empty, doctorDto.Specialization);
        Assert.Equal(string.Empty, doctorDto.Qualifications);
        Assert.Equal(0, doctorDto.ClinicId);
        Assert.Equal(string.Empty, doctorDto.ClinicName);
    }

    [Fact]
    public void DoctorDto_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctorDto = new DoctorDto();
        var expectedId = 100;

        // Act
        doctorDto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, doctorDto.Id);
    }

    [Fact]
    public void DoctorDto_UserId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctorDto = new DoctorDto();
        var expectedUserId = 200;

        // Act
        doctorDto.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, doctorDto.UserId);
    }

    [Fact]
    public void DoctorDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctorDto = new DoctorDto();
        var expectedId = 1;
        var expectedUserId = 10;
        var expectedName = "Dr. Smith";
        var expectedEmail = "smith@example.com";
        var expectedPhoneNumber = "555-1234";
        var expectedSpecialization = "Cardiology";
        var expectedQualifications = "MD, PhD";
        var expectedClinicId = 5;
        var expectedClinicName = "City Hospital";

        // Act
        doctorDto.Id = expectedId;
        doctorDto.UserId = expectedUserId;
        doctorDto.Name = expectedName;
        doctorDto.Email = expectedEmail;
        doctorDto.PhoneNumber = expectedPhoneNumber;
        doctorDto.Specialization = expectedSpecialization;
        doctorDto.Qualifications = expectedQualifications;
        doctorDto.ClinicId = expectedClinicId;
        doctorDto.ClinicName = expectedClinicName;

        // Assert
        Assert.Equal(expectedId, doctorDto.Id);
        Assert.Equal(expectedUserId, doctorDto.UserId);
        Assert.Equal(expectedName, doctorDto.Name);
        Assert.Equal(expectedEmail, doctorDto.Email);
        Assert.Equal(expectedPhoneNumber, doctorDto.PhoneNumber);
        Assert.Equal(expectedSpecialization, doctorDto.Specialization);
        Assert.Equal(expectedQualifications, doctorDto.Qualifications);
        Assert.Equal(expectedClinicId, doctorDto.ClinicId);
        Assert.Equal(expectedClinicName, doctorDto.ClinicName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Cardiology")]
    [InlineData("Neurology")]
    [InlineData("Orthopedics")]
    public void DoctorDto_Specialization_ShouldAcceptVariousValues(string specialization)
    {
        // Arrange
        var doctorDto = new DoctorDto();

        // Act
        doctorDto.Specialization = specialization;

        // Assert
        Assert.Equal(specialization, doctorDto.Specialization);
    }

    [Theory]
    [InlineData("")]
    [InlineData("MD")]
    [InlineData("MBBS, MS")]
    [InlineData("PhD, FRCS")]
    public void DoctorDto_Qualifications_ShouldAcceptVariousValues(string qualifications)
    {
        // Arrange
        var doctorDto = new DoctorDto();

        // Act
        doctorDto.Qualifications = qualifications;

        // Assert
        Assert.Equal(qualifications, doctorDto.Qualifications);
    }

    [Fact]
    public void DoctorDto_ClinicName_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctorDto = new DoctorDto();
        var expectedClinicName = "Memorial Hospital";

        // Act
        doctorDto.ClinicName = expectedClinicName;

        // Assert
        Assert.Equal(expectedClinicName, doctorDto.ClinicName);
    }
}

public class CreateDoctorDtoTests
{
    [Fact]
    public void CreateDoctorDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var createDoctorDto = new CreateDoctorDto();

        // Assert
        Assert.Equal(string.Empty, createDoctorDto.Name);
        Assert.Equal(string.Empty, createDoctorDto.Email);
        Assert.Equal(string.Empty, createDoctorDto.Password);
        Assert.Equal(string.Empty, createDoctorDto.PhoneNumber);
        Assert.Equal(string.Empty, createDoctorDto.Address);
        Assert.Equal(default(DateTime), createDoctorDto.BirthDate);
        Assert.Equal(string.Empty, createDoctorDto.Gender);
        Assert.Equal(string.Empty, createDoctorDto.Specialization);
        Assert.Equal(string.Empty, createDoctorDto.Qualifications);
        Assert.Equal(0, createDoctorDto.ClinicId);
    }

    [Fact]
    public void CreateDoctorDto_Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedName = "Dr. Johnson";

        // Act
        createDoctorDto.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, createDoctorDto.Name);
    }

    [Fact]
    public void CreateDoctorDto_Email_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedEmail = "johnson@example.com";

        // Act
        createDoctorDto.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, createDoctorDto.Email);
    }

    [Fact]
    public void CreateDoctorDto_Password_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedPassword = "SecurePassword123";

        // Act
        createDoctorDto.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, createDoctorDto.Password);
    }

    [Fact]
    public void CreateDoctorDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedName = "Dr. Brown";
        var expectedEmail = "brown@example.com";
        var expectedPassword = "Pass@123";
        var expectedPhoneNumber = "555-9876";
        var expectedAddress = "789 Main St";
        var expectedBirthDate = new DateTime(1980, 3, 15);
        var expectedGender = "Female";
        var expectedSpecialization = "Pediatrics";
        var expectedQualifications = "MD, FAAP";
        var expectedClinicId = 3;

        // Act
        createDoctorDto.Name = expectedName;
        createDoctorDto.Email = expectedEmail;
        createDoctorDto.Password = expectedPassword;
        createDoctorDto.PhoneNumber = expectedPhoneNumber;
        createDoctorDto.Address = expectedAddress;
        createDoctorDto.BirthDate = expectedBirthDate;
        createDoctorDto.Gender = expectedGender;
        createDoctorDto.Specialization = expectedSpecialization;
        createDoctorDto.Qualifications = expectedQualifications;
        createDoctorDto.ClinicId = expectedClinicId;

        // Assert
        Assert.Equal(expectedName, createDoctorDto.Name);
        Assert.Equal(expectedEmail, createDoctorDto.Email);
        Assert.Equal(expectedPassword, createDoctorDto.Password);
        Assert.Equal(expectedPhoneNumber, createDoctorDto.PhoneNumber);
        Assert.Equal(expectedAddress, createDoctorDto.Address);
        Assert.Equal(expectedBirthDate, createDoctorDto.BirthDate);
        Assert.Equal(expectedGender, createDoctorDto.Gender);
        Assert.Equal(expectedSpecialization, createDoctorDto.Specialization);
        Assert.Equal(expectedQualifications, createDoctorDto.Qualifications);
        Assert.Equal(expectedClinicId, createDoctorDto.ClinicId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Oncology")]
    [InlineData("Dermatology")]
    public void CreateDoctorDto_Specialization_ShouldAcceptVariousValues(string specialization)
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();

        // Act
        createDoctorDto.Specialization = specialization;

        // Assert
        Assert.Equal(specialization, createDoctorDto.Specialization);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    public void CreateDoctorDto_Gender_ShouldAcceptVariousValues(string gender)
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();

        // Act
        createDoctorDto.Gender = gender;

        // Assert
        Assert.Equal(gender, createDoctorDto.Gender);
    }

    [Fact]
    public void CreateDoctorDto_BirthDate_ShouldHandleVariousDates()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var birthDate = new DateTime(1975, 12, 31);

        // Act
        createDoctorDto.BirthDate = birthDate;

        // Assert
        Assert.Equal(birthDate, createDoctorDto.BirthDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public void CreateDoctorDto_ClinicId_ShouldAcceptVariousValues(int clinicId)
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();

        // Act
        createDoctorDto.ClinicId = clinicId;

        // Assert
        Assert.Equal(clinicId, createDoctorDto.ClinicId);
    }

    [Fact]
    public void CreateDoctorDto_PhoneNumber_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedPhoneNumber = "+1-555-123-4567";

        // Act
        createDoctorDto.PhoneNumber = expectedPhoneNumber;

        // Assert
        Assert.Equal(expectedPhoneNumber, createDoctorDto.PhoneNumber);
    }

    [Fact]
    public void CreateDoctorDto_Address_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedAddress = "123 Medical Plaza, Suite 100";

        // Act
        createDoctorDto.Address = expectedAddress;

        // Assert
        Assert.Equal(expectedAddress, createDoctorDto.Address);
    }

    [Fact]
    public void CreateDoctorDto_Qualifications_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createDoctorDto = new CreateDoctorDto();
        var expectedQualifications = "MD, PhD, FACS";

        // Act
        createDoctorDto.Qualifications = expectedQualifications;

        // Assert
        Assert.Equal(expectedQualifications, createDoctorDto.Qualifications);
    }
}
