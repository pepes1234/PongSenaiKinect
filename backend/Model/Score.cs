using System;
using System.Collections.Generic;

namespace backend.Model;

public partial class Score
{
    public int Id { get; set; }

    public int? Player1Nickname { get; set; }

    public int? Player2Nickname { get; set; }

    public int? Player1Score { get; set; }

    public int? Player2Score { get; set; }

    public virtual Usuario? Player1NicknameNavigation { get; set; }

    public virtual Usuario? Player2NicknameNavigation { get; set; }
}
