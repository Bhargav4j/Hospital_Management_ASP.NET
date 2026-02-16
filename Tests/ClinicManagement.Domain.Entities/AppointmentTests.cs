using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Tests.Entities
{
    public class AppointmentTests
    {
        [Fact]
        public void Appointment_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var appointment = new Appointment();

            // Assert
            Assert.Equal(0, appointment.AppointmentID);
            Assert.Equal(0, appointment.PatientID);
            Assert.Equal(0, appointment.DoctorID);
            Assert.Equal(string.Empty, appointment.Timings);
            Assert.Equal(string.Empty, appointment.Status);
            Assert.Equal(string.Empty, appointment.Disease);
            Assert.Equal(string.Empty, appointment.Progress);
            Assert.Equal(string.Empty, appointment.Prescription);
            Assert.Equal(0, appointment.BillAmount);
            Assert.False(appointment.IsPaid);
            Assert.False(appointment.FeedbackGiven);
        }

        [Fact]
        public void Appointment_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var appointment = new Appointment();
            var appointmentDate = new DateTime(2024, 12, 25, 10, 30, 0);

            // Act
            appointment.AppointmentID = 1;
            appointment.PatientID = 101;
            appointment.DoctorID = 201;
            appointment.AppointmentDate = appointmentDate;
            appointment.Timings = "10:30 AM";
            appointment.Status = "Confirmed";
            appointment.Disease = "Flu";
            appointment.Progress = "Improving";
            appointment.Prescription = "Medicine A, Medicine B";
            appointment.BillAmount = 500.50m;
            appointment.IsPaid = true;
            appointment.FeedbackGiven = true;

            // Assert
            Assert.Equal(1, appointment.AppointmentID);
            Assert.Equal(101, appointment.PatientID);
            Assert.Equal(201, appointment.DoctorID);
            Assert.Equal(appointmentDate, appointment.AppointmentDate);
            Assert.Equal("10:30 AM", appointment.Timings);
            Assert.Equal("Confirmed", appointment.Status);
            Assert.Equal("Flu", appointment.Disease);
            Assert.Equal("Improving", appointment.Progress);
            Assert.Equal("Medicine A, Medicine B", appointment.Prescription);
            Assert.Equal(500.50m, appointment.BillAmount);
            Assert.True(appointment.IsPaid);
            Assert.True(appointment.FeedbackGiven);
        }

        [Fact]
        public void Appointment_CreatedDate_ShouldBeSetAutomatically()
        {
            // Arrange & Act
            var before = DateTime.UtcNow.AddSeconds(-1);
            var appointment = new Appointment();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(appointment.CreatedDate >= before);
            Assert.True(appointment.CreatedDate <= after);
        }

        [Fact]
        public void Appointment_ModifiedDate_ShouldBeNullByDefault()
        {
            // Act
            var appointment = new Appointment();

            // Assert
            Assert.Null(appointment.ModifiedDate);
        }

        [Fact]
        public void Appointment_ModifiedDate_CanBeSet()
        {
            // Arrange
            var appointment = new Appointment();
            var modifiedDate = DateTime.UtcNow;

            // Act
            appointment.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, appointment.ModifiedDate);
        }

        [Fact]
        public void Appointment_Patient_CanBeSet()
        {
            // Arrange
            var appointment = new Appointment();
            var patient = new Patient { PatientID = 101, Name = "John Doe" };

            // Act
            appointment.Patient = patient;

            // Assert
            Assert.NotNull(appointment.Patient);
            Assert.Equal(patient, appointment.Patient);
        }

        [Fact]
        public void Appointment_Doctor_CanBeSet()
        {
            // Arrange
            var appointment = new Appointment();
            var doctor = new Doctor { DoctorID = 201, Name = "Dr. Smith" };

            // Act
            appointment.Doctor = doctor;

            // Assert
            Assert.NotNull(appointment.Doctor);
            Assert.Equal(doctor, appointment.Doctor);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100.00)]
        [InlineData(999.99)]
        [InlineData(10000.50)]
        public void Appointment_BillAmount_AcceptsDifferentValues(decimal amount)
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.BillAmount = amount;

            // Assert
            Assert.Equal(amount, appointment.BillAmount);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Appointment_IsPaid_CanBeSetToBothValues(bool isPaid)
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.IsPaid = isPaid;

            // Assert
            Assert.Equal(isPaid, appointment.IsPaid);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Appointment_FeedbackGiven_CanBeSetToBothValues(bool feedbackGiven)
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.FeedbackGiven = feedbackGiven;

            // Assert
            Assert.Equal(feedbackGiven, appointment.FeedbackGiven);
        }

        [Theory]
        [InlineData("Pending")]
        [InlineData("Confirmed")]
        [InlineData("Completed")]
        [InlineData("Cancelled")]
        public void Appointment_Status_AcceptsDifferentValues(string status)
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.Status = status;

            // Assert
            Assert.Equal(status, appointment.Status);
        }
    }
}
