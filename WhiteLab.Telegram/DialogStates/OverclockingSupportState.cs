using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class OverclockingSupportState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    { }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";
        if (message.Text.Contains("Назад"))
        {
            user.CurrentState = user.PreviewState ?? new MainState();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        OverclockingEnum result = message.Text switch
        { 
            "CPU и RAM" => OverclockingEnum.ALL,
            "Разгон CPU" => OverclockingEnum.CPU,
            "Разгон RAM" => OverclockingEnum.RAM,
            _ => OverclockingEnum.Default
        };

        user.Requirements!.OverclockingSupport = result;
        user.PreviewState = this;
        user.CurrentState = new YangestComponentsState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 7/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Разгон!")
            .AddLineStr()
            .AddLineStr($"Нужна ли вам поддержка разгона процессора или оперативной памяти?");

        var messageId = (await client.SendMessage(user.ChatId, str.ToString(), ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct)).Id;
        user.LastMessageId = messageId;
    }

    private ReplyMarkup GetButtonsKeyboard()
    {

        return new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Разгон CPU"),
                    new KeyboardButton("Разгон RAM"),
                    new KeyboardButton("CPU и RAM")
                },
                new[]
                {
                    new KeyboardButton("Не имеет значения"),
                    new KeyboardButton("Что это? (нет)"),
                    new KeyboardButton("Назад \U000021A9")
                }
            },
            ResizeKeyboard = true
        };
    }
}
