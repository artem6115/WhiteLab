using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class DiskSizeState : IDialogState
{
    public Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text = message.Text?.Trim() ?? "";
        if (message.Text.Contains("Назад"))
        {
            user.CurrentState = user.GoBack();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        if(!ushort.TryParse(message.Text, out var value))
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку или ввидите целое число", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.Requirements!.MemorySize = value;
        user.PreviewStates.Push(this);
        user.CurrentState = new SplitDiskState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 7/14 ✅")
            .AddLineStr()
            .AddBoldStrHtml("Сколько гигабайт долгосрочной памяти нужено:\U0001F5A5");

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
                        new KeyboardButton("500"),
                        new KeyboardButton("1000")
                    },
                    new[]
                    {
                        new KeyboardButton("1500"),
                        new KeyboardButton("2000")
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
