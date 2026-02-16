using Xunit;
using Moq;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Tests.Services
{
    public class OtherStaffServiceTests
    {
        private readonly Mock<IOtherStaffRepository> _mockRepository;
        private readonly OtherStaffService _service;

        public OtherStaffServiceTests()
        {
            _mockRepository = new Mock<IOtherStaffRepository>();
            _service = new OtherStaffService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetStaffByIdAsync_WithValidId_ReturnsStaff()
        {
            // Arrange
            var staff = new OtherStaff { StaffID = 1, Name = "Jane Nurse" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(staff);

            // Act
            var result = await _service.GetStaffByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.StaffID);
            Assert.Equal("Jane Nurse", result.Name);
        }

        [Fact]
        public async Task GetStaffByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OtherStaff?)null);

            // Act
            var result = await _service.GetStaffByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllStaffAsync_ReturnsAllStaff()
        {
            // Arrange
            var staffList = new List<OtherStaff>
            {
                new OtherStaff { StaffID = 1, Name = "Jane Nurse" },
                new OtherStaff { StaffID = 2, Name = "John Tech" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(staffList);

            // Act
            var result = await _service.GetAllStaffAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateStaffAsync_WithValidStaff_ReturnsCreatedStaff()
        {
            // Arrange
            var staff = new OtherStaff { Name = "Jane Nurse", Designation = "Nurse" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<OtherStaff>())).ReturnsAsync(staff);

            // Act
            var result = await _service.CreateStaffAsync(staff);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<OtherStaff>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStaffAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var staff = new OtherStaff { StaffID = 1, Name = "Updated Name" };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<OtherStaff>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateStaffAsync(staff);

            // Assert
            Assert.NotNull(staff.ModifiedDate);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<OtherStaff>()), Times.Once);
        }

        [Fact]
        public async Task DeleteStaffAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteStaffAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetStaffByDesignationAsync_ReturnsStaffWithDesignation()
        {
            // Arrange
            var staffList = new List<OtherStaff>
            {
                new OtherStaff { StaffID = 1, Designation = "Nurse" }
            };
            _mockRepository.Setup(r => r.GetByDesignationAsync("Nurse")).ReturnsAsync(staffList);

            // Act
            var result = await _service.GetStaffByDesignationAsync("Nurse");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
