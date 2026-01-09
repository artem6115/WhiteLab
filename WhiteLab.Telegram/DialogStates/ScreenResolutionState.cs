using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class ScreenResolutionState : IDialogState
{
    public Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";
        message.Text = message.Text.Replace("2K", "2000").Replace("4K","4000");
        if (message.Text.Contains("Назад"))
        {
            user.CurrentState = user.GoBack();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        if (message.Text != "1080" && message.Text != "1440" && message.Text != "2000" && message.Text != "4000")
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.Requirements?.ScreenResolution = ushort.Parse(message.Text);
        user.PreviewStates.Push(this);
        user.CurrentState = new GraphicsLevelState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 3/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Выбор расширения экрана монитора")
            .AddLineStr()
            .AddLineStr("Нажмите на кнопку с расширенем которе есть у вашего монитора либо, у монитора который планируете использовать с данным компьютером");

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
                    new KeyboardButton("1080"),
                    new KeyboardButton("1440")
                },
                new[]
                {
                    new KeyboardButton("2K"),
                    new KeyboardButton("4K")
                },
                new[]
                {
                    new KeyboardButton("Назад \U000021A9")
                },
            },
            ResizeKeyboard = true
        };
    }
}
