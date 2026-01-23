using Xunit;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Data.Configurations;
using System;

namespace ClinicManagement.Infrastructure.Data.Configurations.Tests;

/// <summary>
/// Unit tests for PatientConfiguration
/// </summary>
public class PatientConfigurationTests
{
    private ClinicDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicDbContext(options);
    }

    [Fact]
    public void PatientConfiguration_Configure_ShouldSetTableName()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("Patients", entityType.GetTableName());
    }

    [Fact]
    public void PatientConfiguration_Configure_ShouldSetPrimaryKey()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var key = entityType.FindPrimaryKey();
        Assert.NotNull(key);
        Assert.Single(key.Properties);
        Assert.Equal("Id", key.Properties[0].Name);
    }

    [Fact]
    public void PatientConfiguration_Configure_Name_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("Name");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(100, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_Email_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("Email");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(100, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_Email_ShouldHaveUniqueIndex()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var indexes = entityType.GetIndexes();
        var emailIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "Email"));
        Assert.NotNull(emailIndex);
        Assert.True(emailIndex.IsUnique);
    }

    [Fact]
    public void PatientConfiguration_Configure_PasswordHash_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("PasswordHash");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(255, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_PhoneNo_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("PhoneNo");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(20, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_Gender_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("Gender");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(10, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_Address_ShouldHaveMaxLength()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("Address");
        Assert.NotNull(property);
        Assert.Equal(500, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_CreatedBy_ShouldBeRequired()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("CreatedBy");
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(100, property.GetMaxLength());
    }

    [Fact]
    public void PatientConfiguration_Configure_ModifiedBy_ShouldHaveMaxLength()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();
        var entityType = context.Model.FindEntityType(typeof(Patient));

        // Assert
        Assert.NotNull(entityType);
        var property = entityType.FindProperty("ModifiedBy");
        Assert.NotNull(property);
        Assert.Equal(100, property.GetMaxLength());
    }
}
