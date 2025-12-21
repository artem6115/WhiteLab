using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class BotsListState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        if(callback.Data == "back")
        {
            user.CurrentState = new MainState();
            await user.CurrentState.SendPage(client, user, ct);
        }
    }

    public Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var msg = new TelegramStringBuilder();
        msg
            .AddBoldStrHtml("PCConfigurator - Бот для сборки и заказа пк")
            .AddLineStr()
            .AddLineStr()
            .AddBoldStrHtml("CPUOverclocker - Бот который поможет разогнать любой процессор, содержит в себе ряд инструкций для различных моделей")
            .AddLineStr()
            .AddLineStr()
            .AddBoldStrHtml("RAMOverclocker - Бот который поможет разогнать оперативную память, содержит в себе ряд различных инструкций в зависимости от материнской платы и др параметров.")
            .AddLineStr()
            .AddLineStr()
            .AddBoldStrHtml("GPUOverclocker - Бот который поможет разогнать видеокарты с помощью программы, содержит в себе ряд инструкций для достижения максимума по видеокарте");
        user.LastMessageId = (await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, replyMarkup: GetInlineKeyboardButton(), cancellationToken: ct)).Id;
        
    }

    private InlineKeyboardMarkup GetInlineKeyboardButton() 
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithUrl("PCConfigurator \U0001F4BB","https://t.me/WhiteLab_PCConfigurator_bot")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("CPUOverclocker \U0001F4DF")

            },
            new []
            {
                 InlineKeyboardButton.WithCallbackData("RAMOverclocker \U0001F4FC")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("GPUOverclocker \U0001F4FD")
            },
            DefaultState.GetBackButton()
        });
    }
}
