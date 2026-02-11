using AutoMapper;
using HospitalManagement.Application.Mappings;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using Xunit;

namespace Tests.HospitalManagement.Application.Mappings;

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
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Arrange
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Act & Assert
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_User_To_UserDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            BirthDate = new DateTime(1990, 1, 1),
            Email = "test@test.com",
            PhoneNo = "1234567890",
            Gender = Gender.Male,
            Address = "123 Main St",
            UserType = UserType.Patient
        };

        // Act
        var userDto = _mapper.Map<UserDto>(user);

        // Assert
        Assert.Equal(user.Id, userDto.Id);
        Assert.Equal(user.Name, userDto.Name);
        Assert.Equal(user.Email, userDto.Email);
        Assert.Equal(user.Gender, userDto.Gender);
        Assert.Equal(user.UserType, userDto.UserType);
    }

    [Fact]
    public void Map_UserCreateDto_To_User_ShouldMapCorrectly()
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Name = "New User",
            BirthDate = new DateTime(1990, 1, 1),
            Email = "new@test.com",
            Password = "password",
            PhoneNo = "1234567890",
            Gender = Gender.Female,
            Address = "456 Oak Ave",
            UserType = UserType.Doctor
        };

        // Act
        var user = _mapper.Map<User>(userCreateDto);

        // Assert
        Assert.Equal(userCreateDto.Name, user.Name);
        Assert.Equal(userCreateDto.Email, user.Email);
        Assert.Equal(userCreateDto.Password, user.Password);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Map_Appointment_To_AppointmentDto_ShouldMapCorrectly()
    {
        // Arrange
        var appointment = new Appointment
        {
            Id = 1,
            PatientId = 10,
            DoctorId = 20,
            Patient = new User { Name = "Patient Name" },
            Doctor = new User { Name = "Doctor Name" },
            AppointmentDate = DateTime.UtcNow,
            Status = "Confirmed"
        };

        // Act
        var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

        // Assert
        Assert.Equal(appointment.Id, appointmentDto.Id);
        Assert.Equal(appointment.PatientId, appointmentDto.PatientId);
        Assert.Equal(appointment.DoctorId, appointmentDto.DoctorId);
        Assert.Equal("Patient Name", appointmentDto.PatientName);
        Assert.Equal("Doctor Name", appointmentDto.DoctorName);
    }

    [Fact]
    public void Map_Bill_To_BillDto_ShouldMapCorrectly()
    {
        // Arrange
        var bill = new Bill
        {
            Id = 1,
            PatientId = 10,
            Patient = new User { Name = "Patient Name" },
            Amount = 150.00m,
            BillDate = DateTime.UtcNow,
            Description = "Consultation",
            IsPaid = true
        };

        // Act
        var billDto = _mapper.Map<BillDto>(bill);

        // Assert
        Assert.Equal(bill.Id, billDto.Id);
        Assert.Equal(bill.PatientId, billDto.PatientId);
        Assert.Equal("Patient Name", billDto.PatientName);
        Assert.Equal(bill.Amount, billDto.Amount);
        Assert.Equal(bill.IsPaid, billDto.IsPaid);
    }

    [Fact]
    public void Map_Feedback_To_FeedbackDto_ShouldMapCorrectly()
    {
        // Arrange
        var feedback = new Feedback
        {
            Id = 1,
            PatientId = 10,
            Patient = new User { Name = "Patient Name" },
            Message = "Great service!",
            Rating = 5,
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var feedbackDto = _mapper.Map<FeedbackDto>(feedback);

        // Assert
        Assert.Equal(feedback.Id, feedbackDto.Id);
        Assert.Equal(feedback.PatientId, feedbackDto.PatientId);
        Assert.Equal("Patient Name", feedbackDto.PatientName);
        Assert.Equal(feedback.Message, feedbackDto.Message);
        Assert.Equal(feedback.Rating, feedbackDto.Rating);
    }

    [Fact]
    public void Map_MedicalHistory_To_MedicalHistoryDto_ShouldMapCorrectly()
    {
        // Arrange
        var medicalHistory = new MedicalHistory
        {
            Id = 1,
            PatientId = 10,
            Patient = new User { Name = "Patient Name" },
            Diagnosis = "Flu",
            Treatment = "Rest",
            VisitDate = DateTime.UtcNow,
            Notes = "Follow-up needed"
        };

        // Act
        var medicalHistoryDto = _mapper.Map<MedicalHistoryDto>(medicalHistory);

        // Assert
        Assert.Equal(medicalHistory.Id, medicalHistoryDto.Id);
        Assert.Equal(medicalHistory.PatientId, medicalHistoryDto.PatientId);
        Assert.Equal("Patient Name", medicalHistoryDto.PatientName);
        Assert.Equal(medicalHistory.Diagnosis, medicalHistoryDto.Diagnosis);
        Assert.Equal(medicalHistory.Treatment, medicalHistoryDto.Treatment);
    }

    [Fact]
    public void Map_Clinic_To_ClinicDto_ShouldMapCorrectly()
    {
        // Arrange
        var clinic = new Clinic
        {
            Id = 1,
            Name = "City Clinic",
            Address = "123 Medical St",
            PhoneNo = "555-1234",
            Description = "General clinic"
        };

        // Act
        var clinicDto = _mapper.Map<ClinicDto>(clinic);

        // Assert
        Assert.Equal(clinic.Id, clinicDto.Id);
        Assert.Equal(clinic.Name, clinicDto.Name);
        Assert.Equal(clinic.Address, clinicDto.Address);
        Assert.Equal(clinic.PhoneNo, clinicDto.PhoneNo);
        Assert.Equal(clinic.Description, clinicDto.Description);
    }
}
