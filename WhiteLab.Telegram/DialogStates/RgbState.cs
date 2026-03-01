using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class RgbState : IDialogState
{
    private static InlineKeyboardMarkup _inlineKeyboardMarkup;
    static RgbState()
    {
        _inlineKeyboardMarkup = GetInlineKeyboard();
    }
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        var msg = new TelegramStringBuilder();
        callback.Data = callback.Data?.Trim();
        switch (callback.Data)
        {
            case "back":
                user.CurrentState = user.GoBack();
                await user.CurrentState.SendPage(client, user, ct);
                return;
            case "reset":
                user.Buffer = null;
                await EditTextAsync(client, user, ct);
                return;
            case "next":
                user.Requirements!.Rgb = user.Buffer as HashSet<RgbEnum> ?? user.Requirements!.Rgb;
                user.Buffer = null;
                user.PreviewStates.Push(this);
                user.CurrentState = new WishesState();
                await client.SendMessage(user.ChatId, $"Осталось совсем чут-чуть!{Environment.NewLine}Ваши пожелания к сборке", replyMarkup:GetButtons(), cancellationToken: ct);
                await user.CurrentState.SendPage(client, user, ct);
                return;
        }

        if (Enum.TryParse<RgbEnum>(callback.Data, true, out var rgb))
        {
            user.Buffer ??= new HashSet<RgbEnum>();
            if (user.Buffer is HashSet<RgbEnum> rgbs)
            {
                if (rgbs.Contains(rgb)) return;
                rgbs.Add(rgb);
                user.Buffer = rgbs;
                await EditTextAsync(client, user, ct);
            }
        }
        else
        {
            await client.SendMessage(user.ChatId, $"\U000026D4 Ошибка ввода{Environment.NewLine}Можете использовать команду /help для просмотра команд", replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
        }

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

        if (message.Text == "Далее")
        {
            user.Requirements!.Rgb = user.Buffer as HashSet<RgbEnum> ?? user.Requirements!.Rgb;
            user.Buffer = null;
            user.PreviewStates.Push(this);
            user.CurrentState = new WishesState();
            await client.SendMessage(user.ChatId, $"Осталось совсем чут-чуть!{Environment.NewLine}Ваши пожелания к сборке", replyMarkup: GetButtons(), cancellationToken: ct);
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }
        await client.SendMessage(user.ChatId, $"\U000026D4 Ошибка ввода{Environment.NewLine}Можете использовать команду /help для просмотра команд", replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var msg = new TelegramStringBuilder();
        msg
            .AddItalicStrHtml("Шаг 13/14 ✅")
            .AddBoldStrHtml("Укажите какие компоненты должны иметь RGB подсветку:")
            .AddLineStr();
        if(user.Buffer is List<RgbEnum> s)
        {
            msg.AddItalicStrHtml("Задано: " + string.Join(',', s));
        }

        user.LastMessageId = (await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, replyMarkup: _inlineKeyboardMarkup, cancellationToken: ct)).Id;

    }

    public async Task EditTextAsync(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        if (user.LastMessageId is null)
        {
            await SendPage(client, user, ct);
            return;
        }

        var msg = new TelegramStringBuilder();
        msg
            .AddItalicStrHtml("Шаг 13/14 ✅")
            .AddBoldStrHtml("Укажите какие компоненты должны иметь RGB подсветку:")
            .AddLineStr();
        if (user.Buffer is HashSet<RgbEnum> s)
        {
            msg.AddItalicStrHtml("Задано: " + string.Join(',', s));
        }

        user.LastMessageId = (await client.EditMessageText(user.ChatId, user.LastMessageId.Value, msg.ToString(), ParseMode.Html, replyMarkup: _inlineKeyboardMarkup, cancellationToken: ct)).Id;

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


    private static InlineKeyboardMarkup GetInlineKeyboard()
    {
        int i = 0;
        var markup = new InlineKeyboardMarkup();
        var btns = new InlineKeyboardButton[2];
        foreach (var w in Enum.GetNames<RgbEnum>())
        {
            btns[i++] = InlineKeyboardButton.WithCallbackData(w,w);
            if (i == 2)
            {
                markup.AddNewRow(btns);
                btns = new InlineKeyboardButton[2];
                i = 0;
            }
        }

        markup.AddNewRow([
            InlineKeyboardButton.WithCallbackData("Сбросить","reset"),
            InlineKeyboardButton.WithCallbackData("Назад", "back"),
            InlineKeyboardButton.WithCallbackData("Далее","next")]);
        return markup;
    }
}
