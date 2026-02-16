using Xunit;
using AutoMapper;
using ClinicManagement.Application.Mappings;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Application.Tests.Mappings
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_Configuration_ShouldBeValid()
        {
            // Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Patient_To_PatientDto_ShouldMap()
        {
            // Arrange
            var patient = new Patient
            {
                PatientID = 1,
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "1234567890"
            };

            // Act
            var dto = _mapper.Map<PatientDto>(patient);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(patient.PatientID, dto.PatientID);
            Assert.Equal(patient.Name, dto.Name);
            Assert.Equal(patient.Email, dto.Email);
        }

        [Fact]
        public void CreatePatientDto_To_Patient_ShouldMap()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                Name = "Jane Doe",
                Email = "jane@test.com",
                Password = "password"
            };

            // Act
            var patient = _mapper.Map<Patient>(dto);

            // Assert
            Assert.NotNull(patient);
            Assert.Equal(dto.Name, patient.Name);
            Assert.Equal(dto.Email, patient.Email);
            Assert.Equal(dto.Password, patient.Password);
        }

        [Fact]
        public void Doctor_To_DoctorDto_ShouldMap()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorID = 1,
                Name = "Dr. Smith",
                Email = "smith@test.com",
                Department = new Department { DeptName = "Cardiology" }
            };

            // Act
            var dto = _mapper.Map<DoctorDto>(doctor);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(doctor.DoctorID, dto.DoctorID);
            Assert.Equal(doctor.Name, dto.Name);
            Assert.Equal("Cardiology", dto.DepartmentName);
        }

        [Fact]
        public void Department_To_DepartmentDto_ShouldMap()
        {
            // Arrange
            var department = new Department
            {
                DeptNo = 1,
                DeptName = "Cardiology",
                Description = "Heart care"
            };

            // Act
            var dto = _mapper.Map<DepartmentDto>(department);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(department.DeptNo, dto.DeptNo);
            Assert.Equal(department.DeptName, dto.DeptName);
            Assert.Equal(department.Description, dto.Description);
        }

        [Fact]
        public void Appointment_To_AppointmentDto_ShouldMap()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentID = 1,
                PatientID = 1,
                DoctorID = 1,
                Patient = new Patient { Name = "John Doe" },
                Doctor = new Doctor { Name = "Dr. Smith" }
            };

            // Act
            var dto = _mapper.Map<AppointmentDto>(appointment);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(appointment.AppointmentID, dto.AppointmentID);
            Assert.Equal("John Doe", dto.PatientName);
            Assert.Equal("Dr. Smith", dto.DoctorName);
        }

        [Fact]
        public void CreateAppointmentDto_To_Appointment_ShouldMapWithDefaults()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientID = 1,
                DoctorID = 1,
                Disease = "Flu"
            };

            // Act
            var appointment = _mapper.Map<Appointment>(dto);

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal("Pending", appointment.Status);
            Assert.False(appointment.IsPaid);
            Assert.False(appointment.FeedbackGiven);
        }

        [Fact]
        public void OtherStaff_To_OtherStaffDto_ShouldMap()
        {
            // Arrange
            var staff = new OtherStaff
            {
                StaffID = 1,
                Name = "Jane Nurse",
                Designation = "Nurse"
            };

            // Act
            var dto = _mapper.Map<OtherStaffDto>(staff);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(staff.StaffID, dto.StaffID);
            Assert.Equal(staff.Name, dto.Name);
            Assert.Equal(staff.Designation, dto.Designation);
        }
    }
}
