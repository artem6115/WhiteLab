using System;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram;

public class UserData
{
    public bool IsAdmin { get; set; }
    public long ChatId { get; init; }
    public int? LastMessageId { get; set; }
    public IDialogState? CurrentState { get; set; }
    public IDialogState? PreviewState { get; set; }

    public object? Buffer { get; set; }
    public Requirements? Requirements { get; set; }
    public PCConfig? PCAssembly { get; set; }
    public string? OrdrStatus { get; set; } 
}
