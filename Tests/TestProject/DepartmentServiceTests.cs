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
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _mockRepository;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _mockRepository = new Mock<IDepartmentRepository>();
            _service = new DepartmentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_WithValidId_ReturnsDepartment()
        {
            // Arrange
            var department = new Department { DeptNo = 1, DeptName = "Cardiology" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

            // Act
            var result = await _service.GetDepartmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DeptNo);
            Assert.Equal("Cardiology", result.DeptName);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Department?)null);

            // Act
            var result = await _service.GetDepartmentByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ReturnsAllDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { DeptNo = 1, DeptName = "Cardiology" },
                new Department { DeptNo = 2, DeptName = "Neurology" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

            // Act
            var result = await _service.GetAllDepartmentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateDepartmentAsync_WithValidDepartment_ReturnsCreatedDepartment()
        {
            // Arrange
            var department = new Department { DeptName = "Cardiology", Description = "Heart care" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Department>())).ReturnsAsync(department);

            // Act
            var result = await _service.CreateDepartmentAsync(department);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Department>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var department = new Department { DeptNo = 1, DeptName = "Updated Name" };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Department>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateDepartmentAsync(department);

            // Assert
            Assert.NotNull(department.ModifiedDate);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Department>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteDepartmentAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
