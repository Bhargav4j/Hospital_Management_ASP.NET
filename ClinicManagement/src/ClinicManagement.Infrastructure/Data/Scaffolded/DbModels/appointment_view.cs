using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Keyless]
public partial class appointment_view
{
    public int? appointid { get; set; }

    [StringLength(30)]
    public string? name { get; set; }

    public int? patientid { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? date { get; set; }
}
