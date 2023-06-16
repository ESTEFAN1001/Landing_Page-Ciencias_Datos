﻿using System;
using System.Collections.Generic;

namespace LosCokis123.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string? Coment { get; set; }

    public int? EpisodeId { get; set; }

    public string? UserId { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Episode? Episode { get; set; }

}
