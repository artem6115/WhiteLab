using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class WishesState : IDialogState
{
    private static InlineKeyboardMarkup _inlineKeyboardMarkup;
    static WishesState()
    {
        _inlineKeyboardMarkup = GetInlineKeyboardButton();
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
                user.Requirements!.Wishes = user.Buffer as HashSet<WishesEnum> ?? user.Requirements!.Wishes;
                user.Buffer = null;
                user.PreviewStates.Push(this);
                user.CurrentState = new BuildPcConfigtState();
                await user.CurrentState.SendPage(client, user, ct);
                return;
        }

        if (Enum.TryParse<WishesEnum>(callback.Data, true, out var wish))
        {
            user.Buffer ??= new HashSet<WishesEnum>();
            if(user.Buffer is HashSet<WishesEnum> wishes)
            {
                if (wishes.Contains(wish)) return;

                wishes.Add(wish);
                user.Buffer = wishes;
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
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }
        await client.SendMessage(user.ChatId, $"\U000026D4 Ошибка ввода{Environment.NewLine}Можете использовать команду /help для просмотра команд", replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        int k = (user.Buffer as HashSet<WishesEnum>)?.Count ?? 0;
        var msg = new TelegramStringBuilder();
        msg
            .AddItalicStrHtml("Шаг 14/14 ✅")
            .AddBoldStrHtml("Укажите пожелания к сборке:")
            .AddLineStr()
            .AddItalicStrHtml("Задано: " + k);
        user.LastMessageId = (await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, replyMarkup: _inlineKeyboardMarkup, cancellationToken: ct)).Id;

    }

    public async Task EditTextAsync(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        if (user.LastMessageId is null)
        {
            await SendPage(client, user, ct);
            return;
        }

        int k = (user.Buffer as HashSet<WishesEnum>)?.Count ?? 0;
        var msg = new TelegramStringBuilder();
        msg
            .AddItalicStrHtml("Шаг 14/14 ✅")
            .AddBoldStrHtml("Укажите пожелания к сборке:")
            .AddLineStr()
            .AddItalicStrHtml("Задано: " + k);
        user.LastMessageId = (await client.EditMessageText(user.ChatId, user.LastMessageId.Value, msg.ToString(), ParseMode.Html, replyMarkup: _inlineKeyboardMarkup, cancellationToken: ct)).Id;

    }

    private static InlineKeyboardMarkup GetInlineKeyboardButton()
    {
        int i = 0;
        var markup = new InlineKeyboardMarkup();
        var btns = new InlineKeyboardButton[3];
        foreach (var w in Enum.GetNames<WishesEnum>())
        {
            btns[i++] = InlineKeyboardButton.WithCallbackData(w, w);
            if (i == 3)
            {
                markup.AddNewRow(btns);
                btns = new InlineKeyboardButton[3];
                i = 0;
            }
        }

        markup.AddNewRow([
            InlineKeyboardButton.WithCallbackData("Сбросить", "reset"),
            InlineKeyboardButton.WithCallbackData("Назад", "back"),
            InlineKeyboardButton.WithCallbackData("Далее", "next")]);
        return markup;
    }
}
