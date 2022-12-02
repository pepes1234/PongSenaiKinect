using System;
using System.Collections.Generic;

namespace backend.Model;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nickname { get; set; }

    public byte[]? FaceData { get; set; }

    public virtual ICollection<Score> ScorePlayer1NicknameNavigations { get; } = new List<Score>();

    public virtual ICollection<Score> ScorePlayer2NicknameNavigations { get; } = new List<Score>();
}
