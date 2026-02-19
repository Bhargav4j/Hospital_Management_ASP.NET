using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("doctor", Schema = "dbo")]
public partial class doctor
{
    [Key]
    public int doctorid { get; set; }

    [StringLength(30)]
    public string name { get; set; } = null!;

    [StringLength(11)]
    public string? phone { get; set; }

    [StringLength(40)]
    public string? address { get; set; }

    public DateOnly birthdate { get; set; }

    [MaxLength(1)]
    public char gender { get; set; }

    public int deptno { get; set; }

    public double charges_per_visit { get; set; }

    public double? monthlysalary { get; set; }

    public double? reputeindex { get; set; }

    public int patients_treated { get; set; }

    [StringLength(100)]
    public string qualification { get; set; } = null!;

    [StringLength(100)]
    public string? specialization { get; set; }

    public int? work_experience { get; set; }

    public int status { get; set; }

    [InverseProperty("doctor")]
    public virtual ICollection<appointment> appointments { get; set; } = new List<appointment>();

    [ForeignKey("deptno")]
    [InverseProperty("doctors")]
    public virtual department deptnoNavigation { get; set; } = null!;
}
