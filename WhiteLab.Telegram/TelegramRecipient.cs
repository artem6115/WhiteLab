using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Type == UpdateType.Message)
        {
            await botClient.SendMessage(update.Message!.Chat.Id, "Hello maboy)" ,replyParameters:
                new ReplyParameters() { ChatId = update.Message.Chat.Id, MessageId = update.Message.Id });
            throw new Exception("STUP");
        }
    }
}
