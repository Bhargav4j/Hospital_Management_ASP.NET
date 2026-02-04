using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class
    /// </summary>
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>();
        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.PatientID, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<PatientUpdateDto, Patient>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : null));

        CreateMap<DoctorCreateDto, Doctor>()
            .ForMember(dest => dest.DoctorID, opt => opt.Ignore())
            .ForMember(dest => dest.ReputationIndex, opt => opt.MapFrom(src => 0.0f))
            .ForMember(dest => dest.PatientsTreated, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<DoctorUpdateDto, Doctor>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.ReputationIndex, opt => opt.Ignore())
            .ForMember(dest => dest.PatientsTreated, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : null))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : null));

        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.AppointmentID, opt => opt.Ignore())
            .ForMember(dest => dest.Progress, opt => opt.Ignore())
            .ForMember(dest => dest.Prescription, opt => opt.Ignore())
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.FeedbackGiven, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore())
            .ForMember(dest => dest.FreeSlot, opt => opt.Ignore());

        CreateMap<AppointmentUpdateDto, Appointment>()
            .ForMember(dest => dest.PatientID, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorID, opt => opt.Ignore())
            .ForMember(dest => dest.FreeSlotID, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore())
            .ForMember(dest => dest.FreeSlot, opt => opt.Ignore());

        // Department mappings
        CreateMap<Department, DepartmentDto>();

        // OtherStaff mappings
        CreateMap<OtherStaff, OtherStaffDto>();

        // FreeSlot mappings
        CreateMap<FreeSlot, FreeSlotDto>()
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : null));
    }
}
