using System;
using System.Collections.Generic;

namespace Crud.Models;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public bool AcceptTerms { get; set; }
}
