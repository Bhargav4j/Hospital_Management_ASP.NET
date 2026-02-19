using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("logintable", Schema = "dbo")]
[Index("email", Name = "uq_login_email", IsUnique = true)]
public partial class logintable
{
    [Key]
    public int loginid { get; set; }

    [StringLength(20)]
    public string password { get; set; } = null!;

    [StringLength(30)]
    public string email { get; set; } = null!;

    public int type { get; set; }

    [InverseProperty("patientNavigation")]
    public virtual patient? patient { get; set; }
}
