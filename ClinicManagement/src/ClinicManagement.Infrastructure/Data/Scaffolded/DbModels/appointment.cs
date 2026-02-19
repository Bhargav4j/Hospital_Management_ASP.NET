using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("appointment", Schema = "dbo")]
public partial class appointment
{
    [Key]
    public int appointid { get; set; }

    public int? doctorid { get; set; }

    public int? patientid { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? date { get; set; }

    public int? appointment_status { get; set; }

    public double? bill_amount { get; set; }

    [StringLength(10)]
    public string? bill_status { get; set; }

    public int? doctornotification { get; set; }

    public int? patientnotification { get; set; }

    public int? feedbackstatus { get; set; }

    [StringLength(100)]
    public string? disease { get; set; }

    [StringLength(100)]
    public string? progress { get; set; }

    [StringLength(100)]
    public string? prescription { get; set; }

    [ForeignKey("doctorid")]
    [InverseProperty("appointments")]
    public virtual doctor? doctor { get; set; }

    [ForeignKey("patientid")]
    [InverseProperty("appointments")]
    public virtual patient? patient { get; set; }
}
