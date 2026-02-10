using AutoMapper;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>();
        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<PatientUpdateDto, Patient>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>();
        CreateMap<DoctorCreateDto, Doctor>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<DoctorUpdateDto, Doctor>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.AppointmentStatus.Pending))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<AppointmentUpdateDto, Appointment>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Bill mappings
        CreateMap<Bill, BillDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty));
        CreateMap<BillCreateDto, Bill>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<BillUpdateDto, Bill>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // TreatmentHistory mappings
        CreateMap<TreatmentHistory, TreatmentHistoryDto>();
        CreateMap<TreatmentHistoryCreateDto, TreatmentHistory>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<TreatmentHistoryUpdateDto, TreatmentHistory>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Feedback mappings
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty));
        CreateMap<FeedbackCreateDto, Feedback>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<FeedbackUpdateDto, Feedback>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Notification mappings
        CreateMap<Notification, NotificationDto>();
        CreateMap<NotificationCreateDto, Notification>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<NotificationUpdateDto, Notification>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Staff mappings
        CreateMap<Staff, StaffDto>();
        CreateMap<StaffCreateDto, Staff>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<StaffUpdateDto, Staff>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));
    }
}
