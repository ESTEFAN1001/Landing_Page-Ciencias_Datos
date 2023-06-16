using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LosCokis123.Models;

public partial class Podcast
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo titulo no puede estar vacio")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "El campo descripcion no puede estar vacio")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "El campo  no puede estar vacio")]
    public string? Author { get; set; }

    public string? Image { get; set; } 

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Episode> Episodes { get; } = new List<Episode>();
}
