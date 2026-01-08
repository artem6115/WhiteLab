using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class MainState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        var msg = new TelegramStringBuilder();
        switch (callback.Data)
        {
            case "my asm":
                user.PreviewState = this;
                if (user.PCAssembly == null) user.CurrentState = new BudgetState();
                else user.CurrentState = new PCConfigurationState();
                await user.CurrentState.SendPage(client, user, ct);
                break;
            case "about me list":
                await client.EditMessageReplyMarkup(user.ChatId, callback.Message!.Id, GetInlineAutorButtons(), cancellationToken: ct);
                break;
            case "back":
                await client.EditMessageReplyMarkup(user.ChatId, callback.Message!.Id, GetInlineButtons(), cancellationToken: ct);
                break;
            case "about me":
                msg
                    .AddLineStr("Меня зовут Артём")
                    .AddLineStr()
                    .AddLineStr("Я работаю программистом и занимаюсь сборкой и настройкой ПК в городе Челябинск")
                    .AddStr("Делаю качественные, ").AddItalicStrHtml("чистые")
                    .AddLineStr(" сборки")
                    .AddLineStr("Для всех сборок которые я делаю, я предоставляю тесты всех компонентов");
                await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, cancellationToken: ct);
                break;
            case "buy instruction":
                msg
                    .AddLineStr("Если вы не ещё не подобрали комплектующие, можете воспользоваться пощью бота или же подобрать сразу со сборщиком")
                    .AddLineStr("После подбора комплектующих, напшите сборщику, отправьте список комплектующих, сборщик проверит на совместимость и напишет точную стоимость и сроки.")
                    .AddLineStr("Отслеживать процесс закупа компонентов, сборки и отправки можно прямо в боте, в главном меню");
                await client.SendMessage(user.ChatId, msg.ToString(), cancellationToken: ct);
                break;
            case "cans list":
                msg
                    .AddBoldStrHtml("Онлайн:").AddLineStr()
                    .AddLineStr("✳ Подбор комплектующих")
                    .AddLineStr("✳ Проверка комплектующих на совместимость")
                    .AddLineStr("✳ Помощь со сборкой")
                    .AddLineStr("---------------------------------------------")
                    .AddBoldStrHtml("Офлайн:").AddLineStr()
                    .AddLineStr("✳ Установка/Переустановка Windows")
                    .AddLineStr("✳ Очистка Windows")
                    .AddLineStr("✳ Разгон ПК")
                    .AddLineStr("✳ Упграде ПК")
                    .AddLineStr("✳ Поиск и удаление вирусов");
                await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, cancellationToken: ct);
                break;
            case "about bot":
                DefaultState.AboutMessage(msg);
                await client.SendMessage(user.ChatId, msg.ToString(), replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
                break;
            case "about whitelab":
                msg = new TelegramStringBuilder();
                msg
                    .AddLineStr("\u2744 WhiteLab — лаборатория белых сборок")
                    .AddLineStr()
                    .AddLineStr("Мы занимаемс закупом и сборкой комплектующих пк, в основном орентир на сборку белых пк (но не только)")
                    .AddLineStr("Закуп комплектующих в ДНС, с предоставлением гарантии из магазина")
                    .AddBoldStrHtml("Гарантии: ")
                    .AddLineStr()
                    .AddLineStr("  ✅ гарантия комплектующих по чекам из ДНС")
                    .AddLineStr("  ✅ 30 дней бесплатной диагностики, при наличии проблем с компьютером")
                    .AddLineStr("  ✅ предоставление результатов тестов перед передачей сбораного пк (AIDA64, FurMark, CrystalDisk и др)")
                    .AddLineStr()
                    .AddLineStr("Доставляем бесплатно самостоятельно клиенту в городе Челябинск, Копейск")
                    .AddLineStr("В другие города есть возможность отправки с помощью авито доставки");

                await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
                break;
            case "bots":
                user.PreviewState = this;
                user.CurrentState = new BotsListState();
                await user.CurrentState.SendPage(client, user, ct);
                break;
            default:
                await client.SendMessage(user.ChatId, $"\U000026D4 Ошибка ввода{Environment.NewLine}Можете использовать команду /help для просмотра команд", replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
                break;
        }
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        await client.SendMessage(user.ChatId, $"\U000026D4 Ошибка ввода{Environment.NewLine}Можете использовать команду /help для просмотра команд", replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Главное меню PCConfigurator \U0001F4F1")
            .AddLineStr("------------------------------------")
            .AddBoldStrHtml(" \U0001F5A5 Ваша сборка: ")
            .AddBoldStrHtml(user.PCAssembly?.Price.ToString() ?? "")
            .AddLineStr()
            .AddBoldStrHtml("\U0001F6D2 Заказы : ")
            .AddBoldStrHtml(user.OrdrStatus ?? "")
            .AddLineStr()
            .AddLineStr("------------------------------------")
            .AddLineStr()
            .AddLineStr("Для управления ботом, используйте кнопки на экране и список команды внизу экрана")
            .AddLineStr("Бот предоставляет сбокри под конкретные нужды пользователя.")
            .AddLineStr("\U000026A0 Внимание: бот не гарантирует полную совместимость компонентов и не заменяет специалистов \U000026A0");

        var messageId = (await client.SendMessage(user.ChatId, str.ToString(), ParseMode.Html, replyMarkup: GetInlineButtons(), cancellationToken: ct)).Id;
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
                    new KeyboardButton("Моя сборка 🖥")
                },
                new[]
                {
                    new KeyboardButton("Информация о боте 📖"),
                },
                new[]
                {
                    new KeyboardButton("Информация о WhiteLab 🔬")
                },
                new[]
                {
                    new KeyboardButton("Другие боты \U0001f9f0")
                }
            },
            ResizeKeyboard = true
        };
    }

    private InlineKeyboardMarkup GetInlineButtons()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Моя сборка 🖥", "my asm"),
                InlineKeyboardButton.WithUrl("Заказать сборку","https://t.me/Artem6115")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Информация о боте \U0001F4D6", "about bot")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Информация о WhiteLab \U0001F52C", "about whitelab")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Информация о сборщике \U0001F468", "about me list")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Другие боты \U0001F9F0", "bots")
            }
        });
    }

    private InlineKeyboardMarkup GetInlineAutorButtons()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Информация о сборщике \U0001F468", "about me")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Порядок оформления заказа \U0001F4DC", "buy instruction")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Услуги \U0001F4CB", "cans list")
            },
            new []
            {
                InlineKeyboardButton.WithUrl("Приверы сборок","https://t.me/Artem6115"),
                InlineKeyboardButton.WithUrl("Заказать сборку","https://t.me/Artem6115")
            },
            DefaultState.GetBackButton()
        });
    }
}
