using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteLab.Telegram;

public class UserData
{
    public bool IsAdmin { get; set; }
    public long ChatId { get; init; }
    public int? LastMessageId { get; set; }
    public IDialogState? CurrentState { get; set; }
    public object? Buffer { get; set; }
    public string? PCAssembly { get; set; }
    public string? OrdrStatus { get; set; } 
}
