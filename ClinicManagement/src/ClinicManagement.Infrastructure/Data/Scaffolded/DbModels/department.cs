using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;

[Table("department", Schema = "dbo")]
[Index("deptname", Name = "uq_dept_name", IsUnique = true)]
public partial class department
{
    [Key]
    public int deptno { get; set; }

    [StringLength(30)]
    public string deptname { get; set; } = null!;

    [StringLength(1000)]
    public string? description { get; set; }

    [InverseProperty("deptnoNavigation")]
    public virtual ICollection<doctor> doctors { get; set; } = new List<doctor>();
}
