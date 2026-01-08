using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class WorkTirState : IDialogState
{
    public Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";
        message.Text = message.Text.Replace(" ", "");

        if (message.Text.Contains("Назад"))
        {
            user.CurrentState = user.PreviewState ?? new MainState();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }


        if (!Enum.TryParse(typeof(PowerLevelEnum), message.Text, true, out var result))
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.Requirements!.PowerLevel = (PowerLevelEnum)result;
        user.PreviewState = this;
        user.CurrentState = new SelectProgramsState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 5/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Выберите требования к производительности")
            .AddLineStr()
            .AddLineStr("Выберите какого уровня задачи должен решать компьютер");

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
                    new KeyboardButton("School"),
                    new KeyboardButton("Study"),
                    new KeyboardButton("Office"),
                },
                new[]
                {
                    new KeyboardButton("Gaming"),
                    new KeyboardButton("Render"),
                    new KeyboardButton("Streamin"),
                },
                new[]
                {
                    new KeyboardButton("High Gaming"),
                    new KeyboardButton("Pro Gaming"),
                },
                new[]
                {
                    new KeyboardButton("Hard Render"),
                    new KeyboardButton("Pro Render"),
                },
                new[]
                {
                    new KeyboardButton("Gaming and Render"),
                },
                new[]
                {
                    new KeyboardButton("Hard Gaming and Render"),
                },
                new[]
                {
                    new KeyboardButton("Pro Gaming and Render"),
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
