using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

public class ClinicTests
{
    [Fact]
    public void Clinic_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Equal(0, clinic.Id);
        Assert.Equal(string.Empty, clinic.Name);
        Assert.Equal(string.Empty, clinic.Address);
        Assert.Equal(string.Empty, clinic.PhoneNumber);
        Assert.Equal(default(DateTime), clinic.CreatedAt);
        Assert.Null(clinic.UpdatedAt);
        Assert.NotNull(clinic.Doctors);
        Assert.Empty(clinic.Doctors);
    }

    [Fact]
    public void Clinic_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedId = 50;

        // Act
        clinic.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, clinic.Id);
    }

    [Fact]
    public void Clinic_Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedName = "General Hospital";

        // Act
        clinic.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, clinic.Name);
    }

    [Fact]
    public void Clinic_Address_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedAddress = "123 Main Street, City";

        // Act
        clinic.Address = expectedAddress;

        // Assert
        Assert.Equal(expectedAddress, clinic.Address);
    }

    [Fact]
    public void Clinic_PhoneNumber_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedPhoneNumber = "+1234567890";

        // Act
        clinic.PhoneNumber = expectedPhoneNumber;

        // Assert
        Assert.Equal(expectedPhoneNumber, clinic.PhoneNumber);
    }

    [Fact]
    public void Clinic_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedCreatedAt = DateTime.Now;

        // Act
        clinic.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, clinic.CreatedAt);
    }

    [Fact]
    public void Clinic_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        clinic.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, clinic.UpdatedAt);
    }

    [Fact]
    public void Clinic_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.UpdatedAt = null;

        // Assert
        Assert.Null(clinic.UpdatedAt);
    }

    [Fact]
    public void Clinic_Doctors_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.NotNull(clinic.Doctors);
        Assert.IsAssignableFrom<ICollection<Doctor>>(clinic.Doctors);
        Assert.Empty(clinic.Doctors);
    }

    [Fact]
    public void Clinic_Doctors_ShouldAddDoctorsCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var doctor1 = new Doctor { Id = 1, Specialization = "Cardiology" };
        var doctor2 = new Doctor { Id = 2, Specialization = "Neurology" };

        // Act
        clinic.Doctors.Add(doctor1);
        clinic.Doctors.Add(doctor2);

        // Assert
        Assert.Equal(2, clinic.Doctors.Count);
        Assert.Contains(doctor1, clinic.Doctors);
        Assert.Contains(doctor2, clinic.Doctors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Clinic_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Id = id;

        // Assert
        Assert.Equal(id, clinic.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("City Clinic")]
    [InlineData("Memorial Hospital")]
    [InlineData("ABC Medical Center")]
    public void Clinic_Name_ShouldAcceptVariousValues(string name)
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Name = name;

        // Assert
        Assert.Equal(name, clinic.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123 Street")]
    [InlineData("456 Oak Ave, Suite 100")]
    public void Clinic_Address_ShouldAcceptVariousValues(string address)
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Address = address;

        // Assert
        Assert.Equal(address, clinic.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData("555-1234")]
    [InlineData("+1-555-123-4567")]
    public void Clinic_PhoneNumber_ShouldAcceptVariousValues(string phoneNumber)
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.PhoneNumber = phoneNumber;

        // Assert
        Assert.Equal(phoneNumber, clinic.PhoneNumber);
    }

    [Fact]
    public void Clinic_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedId = 99;
        var expectedName = "Central Clinic";
        var expectedAddress = "789 Central Ave";
        var expectedPhoneNumber = "555-9999";
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddDays(1);
        var doctor = new Doctor { Id = 5 };

        // Act
        clinic.Id = expectedId;
        clinic.Name = expectedName;
        clinic.Address = expectedAddress;
        clinic.PhoneNumber = expectedPhoneNumber;
        clinic.CreatedAt = expectedCreatedAt;
        clinic.UpdatedAt = expectedUpdatedAt;
        clinic.Doctors.Add(doctor);

        // Assert
        Assert.Equal(expectedId, clinic.Id);
        Assert.Equal(expectedName, clinic.Name);
        Assert.Equal(expectedAddress, clinic.Address);
        Assert.Equal(expectedPhoneNumber, clinic.PhoneNumber);
        Assert.Equal(expectedCreatedAt, clinic.CreatedAt);
        Assert.Equal(expectedUpdatedAt, clinic.UpdatedAt);
        Assert.Single(clinic.Doctors);
    }

    [Fact]
    public void Clinic_Doctors_ShouldAllowReassignment()
    {
        // Arrange
        var clinic = new Clinic();
        var newDoctors = new List<Doctor>
        {
            new Doctor { Id = 1 },
            new Doctor { Id = 2 },
            new Doctor { Id = 3 }
        };

        // Act
        clinic.Doctors = newDoctors;

        // Assert
        Assert.Equal(3, clinic.Doctors.Count);
        Assert.Same(newDoctors, clinic.Doctors);
    }

    [Fact]
    public void Clinic_Doctors_ShouldRemoveDoctors()
    {
        // Arrange
        var clinic = new Clinic();
        var doctor = new Doctor { Id = 10 };
        clinic.Doctors.Add(doctor);

        // Act
        var removed = clinic.Doctors.Remove(doctor);

        // Assert
        Assert.True(removed);
        Assert.Empty(clinic.Doctors);
    }

    [Fact]
    public void Clinic_Doctors_ShouldClearAllDoctors()
    {
        // Arrange
        var clinic = new Clinic();
        clinic.Doctors.Add(new Doctor { Id = 1 });
        clinic.Doctors.Add(new Doctor { Id = 2 });
        clinic.Doctors.Add(new Doctor { Id = 3 });

        // Act
        clinic.Doctors.Clear();

        // Assert
        Assert.Empty(clinic.Doctors);
    }
}
