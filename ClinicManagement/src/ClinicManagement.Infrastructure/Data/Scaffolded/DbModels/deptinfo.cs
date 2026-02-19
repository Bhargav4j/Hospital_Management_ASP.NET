using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Keyless]
public partial class deptinfo
{
    [StringLength(30)]
    public string? deptname { get; set; }

    [StringLength(1000)]
    public string? description { get; set; }

    [Column("Number of Doctors")]
    public long? Number_of_Doctors { get; set; }
}
