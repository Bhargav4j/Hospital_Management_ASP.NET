using System;
using Xunit;
using AutoMapper;
using ClinicManagement.Application.Mappings;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Application.Mappings.Tests
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MappingProfile_Configuration_CanCreateMapper()
        {
            // Arrange & Act
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();

            // Assert
            Assert.NotNull(mapper);
        }

        [Fact]
        public void MappingProfile_MapPatientToPatientDto_MapsCorrectly()
        {
            // Arrange
            var patient = new Patient
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "1234567890",
                BirthDate = new DateTime(1990, 1, 1),
                Gender = "Male",
                Address = "123 Main St",
                IsActive = true
            };

            // Act
            var dto = _mapper.Map<PatientDto>(patient);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(patient.Id, dto.Id);
            Assert.Equal(patient.Name, dto.Name);
            Assert.Equal(patient.Email, dto.Email);
            Assert.Equal(patient.Phone, dto.Phone);
            Assert.Equal(patient.BirthDate, dto.BirthDate);
            Assert.Equal(patient.Gender, dto.Gender);
            Assert.Equal(patient.Address, dto.Address);
            Assert.Equal(patient.IsActive, dto.IsActive);
        }

        [Fact]
        public void MappingProfile_MapPatientCreateDtoToPatient_SetsDefaults()
        {
            // Arrange
            var createDto = new PatientCreateDto
            {
                Name = "New Patient",
                Email = "new@test.com",
                Password = "pass123",
                Phone = "9876543210",
                BirthDate = new DateTime(1995, 5, 15),
                Gender = "Female",
                Address = "456 Oak Ave"
            };

            // Act
            var patient = _mapper.Map<Patient>(createDto);

            // Assert
            Assert.NotNull(patient);
            Assert.Equal(createDto.Name, patient.Name);
            Assert.Equal(createDto.Email, patient.Email);
            Assert.Equal(createDto.Password, patient.Password);
            Assert.True(patient.IsActive);
            Assert.Equal("System", patient.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapDoctorToDoctorDto_MapsCorrectly()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 10,
                Name = "Dr. Smith",
                Email = "smith@clinic.com",
                Phone = "5551234567",
                Specialization = "Cardiology",
                Qualification = "MBBS, MD",
                Experience = 15,
                ConsultationFee = 500.00m,
                IsActive = true
            };

            // Act
            var dto = _mapper.Map<DoctorDto>(doctor);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(doctor.Id, dto.Id);
            Assert.Equal(doctor.Name, dto.Name);
            Assert.Equal(doctor.Email, dto.Email);
            Assert.Equal(doctor.Specialization, dto.Specialization);
            Assert.Equal(doctor.ConsultationFee, dto.ConsultationFee);
            Assert.Equal(doctor.IsActive, dto.IsActive);
        }

        [Fact]
        public void MappingProfile_MapDoctorCreateDtoToDoctor_SetsDefaults()
        {
            // Arrange
            var createDto = new DoctorCreateDto
            {
                Name = "Dr. New",
                Email = "new@hospital.com",
                Password = "docpass",
                Phone = "7778889999",
                Specialization = "Neurology",
                Qualification = "MBBS, DM",
                Experience = 10,
                ConsultationFee = 800.00m
            };

            // Act
            var doctor = _mapper.Map<Doctor>(createDto);

            // Assert
            Assert.NotNull(doctor);
            Assert.Equal(createDto.Name, doctor.Name);
            Assert.True(doctor.IsActive);
            Assert.Equal("System", doctor.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapAppointmentCreateDtoToAppointment_SetsDefaults()
        {
            // Arrange
            var createDto = new AppointmentCreateDto
            {
                PatientId = 100,
                DoctorId = 50,
                AppointmentDate = new DateTime(2026, 3, 15),
                TimeSlot = "10:00 AM - 10:30 AM",
                Reason = "Checkup"
            };

            // Act
            var appointment = _mapper.Map<Appointment>(createDto);

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal(createDto.PatientId, appointment.PatientId);
            Assert.Equal(createDto.DoctorId, appointment.DoctorId);
            Assert.Equal("Pending", appointment.Status);
            Assert.True(appointment.IsActive);
            Assert.Equal("System", appointment.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapBillCreateDtoToBill_SetsDefaults()
        {
            // Arrange
            var createDto = new BillCreateDto
            {
                PatientId = 200,
                DoctorId = 100,
                Amount = 1500.00m,
                Description = "Consultation"
            };

            // Act
            var bill = _mapper.Map<Bill>(createDto);

            // Assert
            Assert.NotNull(bill);
            Assert.Equal(createDto.PatientId, bill.PatientId);
            Assert.Equal(createDto.Amount, bill.Amount);
            Assert.Equal("Unpaid", bill.Status);
            Assert.True(bill.IsActive);
            Assert.Equal("System", bill.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapTreatmentHistoryCreateDtoToTreatmentHistory_SetsDefaults()
        {
            // Arrange
            var createDto = new TreatmentHistoryCreateDto
            {
                PatientId = 300,
                DoctorId = 150,
                Diagnosis = "Flu",
                Treatment = "Rest and medication",
                Prescription = "Paracetamol",
                Notes = "Follow-up in 7 days"
            };

            // Act
            var treatment = _mapper.Map<TreatmentHistory>(createDto);

            // Assert
            Assert.NotNull(treatment);
            Assert.Equal(createDto.PatientId, treatment.PatientId);
            Assert.Equal(createDto.Diagnosis, treatment.Diagnosis);
            Assert.True(treatment.IsActive);
            Assert.Equal("System", treatment.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapFeedbackCreateDtoToFeedback_SetsDefaults()
        {
            // Arrange
            var createDto = new FeedbackCreateDto
            {
                PatientId = 400,
                DoctorId = 200,
                Subject = "Great service",
                Message = "Very satisfied",
                Rating = 5
            };

            // Act
            var feedback = _mapper.Map<Feedback>(createDto);

            // Assert
            Assert.NotNull(feedback);
            Assert.Equal(createDto.PatientId, feedback.PatientId);
            Assert.Equal(createDto.Subject, feedback.Subject);
            Assert.Equal(createDto.Rating, feedback.Rating);
            Assert.True(feedback.IsActive);
            Assert.Equal("System", feedback.CreatedBy);
        }

        [Fact]
        public void MappingProfile_MapStaffToStaffDto_MapsCorrectly()
        {
            // Arrange
            var staff = new Staff
            {
                Id = 500,
                Name = "Alice Staff",
                Email = "alice@clinic.com",
                Phone = "6665554444",
                Role = "Nurse",
                Department = "Emergency",
                IsActive = true
            };

            // Act
            var dto = _mapper.Map<StaffDto>(staff);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(staff.Id, dto.Id);
            Assert.Equal(staff.Name, dto.Name);
            Assert.Equal(staff.Email, dto.Email);
            Assert.Equal(staff.Role, dto.Role);
            Assert.Equal(staff.IsActive, dto.IsActive);
        }

        [Fact]
        public void MappingProfile_MapStaffCreateDtoToStaff_SetsDefaults()
        {
            // Arrange
            var createDto = new StaffCreateDto
            {
                Name = "Bob Staff",
                Email = "bob@hospital.com",
                Password = "staffpass",
                Phone = "5554443333",
                Role = "Receptionist",
                Department = "General"
            };

            // Act
            var staff = _mapper.Map<Staff>(createDto);

            // Assert
            Assert.NotNull(staff);
            Assert.Equal(createDto.Name, staff.Name);
            Assert.True(staff.IsActive);
            Assert.Equal("System", staff.CreatedBy);
        }
    }
}
