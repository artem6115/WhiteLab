using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class GraphicsLevelState : IDialogState
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
            user.CurrentState = user.PreviewState ?? new MainState();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        if (message.Text != "Low" && message.Text != "Medium" && message.Text != "High" && message.Text != "Ultra")
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.Requirements?.GraphicsLevel = (GraphicsLevelEnum)Enum.Parse(typeof(GraphicsLevelEnum), message.Text);
        user.PreviewState = this;
        user.CurrentState = new WorkTirState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 4/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Выберите настройки графики")
            .AddLineStr()
            .AddLineStr("Выберите уровень графики который компьютер должен поддерживать");

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
                    new KeyboardButton("Low"),
                    new KeyboardButton("Medium")
                },
                new[]
                {
                    new KeyboardButton("High"),
                    new KeyboardButton("Ultra")
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
