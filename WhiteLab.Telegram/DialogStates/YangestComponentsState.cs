using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class YangestComponentsState : IDialogState
{
    public Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";

        if (message.Text.Contains("Назад"))
        {
            user.CurrentState = user.GoBack();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        if (message.Text != "Да" && message.Text != "Неважно")
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.Requirements!.YangestComponents = message.Text == "Да";
        user.PreviewStates.Push(this);
        user.CurrentState = new BuildPcConfigtState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 8/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Только современное железо?")
            .AddLineStr()
            .AddLineStr("Хотите ли вы иметь современные компоненты?")
            .AddLineStr("Это даёт возможность апгрейда на более мощные компоненты в будущем, но значительно увеличивает итоговую стоимость");


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
                    new KeyboardButton("Да"),
                    new KeyboardButton("Неважно"),
                    new KeyboardButton("Назад \U000021A9")
                }
            },
            ResizeKeyboard = true
        };
    }
}
