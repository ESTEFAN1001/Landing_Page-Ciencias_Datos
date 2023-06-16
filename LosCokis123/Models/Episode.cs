using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LosCokis123.Models;

public partial class Episode
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo titulo no puede estar vacio")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "El campo descripcion no puede estar vacio")]
    public string? Description { get; set; }
    public int? PodcastId { get; set; }
    [Required(ErrorMessage = "El campo autor no puede estar vacio")]
    public string? Author { get; set; }
    [Required(ErrorMessage = "El campo duracion no puede estar vacio")]
    public string? Duration { get; set; }
    public string? AudioUrl { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Favorite> Favorites { get; } = new List<Favorite>();

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual Podcast? Podcast { get; set; }
}
