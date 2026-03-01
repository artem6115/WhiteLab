using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class ColorState : IDialogState
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

        switch (message.Text)
        {
            case "Белый":
                user.Requirements!.ColorStyle = ColorStyleEnum.White;
                break;
            case "Серебряный":
                user.Requirements!.ColorStyle = ColorStyleEnum.Silver;
                break;
            case "Чёрный":
                user.Requirements!.ColorStyle = ColorStyleEnum.Dark;
                break;
            case "Неважно": break;
            default:
                await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtons(), cancellationToken: ct);
                return;
        }

        user.PreviewStates.Push(this);
        user.CurrentState = new RgbState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 12/14 ✅")
            .AddLineStr()
            .AddBoldStrHtml("Выберите основной цвет компонентов пк:");

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
                        new KeyboardButton("Белый"),
                        new KeyboardButton("Серебряный")
                    },
                    new[]
                    {
                        new KeyboardButton("Чёрный"),
                        new KeyboardButton("Неважно")
                    },
                    new[]
                    {
                        new KeyboardButton("Назад \U000021A9")
                    },
                },
            ResizeKeyboard = true
        };
    }
    private static ReplyMarkup GetButtons()
    {
        return new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Назад \U000021A9"),
                    new KeyboardButton("Далее")
                }
            },
            ResizeKeyboard = true
        };
    }
}
