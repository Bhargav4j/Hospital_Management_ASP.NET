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
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _mockRepository;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _mockRepository = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WithValidId_ReturnsAppointment()
        {
            // Arrange
            var appointment = new Appointment { AppointmentID = 1, PatientID = 1, DoctorID = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            // Act
            var result = await _service.GetAppointmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentID);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Appointment?)null);

            // Act
            var result = await _service.GetAppointmentByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentID = 1 },
                new Appointment { AppointmentID = 2 }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(appointments);

            // Act
            var result = await _service.GetAllAppointmentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateAppointmentAsync_WithValidAppointment_ReturnsCreatedAppointment()
        {
            // Arrange
            var appointment = new Appointment { PatientID = 1, DoctorID = 1 };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);

            // Act
            var result = await _service.CreateAppointmentAsync(appointment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pending", result.Status);
            Assert.False(result.IsPaid);
            Assert.False(result.FeedbackGiven);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var appointment = new Appointment { AppointmentID = 1 };
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAppointmentAsync(appointment);

            // Assert
            Assert.NotNull(appointment.ModifiedDate);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_CallsRepositoryDelete()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAppointmentAsync(1);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPatientAppointmentsAsync_ReturnsAppointmentsForPatient()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentID = 1, PatientID = 1 }
            };
            _mockRepository.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(appointments);

            // Act
            var result = await _service.GetPatientAppointmentsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetDoctorAppointmentsAsync_ReturnsAppointmentsForDoctor()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentID = 1, DoctorID = 1 }
            };
            _mockRepository.Setup(r => r.GetByDoctorIdAsync(1)).ReturnsAsync(appointments);

            // Act
            var result = await _service.GetDoctorAppointmentsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task UpdateAppointmentStatusAsync_UpdatesStatusSuccessfully()
        {
            // Arrange
            var appointment = new Appointment { AppointmentID = 1, Status = "Pending" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAppointmentStatusAsync(1, "Confirmed");

            // Assert
            Assert.Equal("Confirmed", appointment.Status);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBillAsync_UpdatesBillAmountSuccessfully()
        {
            // Arrange
            var appointment = new Appointment { AppointmentID = 1, BillAmount = 0 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateBillAsync(1, 500.00m);

            // Assert
            Assert.Equal(500.00m, appointment.BillAmount);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task MarkAsPaidAsync_MarksAppointmentAsPaid()
        {
            // Arrange
            var appointment = new Appointment { AppointmentID = 1, IsPaid = false };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            // Act
            await _service.MarkAsPaidAsync(1);

            // Assert
            Assert.True(appointment.IsPaid);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
