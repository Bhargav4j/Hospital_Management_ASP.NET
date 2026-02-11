using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class DepartmentTests
{
    [Fact]
    public void Department_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.NotNull(department);
        Assert.Equal(0, department.Id);
        Assert.Equal(string.Empty, department.Name);
        Assert.Null(department.Description);
        Assert.Equal(default(DateTime), department.CreatedDate);
        Assert.Null(department.ModifiedDate);
        Assert.False(department.IsActive);
        Assert.Equal(string.Empty, department.CreatedBy);
        Assert.Null(department.ModifiedBy);
        Assert.NotNull(department.Doctors);
        Assert.Empty(department.Doctors);
    }

    [Fact]
    public void Department_SetId_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedId = 5;

        // Act
        department.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, department.Id);
    }

    [Fact]
    public void Department_SetName_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedName = "Cardiology";

        // Act
        department.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, department.Name);
    }

    [Fact]
    public void Department_SetDescription_WithNull_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.Description = null;

        // Assert
        Assert.Null(department.Description);
    }

    [Fact]
    public void Department_SetDescription_WithValue_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedDescription = "Heart and cardiovascular treatment";

        // Act
        department.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, department.Description);
    }

    [Fact]
    public void Department_SetCreatedDate_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedDate = DateTime.UtcNow;

        // Act
        department.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, department.CreatedDate);
    }

    [Fact]
    public void Department_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.ModifiedDate = null;

        // Assert
        Assert.Null(department.ModifiedDate);
    }

    [Fact]
    public void Department_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedDate = DateTime.UtcNow;

        // Act
        department.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, department.ModifiedDate);
    }

    [Fact]
    public void Department_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.IsActive = true;

        // Assert
        Assert.True(department.IsActive);
    }

    [Fact]
    public void Department_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.IsActive = false;

        // Assert
        Assert.False(department.IsActive);
    }

    [Fact]
    public void Department_SetCreatedBy_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedCreatedBy = "admin";

        // Act
        department.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, department.CreatedBy);
    }

    [Fact]
    public void Department_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.ModifiedBy = null;

        // Assert
        Assert.Null(department.ModifiedBy);
    }

    [Fact]
    public void Department_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var department = new Department();
        var expectedModifiedBy = "manager";

        // Act
        department.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, department.ModifiedBy);
    }

    [Fact]
    public void Department_Doctors_CanAddItems()
    {
        // Arrange
        var department = new Department();
        var doctor = new Doctor { Id = 1, FirstName = "John", LastName = "Doe" };

        // Act
        department.Doctors.Add(doctor);

        // Assert
        Assert.Single(department.Doctors);
        Assert.Contains(doctor, department.Doctors);
    }

    [Fact]
    public void Department_Doctors_CanAddMultipleItems()
    {
        // Arrange
        var department = new Department();
        var doctor1 = new Doctor { Id = 1, FirstName = "John", LastName = "Doe" };
        var doctor2 = new Doctor { Id = 2, FirstName = "Jane", LastName = "Smith" };
        var doctor3 = new Doctor { Id = 3, FirstName = "Bob", LastName = "Brown" };

        // Act
        department.Doctors.Add(doctor1);
        department.Doctors.Add(doctor2);
        department.Doctors.Add(doctor3);

        // Assert
        Assert.Equal(3, department.Doctors.Count);
        Assert.Contains(doctor1, department.Doctors);
        Assert.Contains(doctor2, department.Doctors);
        Assert.Contains(doctor3, department.Doctors);
    }

    [Fact]
    public void Department_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var department = new Department();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        department.Id = 10;
        department.Name = "Neurology";
        department.Description = "Brain and nervous system treatment";
        department.CreatedDate = createdDate;
        department.ModifiedDate = modifiedDate;
        department.IsActive = true;
        department.CreatedBy = "system";
        department.ModifiedBy = "admin";

        // Assert
        Assert.Equal(10, department.Id);
        Assert.Equal("Neurology", department.Name);
        Assert.Equal("Brain and nervous system treatment", department.Description);
        Assert.Equal(createdDate, department.CreatedDate);
        Assert.Equal(modifiedDate, department.ModifiedDate);
        Assert.True(department.IsActive);
        Assert.Equal("system", department.CreatedBy);
        Assert.Equal("admin", department.ModifiedBy);
    }

    [Fact]
    public void Department_SetName_WithEmptyString_StoresValue()
    {
        // Arrange
        var department = new Department();

        // Act
        department.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, department.Name);
    }

    [Theory]
    [InlineData("Emergency")]
    [InlineData("Pediatrics")]
    [InlineData("Orthopedics")]
    [InlineData("Dermatology")]
    public void Department_SetName_WithDifferentValues_StoresCorrectly(string departmentName)
    {
        // Arrange
        var department = new Department();

        // Act
        department.Name = departmentName;

        // Assert
        Assert.Equal(departmentName, department.Name);
    }

    [Fact]
    public void Department_CreatedDate_CanStoreMinValue()
    {
        // Arrange
        var department = new Department();
        var minDate = DateTime.MinValue;

        // Act
        department.CreatedDate = minDate;

        // Assert
        Assert.Equal(minDate, department.CreatedDate);
    }

    [Fact]
    public void Department_CreatedDate_CanStoreMaxValue()
    {
        // Arrange
        var department = new Department();
        var maxDate = DateTime.MaxValue;

        // Act
        department.CreatedDate = maxDate;

        // Assert
        Assert.Equal(maxDate, department.CreatedDate);
    }
}
