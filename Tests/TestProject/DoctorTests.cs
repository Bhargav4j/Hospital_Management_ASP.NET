using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Domain.Tests.Entities
{
    public class DoctorTests
    {
        [Fact]
        public void Doctor_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var doctor = new Doctor();

            // Assert
            Assert.Equal(0, doctor.DoctorID);
            Assert.Equal(string.Empty, doctor.Name);
            Assert.Equal(string.Empty, doctor.Email);
            Assert.Equal(string.Empty, doctor.Password);
            Assert.Equal(string.Empty, doctor.Phone);
            Assert.Equal(string.Empty, doctor.Address);
            Assert.Equal(string.Empty, doctor.Gender);
            Assert.Equal(0, doctor.DeptNo);
            Assert.Equal(0, doctor.Experience);
            Assert.Equal(0, doctor.Salary);
            Assert.Equal(0, doctor.ChargesPerVisit);
            Assert.Equal(string.Empty, doctor.Specialization);
            Assert.Equal(string.Empty, doctor.Qualification);
            Assert.NotNull(doctor.Appointments);
            Assert.Empty(doctor.Appointments);
        }

        [Fact]
        public void Doctor_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var doctor = new Doctor();
            var testDate = new DateTime(1980, 3, 20);

            // Act
            doctor.DoctorID = 1;
            doctor.Name = "Dr. Smith";
            doctor.Email = "smith@hospital.com";
            doctor.Password = "securepass";
            doctor.Phone = "9876543210";
            doctor.Address = "456 Medical Ave";
            doctor.BirthDate = testDate;
            doctor.Gender = "Female";
            doctor.DeptNo = 5;
            doctor.Experience = 10;
            doctor.Salary = 75000.00m;
            doctor.ChargesPerVisit = 500.00m;
            doctor.Specialization = "Cardiology";
            doctor.Qualification = "MD";

            // Assert
            Assert.Equal(1, doctor.DoctorID);
            Assert.Equal("Dr. Smith", doctor.Name);
            Assert.Equal("smith@hospital.com", doctor.Email);
            Assert.Equal("securepass", doctor.Password);
            Assert.Equal("9876543210", doctor.Phone);
            Assert.Equal("456 Medical Ave", doctor.Address);
            Assert.Equal(testDate, doctor.BirthDate);
            Assert.Equal("Female", doctor.Gender);
            Assert.Equal(5, doctor.DeptNo);
            Assert.Equal(10, doctor.Experience);
            Assert.Equal(75000.00m, doctor.Salary);
            Assert.Equal(500.00m, doctor.ChargesPerVisit);
            Assert.Equal("Cardiology", doctor.Specialization);
            Assert.Equal("MD", doctor.Qualification);
        }

        [Fact]
        public void Doctor_CreatedDate_ShouldBeSetAutomatically()
        {
            // Arrange & Act
            var before = DateTime.UtcNow.AddSeconds(-1);
            var doctor = new Doctor();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(doctor.CreatedDate >= before);
            Assert.True(doctor.CreatedDate <= after);
        }

        [Fact]
        public void Doctor_ModifiedDate_ShouldBeNullByDefault()
        {
            // Act
            var doctor = new Doctor();

            // Assert
            Assert.Null(doctor.ModifiedDate);
        }

        [Fact]
        public void Doctor_ModifiedDate_CanBeSet()
        {
            // Arrange
            var doctor = new Doctor();
            var modifiedDate = DateTime.UtcNow;

            // Act
            doctor.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, doctor.ModifiedDate);
        }

        [Fact]
        public void Doctor_Appointments_CanAddAppointment()
        {
            // Arrange
            var doctor = new Doctor();
            var appointment = new Appointment { AppointmentID = 1 };

            // Act
            doctor.Appointments.Add(appointment);

            // Assert
            Assert.Single(doctor.Appointments);
            Assert.Contains(appointment, doctor.Appointments);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(20)]
        [InlineData(40)]
        public void Doctor_Experience_AcceptsDifferentValues(int experience)
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.Experience = experience;

            // Assert
            Assert.Equal(experience, doctor.Experience);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50000.50)]
        [InlineData(100000.99)]
        public void Doctor_Salary_AcceptsDifferentValues(decimal salary)
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.Salary = salary;

            // Assert
            Assert.Equal(salary, doctor.Salary);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(250.75)]
        [InlineData(1000.00)]
        public void Doctor_ChargesPerVisit_AcceptsDifferentValues(decimal charges)
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.ChargesPerVisit = charges;

            // Assert
            Assert.Equal(charges, doctor.ChargesPerVisit);
        }

        [Fact]
        public void Doctor_Department_CanBeSet()
        {
            // Arrange
            var doctor = new Doctor();
            var department = new Department { DeptNo = 1, DeptName = "Cardiology" };

            // Act
            doctor.Department = department;

            // Assert
            Assert.NotNull(doctor.Department);
            Assert.Equal(department, doctor.Department);
        }
    }
}
