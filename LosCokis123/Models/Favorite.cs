using System;
using System.Collections.Generic;

namespace LosCokis123.Models;

public partial class Favorite
{
    public int Id { get; set; }

    public bool? Favorite1 { get; set; }

    public int? EpisodeId { get; set; }

    public string? UserId { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Episode? Episode { get; set; }

}
