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
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepository;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WithValidId_ReturnsPatient()
        {
            // Arrange
            var patient = new Patient { PatientID = 1, Name = "John Doe" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);

            // Act
            var result = await _service.GetPatientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatientID);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.GetPatientByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsAllPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { PatientID = 1, Name = "John" },
                new Patient { PatientID = 2, Name = "Jane" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreatePatientAsync_WithValidPatient_ReturnsCreatedPatient()
        {
            // Arrange
            var patient = new Patient { Name = "John Doe", Email = "john@test.com", Password = "password" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Patient>())).ReturnsAsync(patient);

            // Act
            var result = await _service.CreatePatientAsync(patient);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var patient = new Patient { PatientID = 1, Name = "Updated Name" };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdatePatientAsync(patient);

            // Assert
            Assert.NotNull(patient.ModifiedDate);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeletePatientAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task SearchPatientsAsync_WithSearchTerm_ReturnsMatchingPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { PatientID = 1, Name = "John Doe" }
            };
            _mockRepository.Setup(r => r.SearchPatientsAsync("John")).ReturnsAsync(patients);

            // Act
            var result = await _service.SearchPatientsAsync("John");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_WithValidCredentials_ReturnsPatient()
        {
            // Arrange
            var patient = new Patient { PatientID = 1, Email = "test@test.com" };
            _mockRepository.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.ValidateLoginAsync("test@test.com", "password");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.ValidateLoginAsync("wrong@test.com", "wrong");

            // Assert
            Assert.Null(result);
        }
    }
}
