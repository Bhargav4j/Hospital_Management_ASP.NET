using AutoMapper;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name));
        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));
        CreateMap<AppointmentUpdateDto, Appointment>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Bill, BillDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name));
        CreateMap<BillCreateDto, Bill>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.BillDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false));
        CreateMap<BillUpdateDto, Bill>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Feedback, FeedbackDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name));
        CreateMap<FeedbackCreateDto, Feedback>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<FeedbackUpdateDto, Feedback>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<MedicalHistory, MedicalHistoryDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name));
        CreateMap<MedicalHistoryCreateDto, MedicalHistory>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<MedicalHistoryUpdateDto, MedicalHistory>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Clinic, ClinicDto>();
        CreateMap<ClinicCreateDto, Clinic>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<ClinicUpdateDto, Clinic>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
