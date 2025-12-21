using Telegram.Bot;
using Telegram.Bot.Types;

namespace WhiteLab.Telegram;

public interface IDialogState
{
    Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct);
    Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct);
    Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct);
}
