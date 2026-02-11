using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class PatientTests
{
    [Fact]
    public void Patient_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient);
        Assert.Equal(0, patient.Id);
        Assert.Equal(string.Empty, patient.FirstName);
        Assert.Equal(string.Empty, patient.LastName);
        Assert.Equal(string.Empty, patient.Email);
        Assert.Equal(string.Empty, patient.Password);
        Assert.Equal(string.Empty, patient.Gender);
        Assert.Equal(default(DateTime), patient.DateOfBirth);
        Assert.Equal(string.Empty, patient.ContactNumber);
        Assert.Equal(string.Empty, patient.Address);
        Assert.Equal(string.Empty, patient.BloodGroup);
        Assert.Equal(default(DateTime), patient.CreatedDate);
        Assert.Null(patient.ModifiedDate);
        Assert.False(patient.IsActive);
        Assert.Equal(string.Empty, patient.CreatedBy);
        Assert.Null(patient.ModifiedBy);
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
        Assert.NotNull(patient.Treatments);
        Assert.Empty(patient.Treatments);
        Assert.NotNull(patient.Bills);
        Assert.Empty(patient.Bills);
    }

    [Fact]
    public void Patient_SetId_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 789;

        // Act
        patient.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, patient.Id);
    }

    [Fact]
    public void Patient_SetFirstName_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedFirstName = "Sarah";

        // Act
        patient.FirstName = expectedFirstName;

        // Assert
        Assert.Equal(expectedFirstName, patient.FirstName);
    }

    [Fact]
    public void Patient_SetLastName_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedLastName = "Johnson";

        // Act
        patient.LastName = expectedLastName;

        // Assert
        Assert.Equal(expectedLastName, patient.LastName);
    }

    [Fact]
    public void Patient_SetEmail_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedEmail = "sarah.johnson@email.com";

        // Act
        patient.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, patient.Email);
    }

    [Fact]
    public void Patient_SetPassword_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedPassword = "SecurePass456";

        // Act
        patient.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, patient.Password);
    }

    [Fact]
    public void Patient_SetGender_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedGender = "Female";

        // Act
        patient.Gender = expectedGender;

        // Assert
        Assert.Equal(expectedGender, patient.Gender);
    }

    [Fact]
    public void Patient_SetDateOfBirth_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedDob = new DateTime(1992, 8, 25);

        // Act
        patient.DateOfBirth = expectedDob;

        // Assert
        Assert.Equal(expectedDob, patient.DateOfBirth);
    }

    [Fact]
    public void Patient_SetContactNumber_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedContact = "+1122334455";

        // Act
        patient.ContactNumber = expectedContact;

        // Assert
        Assert.Equal(expectedContact, patient.ContactNumber);
    }

    [Fact]
    public void Patient_SetAddress_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedAddress = "789 Pine St, Town";

        // Act
        patient.Address = expectedAddress;

        // Assert
        Assert.Equal(expectedAddress, patient.Address);
    }

    [Fact]
    public void Patient_SetBloodGroup_WithAPositive_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedBloodGroup = "A+";

        // Act
        patient.BloodGroup = expectedBloodGroup;

        // Assert
        Assert.Equal(expectedBloodGroup, patient.BloodGroup);
    }

    [Fact]
    public void Patient_SetBloodGroup_WithONegative_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedBloodGroup = "O-";

        // Act
        patient.BloodGroup = expectedBloodGroup;

        // Assert
        Assert.Equal(expectedBloodGroup, patient.BloodGroup);
    }

    [Fact]
    public void Patient_SetBloodGroup_WithABPositive_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedBloodGroup = "AB+";

        // Act
        patient.BloodGroup = expectedBloodGroup;

        // Assert
        Assert.Equal(expectedBloodGroup, patient.BloodGroup);
    }

    [Fact]
    public void Patient_SetCreatedDate_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedDate = DateTime.UtcNow;

        // Act
        patient.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patient.CreatedDate);
    }

    [Fact]
    public void Patient_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedDate = null;

        // Assert
        Assert.Null(patient.ModifiedDate);
    }

    [Fact]
    public void Patient_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedDate = DateTime.UtcNow;

        // Act
        patient.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patient.ModifiedDate);
    }

    [Fact]
    public void Patient_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.IsActive = true;

        // Assert
        Assert.True(patient.IsActive);
    }

    [Fact]
    public void Patient_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.IsActive = false;

        // Assert
        Assert.False(patient.IsActive);
    }

    [Fact]
    public void Patient_SetCreatedBy_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedCreatedBy = "registrar";

        // Act
        patient.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, patient.CreatedBy);
    }

    [Fact]
    public void Patient_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedBy = null;

        // Assert
        Assert.Null(patient.ModifiedBy);
    }

    [Fact]
    public void Patient_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var patient = new Patient();
        var expectedModifiedBy = "staff";

        // Act
        patient.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
    }

    [Fact]
    public void Patient_Appointments_CanAddItems()
    {
        // Arrange
        var patient = new Patient();
        var appointment = new Appointment { Id = 1 };

        // Act
        patient.Appointments.Add(appointment);

        // Assert
        Assert.Single(patient.Appointments);
        Assert.Contains(appointment, patient.Appointments);
    }

    [Fact]
    public void Patient_Treatments_CanAddItems()
    {
        // Arrange
        var patient = new Patient();
        var treatment = new Treatment { Id = 1 };

        // Act
        patient.Treatments.Add(treatment);

        // Assert
        Assert.Single(patient.Treatments);
        Assert.Contains(treatment, patient.Treatments);
    }

    [Fact]
    public void Patient_Bills_CanAddItems()
    {
        // Arrange
        var patient = new Patient();
        var bill = new Bill { Id = 1 };

        // Act
        patient.Bills.Add(bill);

        // Assert
        Assert.Single(patient.Bills);
        Assert.Contains(bill, patient.Bills);
    }

    [Fact]
    public void Patient_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var patient = new Patient();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;
        var dob = new DateTime(1988, 11, 10);

        // Act
        patient.Id = 200;
        patient.FirstName = "Michael";
        patient.LastName = "Williams";
        patient.Email = "michael.williams@email.com";
        patient.Password = "Password789";
        patient.Gender = "Male";
        patient.DateOfBirth = dob;
        patient.ContactNumber = "+5544332211";
        patient.Address = "321 Elm St, Village";
        patient.BloodGroup = "B+";
        patient.CreatedDate = createdDate;
        patient.ModifiedDate = modifiedDate;
        patient.IsActive = true;
        patient.CreatedBy = "admin";
        patient.ModifiedBy = "clerk";

        // Assert
        Assert.Equal(200, patient.Id);
        Assert.Equal("Michael", patient.FirstName);
        Assert.Equal("Williams", patient.LastName);
        Assert.Equal("michael.williams@email.com", patient.Email);
        Assert.Equal("Password789", patient.Password);
        Assert.Equal("Male", patient.Gender);
        Assert.Equal(dob, patient.DateOfBirth);
        Assert.Equal("+5544332211", patient.ContactNumber);
        Assert.Equal("321 Elm St, Village", patient.Address);
        Assert.Equal("B+", patient.BloodGroup);
        Assert.Equal(createdDate, patient.CreatedDate);
        Assert.Equal(modifiedDate, patient.ModifiedDate);
        Assert.True(patient.IsActive);
        Assert.Equal("admin", patient.CreatedBy);
        Assert.Equal("clerk", patient.ModifiedBy);
    }

    [Fact]
    public void Patient_MultipleCollections_CanAddMultipleItems()
    {
        // Arrange
        var patient = new Patient();
        var appointment1 = new Appointment { Id = 1 };
        var appointment2 = new Appointment { Id = 2 };
        var treatment1 = new Treatment { Id = 1 };
        var treatment2 = new Treatment { Id = 2 };
        var bill1 = new Bill { Id = 1 };
        var bill2 = new Bill { Id = 2 };

        // Act
        patient.Appointments.Add(appointment1);
        patient.Appointments.Add(appointment2);
        patient.Treatments.Add(treatment1);
        patient.Treatments.Add(treatment2);
        patient.Bills.Add(bill1);
        patient.Bills.Add(bill2);

        // Assert
        Assert.Equal(2, patient.Appointments.Count);
        Assert.Equal(2, patient.Treatments.Count);
        Assert.Equal(2, patient.Bills.Count);
    }
}
