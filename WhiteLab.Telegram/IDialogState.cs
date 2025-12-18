using Telegram.Bot;

namespace WhiteLab.Telegram;

public interface IDialogState
{
    void AcceptcMessage(ITelegramBotClient client, CancellationToken ct);
}
