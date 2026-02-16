using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>();
        CreateMap<CreatePatientDto, Patient>();
        CreateMap<UpdatePatientDto, Patient>();

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DeptName));
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>();

        // Department mappings
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name));
        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.FeedbackGiven, opt => opt.MapFrom(src => false));
        CreateMap<UpdateAppointmentDto, Appointment>();

        // OtherStaff mappings
        CreateMap<OtherStaff, OtherStaffDto>();
        CreateMap<CreateOtherStaffDto, OtherStaff>();
        CreateMap<UpdateOtherStaffDto, OtherStaff>();
    }
}
