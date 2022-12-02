using System;
using System.Collections.Generic;

namespace backend.Model;

public partial class Rgb
{
    public int Id { get; set; }

    public int? ArrayIndex { get; set; }

    public int? UserId { get; set; }

    public int? R { get; set; }

    public int? G { get; set; }

    public int? B { get; set; }

    public virtual Usuario? ArrayIndexNavigation { get; set; }

    public virtual Usuario? User { get; set; }
}
