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
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _mockRepository;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _mockRepository = new Mock<IDoctorRepository>();
            _service = new DoctorService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WithValidId_ReturnsDoctor()
        {
            // Arrange
            var doctor = new Doctor { DoctorID = 1, Name = "Dr. Smith" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            // Act
            var result = await _service.GetDoctorByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorID);
            Assert.Equal("Dr. Smith", result.Name);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Doctor?)null);

            // Act
            var result = await _service.GetDoctorByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsAllDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorID = 1, Name = "Dr. Smith" },
                new Doctor { DoctorID = 2, Name = "Dr. Jones" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(doctors);

            // Act
            var result = await _service.GetAllDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateDoctorAsync_WithValidDoctor_ReturnsCreatedDoctor()
        {
            // Arrange
            var doctor = new Doctor { Name = "Dr. Smith", Email = "smith@test.com", Password = "password" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Doctor>())).ReturnsAsync(doctor);

            // Act
            var result = await _service.CreateDoctorAsync(doctor);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var doctor = new Doctor { DoctorID = 1, Name = "Updated Name" };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Doctor>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateDoctorAsync(doctor);

            // Assert
            Assert.NotNull(doctor.ModifiedDate);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteDoctorAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsByDepartmentAsync_ReturnsDoctorsInDepartment()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorID = 1, DeptNo = 5, Name = "Dr. Smith" }
            };
            _mockRepository.Setup(r => r.GetByDepartmentAsync(5)).ReturnsAsync(doctors);

            // Act
            var result = await _service.GetDoctorsByDepartmentAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetDoctorsBySpecializationAsync_ReturnsDoctorsWithSpecialization()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorID = 1, Specialization = "Cardiology" }
            };
            _mockRepository.Setup(r => r.GetBySpecializationAsync("Cardiology")).ReturnsAsync(doctors);

            // Act
            var result = await _service.GetDoctorsBySpecializationAsync("Cardiology");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task SearchDoctorsAsync_WithSearchTerm_ReturnsMatchingDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorID = 1, Name = "Dr. Smith" }
            };
            _mockRepository.Setup(r => r.SearchDoctorsAsync("Smith")).ReturnsAsync(doctors);

            // Act
            var result = await _service.SearchDoctorsAsync("Smith");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_WithValidCredentials_ReturnsDoctor()
        {
            // Arrange
            var doctor = new Doctor { DoctorID = 1, Email = "test@test.com" };
            _mockRepository.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(doctor);

            // Act
            var result = await _service.ValidateLoginAsync("test@test.com", "password");

            // Assert
            Assert.NotNull(result);
        }
    }
}
