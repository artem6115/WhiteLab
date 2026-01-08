using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WhiteLab.Telegram.DialogStates;

namespace WhiteLab.Telegram;

internal class TelegramRecipient : IUpdateHandler
{
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        await Task.Yield();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception.Message);
        Console.WriteLine("Inner: " + exception.InnerException?.Message ?? "");
        Console.ResetColor();

    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Message == null)
        {
            await CallbackUpdateAsync(botClient, update, ct);
            return;
        }

        var chatid = update.Message.Chat.Id;
        UserProvider.UsersData.TryGetValue(chatid, out var user);
        if (user == null)
        {
            user = new UserData() { ChatId = chatid };
            UserProvider.UsersData.TryAdd(chatid, user);
        }

        if (user.IsAdmin || update.Message.Text?.Trim() == TelegramOptions.Token)
        {
            await HandleAdminUpdateAsync(botClient, update, user, ct);
            return;
        }

        if (await DefaultState.AcceptcMessage(botClient, update.Message, user, ct)) return;
        user.CurrentState ??= new MainState();
        await user.CurrentState.AcceptcMessage(botClient, update.Message, user, ct);
    }

    private async Task CallbackUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.CallbackQuery == null) return;

        var id = update.CallbackQuery.From.Id;
        UserProvider.UsersData.TryGetValue(id, out var user);
        if (user == null)
        {
            user = new UserData() { ChatId = id };
            UserProvider.UsersData.TryAdd(id, user);
        }

        if (user.CurrentState == null)
        {
            user.CurrentState = new MainState();
            await user.CurrentState.SendPage(botClient, user, ct);
            return;
        }

        await user.CurrentState.AcceptcCallback(botClient, update.CallbackQuery, user, ct);
    }

    private async Task HandleAdminUpdateAsync(ITelegramBotClient botClient, Update update, UserData user, CancellationToken ct)
    {
        if(update.Type == UpdateType.Message)
        {
            if(update.Message!.Text != null && update.Message.Text.Trim().ToLower() == "exit")
            {
                user.IsAdmin = false;
                await botClient.SendMessage(user.ChatId, "Выход из режима админимтратора", cancellationToken: ct);
                return;
            }
        }
        user.IsAdmin = true;
        await botClient.SendMessage(user.ChatId, "Принято (admin)✅", cancellationToken: ct);
    }
}
