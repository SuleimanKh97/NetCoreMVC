using System;
using System.Collections.Generic;

namespace Day8Model.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ContactInfo { get; set; } = null!;

    public string AssignedSection { get; set; } = null!;

    public DateOnly EmploymentDate { get; set; }

    public int Experience { get; set; }
}
