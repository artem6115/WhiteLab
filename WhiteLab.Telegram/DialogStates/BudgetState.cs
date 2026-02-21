using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class BudgetState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";
        if(message.Text.Contains("Без ограничений"))
        {
            message.Text = uint.MaxValue.ToString();
        }
        else if(message.Text.Contains("Назад"))
        {
            user.CurrentState = user.GoBack();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }
        var usaVal = message.Text.Contains("$") || message.Text.Contains("долларов") || message.Text.Contains("dolors");
        var parsed = message.Text
            .ToLower()
            .Replace("k", "000")
            .Replace("m", "000000")
            .Replace("к", "000")
            .Replace("м", "000000")
            .Replace("thousand", "000")
            .Replace("тысяч", "000")
            .Replace("тыс", "000")
            .Replace("тыщ", "000")
            .Replace("₽", "")
            .Replace("руб", "")
            .Replace("рублей", "")
            .Replace("$", "")
            .Replace("долларов", "")
            .Replace("dolors", "")
            .Split([' ', '.', ',', '-'])
            .Where(s => !string.IsNullOrWhiteSpace(s));

        var bgText = string.Join("", parsed);

        if (!uint.TryParse(bgText, out var badget))
        {
            await client.SendMessage(user.ChatId, "Пожалуйста отправьте число или нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }
        if (usaVal) badget *= 75;
        if(badget != uint.MaxValue)await client.SendMessage(user.ChatId, $"Бюджет: {badget}₽", cancellationToken: ct);
        user.Requirements = new PCConfigurator.Requirenments.Requirements() { Budget = badget };
        user.PreviewStates.Push(this);
        user.CurrentState = new ExistPGUState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 1/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Отлично, приступим к созданию сборки \U0001F5A5")
            .AddLineStr()
            .AddLineStr("Для начала определимся с бюджетом. Напишите примерную сумму (пример: 120 000) или нажмите кнопку 'Без ограничений' — программа подберёт оптимальную сборку по требованиям и назовёт стоимость");

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
                    new KeyboardButton("Без ограничений"),
                    new KeyboardButton("Назад \U000021A9")
                },
            },
            ResizeKeyboard = true
        };
    }
}
