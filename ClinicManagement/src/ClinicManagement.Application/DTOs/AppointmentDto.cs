namespace ClinicManagement.Application.DTOs;

public class AppointmentDto
{
    public int AppointmentID { get; set; }
    public int PatientID { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorID { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Timings { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Disease { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public decimal BillAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool FeedbackGiven { get; set; }
}

public class CreateAppointmentDto
{
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Timings { get; set; } = string.Empty;
    public string Disease { get; set; } = string.Empty;
}

public class UpdateAppointmentDto
{
    public int AppointmentID { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public decimal BillAmount { get; set; }
}
