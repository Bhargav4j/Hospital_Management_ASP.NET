namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for FreeSlot entity
/// </summary>
public class FreeSlotDto
{
    /// <summary>
    /// Gets or sets the free slot identifier
    /// </summary>
    public int FreeSlotID { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier
    /// </summary>
    public int DoctorID { get; set; }

    /// <summary>
    /// Gets or sets the start time
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modification date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the slot is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets who created the record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets who modified the record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the doctor name
    /// </summary>
    public string? DoctorName { get; set; }
}
