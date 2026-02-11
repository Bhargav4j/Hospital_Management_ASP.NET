using HospitalManagement.Domain.DTOs;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class BillDtoTests
{
    [Fact]
    public void BillDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var billDto = new BillDto();

        // Assert
        Assert.Equal(0, billDto.Id);
        Assert.Equal(0, billDto.PatientId);
        Assert.Equal(string.Empty, billDto.PatientName);
        Assert.Equal(0m, billDto.Amount);
        Assert.Equal(string.Empty, billDto.Description);
        Assert.False(billDto.IsPaid);
    }

    [Fact]
    public void BillDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var billDto = new BillDto();
        var billDate = DateTime.UtcNow;

        // Act
        billDto.Id = 1;
        billDto.PatientId = 10;
        billDto.PatientName = "John Doe";
        billDto.Amount = 250.50m;
        billDto.BillDate = billDate;
        billDto.Description = "Lab tests";
        billDto.IsPaid = true;

        // Assert
        Assert.Equal(1, billDto.Id);
        Assert.Equal(10, billDto.PatientId);
        Assert.Equal("John Doe", billDto.PatientName);
        Assert.Equal(250.50m, billDto.Amount);
        Assert.Equal(billDate, billDto.BillDate);
        Assert.Equal("Lab tests", billDto.Description);
        Assert.True(billDto.IsPaid);
    }
}

public class BillCreateDtoTests
{
    [Fact]
    public void BillCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var billCreateDto = new BillCreateDto();

        // Assert
        Assert.Equal(0, billCreateDto.PatientId);
        Assert.Equal(0m, billCreateDto.Amount);
        Assert.Equal(string.Empty, billCreateDto.Description);
    }

    [Fact]
    public void BillCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var billCreateDto = new BillCreateDto();

        // Act
        billCreateDto.PatientId = 10;
        billCreateDto.Amount = 100.00m;
        billCreateDto.Description = "Consultation";

        // Assert
        Assert.Equal(10, billCreateDto.PatientId);
        Assert.Equal(100.00m, billCreateDto.Amount);
        Assert.Equal("Consultation", billCreateDto.Description);
    }
}

public class BillUpdateDtoTests
{
    [Fact]
    public void BillUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var billUpdateDto = new BillUpdateDto();

        // Assert
        Assert.Equal(0m, billUpdateDto.Amount);
        Assert.Equal(string.Empty, billUpdateDto.Description);
        Assert.False(billUpdateDto.IsPaid);
    }

    [Fact]
    public void BillUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var billUpdateDto = new BillUpdateDto();

        // Act
        billUpdateDto.Amount = 150.75m;
        billUpdateDto.Description = "Updated description";
        billUpdateDto.IsPaid = true;

        // Assert
        Assert.Equal(150.75m, billUpdateDto.Amount);
        Assert.Equal("Updated description", billUpdateDto.Description);
        Assert.True(billUpdateDto.IsPaid);
    }
}
