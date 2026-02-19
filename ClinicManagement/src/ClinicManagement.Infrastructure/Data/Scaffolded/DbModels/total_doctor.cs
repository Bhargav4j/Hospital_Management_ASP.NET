using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Keyless]
public partial class total_doctor
{
    [Column("Total Doctors")]
    public long? Total_Doctors { get; set; }
}
