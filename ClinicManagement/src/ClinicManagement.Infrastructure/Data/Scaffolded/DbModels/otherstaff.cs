using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("otherstaff", Schema = "dbo")]
public partial class otherstaff
{
    [Key]
    public int staffid { get; set; }

    [StringLength(30)]
    public string name { get; set; } = null!;

    [StringLength(11)]
    public string? phone { get; set; }

    [StringLength(30)]
    public string? address { get; set; }

    [StringLength(15)]
    public string designation { get; set; } = null!;

    [MaxLength(1)]
    public char gender { get; set; }

    public DateOnly? birthdate { get; set; }

    [StringLength(50)]
    public string? highest_qualification { get; set; }

    public double? salary { get; set; }
}
