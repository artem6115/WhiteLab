using System;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;
using WhiteLab.Telegram.DialogStates;

namespace WhiteLab.Telegram;

public class UserData
{
    public bool IsAdmin { get; set; }
    public long ChatId { get; init; }
    public int? LastMessageId { get; set; }
    public IDialogState? CurrentState { get; set; }
    public Stack<IDialogState> PreviewStates { get; set; } = new();

    public object? Buffer { get; set; }
    public Requirements? Requirements { get; set; }
    public PCConfig? PCAssembly { get; set; }
    public string? OrdrStatus { get; set; } 

    public IDialogState GoBack() => PreviewStates.Any() ? PreviewStates.Pop() : new MainState();
}
