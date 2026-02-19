using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("patient", Schema = "dbo")]
public partial class patient
{
    [Key]
    public int patientid { get; set; }

    [StringLength(30)]
    public string name { get; set; } = null!;

    [StringLength(11)]
    public string? phone { get; set; }

    [StringLength(40)]
    public string? address { get; set; }

    public DateOnly birthdate { get; set; }

    [MaxLength(1)]
    public char gender { get; set; }

    [InverseProperty("patient")]
    public virtual ICollection<appointment> appointments { get; set; } = new List<appointment>();

    [ForeignKey("patientid")]
    [InverseProperty("patient")]
    public virtual logintable patientNavigation { get; set; } = null!;
}
