using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Keyless]
public partial class patient_view
{
    public int? id { get; set; }

    [StringLength(30)]
    public string? name { get; set; }

    [StringLength(11)]
    public string? phone { get; set; }
}
