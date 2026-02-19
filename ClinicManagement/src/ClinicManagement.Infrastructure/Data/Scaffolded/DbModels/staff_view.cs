using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Keyless]
public partial class staff_view
{
    public int? id { get; set; }

    [StringLength(30)]
    public string? name { get; set; }

    [StringLength(15)]
    public string? designation { get; set; }
}
