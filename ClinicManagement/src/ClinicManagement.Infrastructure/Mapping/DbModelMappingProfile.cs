using AutoMapper;
using ClinicManagement.Domain.Entities;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

namespace ClinicManagement.Infrastructure.Mappings
{
    public class DbModelMappingProfile : Profile
    {
        public DbModelMappingProfile()
        {
            // ============================================================
            // PATIENT MAPPINGS
            // Domain Patient <-> DbModel patient
            // Type diffs: BirthDate (DateTime vs DateOnly), Gender (string vs char)
            // Email/Password live in logintable, accessed via patientNavigation
            // ============================================================
            CreateMap<Patient, DbModels.patient>()
                .ForMember(dest => dest.birthdate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)))
                .ForMember(dest => dest.gender, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Gender) ? src.Gender[0] : 'U'))
                .ForMember(dest => dest.appointments, opt => opt.Ignore())
                .ForMember(dest => dest.patientNavigation, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.birthdate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender.ToString()))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone ?? string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address ?? string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                    src.patientNavigation != null ? src.patientNavigation.email : string.Empty))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src =>
                    src.patientNavigation != null ? src.patientNavigation.password : string.Empty))
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // ============================================================
            // DOCTOR MAPPINGS
            // Domain Doctor <-> DbModel doctor
            // Name diffs: Salary->monthlysalary, Experience->work_experience,
            //   ChargesPerVisit->charges_per_visit
            // Type diffs: BirthDate (DateTime vs DateOnly), Gender (string vs char),
            //   Salary/ChargesPerVisit (decimal vs double)
            // DbModel-only fields: reputeindex, patients_treated, status
            // ============================================================
            CreateMap<Doctor, DbModels.doctor>()
                .ForMember(dest => dest.birthdate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)))
                .ForMember(dest => dest.gender, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Gender) ? src.Gender[0] : 'U'))
                .ForMember(dest => dest.charges_per_visit, opt => opt.MapFrom(src => (double)src.ChargesPerVisit))
                .ForMember(dest => dest.monthlysalary, opt => opt.MapFrom(src => (double)src.Salary))
                .ForMember(dest => dest.work_experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.reputeindex, opt => opt.Ignore())
                .ForMember(dest => dest.patients_treated, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.Ignore())
                .ForMember(dest => dest.appointments, opt => opt.Ignore())
                .ForMember(dest => dest.deptnoNavigation, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.birthdate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender.ToString()))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone ?? string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address ?? string.Empty))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.work_experience ?? 0))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => (decimal)(src.monthlysalary ?? 0)))
                .ForMember(dest => dest.ChargesPerVisit, opt => opt.MapFrom(src => (decimal)src.charges_per_visit))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.specialization ?? string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.deptnoNavigation))
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // ============================================================
            // DEPARTMENT MAPPINGS
            // Domain Department <-> DbModel department
            // All names match case-insensitively (DeptNo/deptno, DeptName/deptname)
            // ============================================================
            CreateMap<Department, DbModels.department>()
                .ForMember(dest => dest.doctors, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description ?? string.Empty))
                .ForMember(dest => dest.Doctors, opt => opt.Ignore());

            // ============================================================
            // APPOINTMENT MAPPINGS
            // Domain Appointment <-> DbModel appointment
            // Name diffs: AppointmentID->appointid, AppointmentDate->date,
            //   Status->appointment_status, BillAmount->bill_amount,
            //   IsPaid->bill_status, FeedbackGiven->feedbackstatus
            // ============================================================
            CreateMap<Appointment, DbModels.appointment>()
                .ForMember(dest => dest.appointid, opt => opt.MapFrom(src => src.AppointmentID))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.appointment_status, opt => opt.MapFrom(src => MapStatusToInt(src.Status)))
                .ForMember(dest => dest.bill_amount, opt => opt.MapFrom(src => (double)src.BillAmount))
                .ForMember(dest => dest.bill_status, opt => opt.MapFrom(src => src.IsPaid ? "Paid" : "Unpaid"))
                .ForMember(dest => dest.feedbackstatus, opt => opt.MapFrom(src => src.FeedbackGiven ? 1 : 0))
                .ForMember(dest => dest.doctornotification, opt => opt.Ignore())
                .ForMember(dest => dest.patientnotification, opt => opt.Ignore())
                .ForMember(dest => dest.doctor, opt => opt.Ignore())
                .ForMember(dest => dest.patient, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.AppointmentID, opt => opt.MapFrom(src => src.appointid))
                .ForMember(dest => dest.PatientID, opt => opt.MapFrom(src => src.patientid ?? 0))
                .ForMember(dest => dest.DoctorID, opt => opt.MapFrom(src => src.doctorid ?? 0))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.date ?? DateTime.MinValue))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapIntToStatus(src.appointment_status)))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.disease ?? string.Empty))
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => src.progress ?? string.Empty))
                .ForMember(dest => dest.Prescription, opt => opt.MapFrom(src => src.prescription ?? string.Empty))
                .ForMember(dest => dest.BillAmount, opt => opt.MapFrom(src => (decimal)(src.bill_amount ?? 0)))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src =>
                    src.bill_status != null && src.bill_status.Equals("Paid", StringComparison.OrdinalIgnoreCase)))
                .ForMember(dest => dest.FeedbackGiven, opt => opt.MapFrom(src => src.feedbackstatus == 1))
                .ForMember(dest => dest.Timings, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.patient))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.doctor));

            // ============================================================
            // OTHERSTAFF MAPPINGS
            // Domain OtherStaff <-> DbModel otherstaff
            // Name diffs: Qualification->highest_qualification
            // Type diffs: BirthDate (DateTime vs DateOnly?), Gender (string vs char),
            //   Salary (decimal vs double?)
            // ============================================================
            CreateMap<OtherStaff, DbModels.otherstaff>()
                .ForMember(dest => dest.birthdate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)))
                .ForMember(dest => dest.gender, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Gender) ? src.Gender[0] : 'U'))
                .ForMember(dest => dest.salary, opt => opt.MapFrom(src => (double)src.Salary))
                .ForMember(dest => dest.highest_qualification, opt => opt.MapFrom(src => src.Qualification))
                .ReverseMap()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone ?? string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address ?? string.Empty))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender.ToString()))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.birthdate.HasValue ? src.birthdate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => (decimal)(src.salary ?? 0)))
                .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.highest_qualification ?? string.Empty));
        }

        private static int? MapStatusToInt(string status)
        {
            return status?.ToLower() switch
            {
                "pending" => 0,
                "approved" => 1,
                "completed" => 2,
                "canceled" or "cancelled" => 3,
                _ => 0
            };
        }

        private static string MapIntToStatus(int? status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Approved",
                2 => "Completed",
                3 => "Canceled",
                _ => "Pending"
            };
        }
    }
}
