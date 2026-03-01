using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates
{
    internal class SplitDiskState : IDialogState
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

            if (message.Text != "Да (если возможно)" && message.Text != "Нет")
            {
                await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
                return;
            }

            user.Requirements!.SplitDisk = message.Text != "Нет";
            user.PreviewStates.Push(this);
            user.CurrentState = new OverclockingSupportState();
            await user.CurrentState.SendPage(client, user, ct);
        }

        public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
        {
            var str = new TelegramStringBuilder();
            str
                .AddItalicStrHtml("Шаг 8/14 ✅")
                .AddLineStr()
                .AddBoldStrHtml("Разделить объем памяти на несколько ssd?");

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
                    new KeyboardButton("Да (если возможно)"),
                    new KeyboardButton("Нет")
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
}
