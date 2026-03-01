using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

/*
 1 - budget
 2 - need GPU
 3 - monitor resolution
 4 - grapfics level
 5 - target
 6 - soft
 7 - disk size
 8 - split disk
 9 - kachegaring
 10 - yangest components
 11 - from factor
 12 - color
 13 - rgb 
 14 - wishes

 */

internal class MainState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        var msg = new TelegramStringBuilder();
        switch (callback.Data)
        {
            case "my asm":
                user.PreviewStates.Push(this);
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
                    .AddLineStr("Меня зовут Артём — программист и сборщик ПК из Челябинска.")
                    .AddLineStr()
                    .AddStr("Специализируюсь на белых, ").AddItalicStrHtml("чистых ")
                    .AddLineStr("сборках с гарантией качества:")
                    .AddLineStr("✅ Закуп комплектующих в ДНС с чеком и заводской гарантией")
                    .AddLineStr("✅ 30 дней бесплатной диагностики готовой системы")
                    .AddLineStr("✅ Полные тесты всех компонентов (AIDA64, FurMark, CrystalDiskMark+)")
                    .AddLineStr("✅ Бесплатная доставка по Челябинску и Копейску")
                    .AddLineStr()
                    .AddLineStr("Разработал PCConfigurator — Telegram-бота для точного подбора комплектующих под бюджет и задачи. Автоматизирую подбор, но каждый ПК собираю вручную.")
                    .AddLineStr()
                    .AddLineStr("Сборка под ключ = ваша мечта + моя экспертиза");

                await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, cancellationToken: ct);
                break;
            case "buy instruction":
                msg
                    .AddLineStr("Если вы ещё не подобрали комплектующие, можете воспользоваться помощью бота или сразу обратиться к сборщику")
                    .AddLineStr("После подбора комплектующих напишите сборщику, отправьте список — он проверит совместимость, точную стоимость и сроки")
                    .AddLineStr("Отслеживать процесс закупки компонентов, сборки и отправки можно прямо в боте, в главном меню");
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
                    .AddLineStr("✳ Апгрейд ПК")
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
                    .AddLineStr("❄ WhiteLab — лаборатория белых сборок")
                    .AddLineStr()
                    .AddLineStr("Мы занимаемся закупом и сборкой комплектующих ПК, в основном ориентир на сборку белых ПК (но не только).")
                    .AddLineStr("Закуп комплектующих в ДНС с предоставлением гарантии из магазина.")
                    .AddBoldStrHtml("Гарантии: ")
                    .AddLineStr()
                    .AddLineStr("  ✅ гарантия комплектующих по чекам из ДНС")
                    .AddLineStr("  ✅ 30 дней бесплатной диагностики при наличии проблем с компьютером")
                    .AddLineStr("  ✅ предоставление результатов тестов перед передачей собранного ПК (AIDA64, FurMark, CrystalDisk и др.)")
                    .AddLineStr()
                    .AddLineStr("Бесплатно доставим ПК клиенту в Челябинск, Копейск.")
                    .AddLineStr("В другие города есть возможность отправки с помощью Авито Доставки");

                await client.SendMessage(user.ChatId, msg.ToString(), ParseMode.Html, replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
                break;
            case "bots":
                user.PreviewStates.Push(this);
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
            .AddBoldStrHtml((user.PCAssembly?.Price.ToString() ?? "0" + "₽"))
            .AddLineStr()
            .AddBoldStrHtml("\U0001F6D2 Заказы : ")
            .AddBoldStrHtml(user.OrderStatus ?? "")
            .AddLineStr()
            .AddLineStr("------------------------------------")
            .AddLineStr()
            .AddLineStr("Для управления ботом используйте кнопки на экране и список команд внизу.")
            .AddLineStr("Бот предоставляет сборки под конкретные нужды пользователя.")
            .AddLineStr("\U000026A0 Внимание: бот не гарантирует полную совместимость компонентов и не заменяет специалистов \U000026A0");

        var messageId = (await client.SendMessage(user.ChatId, str.ToString(), ParseMode.Html, replyMarkup: GetInlineButtons(), cancellationToken: ct)).Id;
        user.LastMessageId = messageId;
    }

    private InlineKeyboardMarkup GetInlineButtons()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Моя сборка 🖥", "my asm"),
                InlineKeyboardButton.WithUrl("Готовые сборки 🖥","https://t.me/Artem6115"),

            },
            new []
            {
                InlineKeyboardButton.WithUrl("Заказать сборку","https://t.me/Artem6115")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Информация о боте \U0001F4D6", "about bot")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("О WhiteLab \U0001F52C", "about whitelab")

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("О сборщике \U0001F468", "about me list")

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
                InlineKeyboardButton.WithUrl("Примеры сборок","https://t.me/Artem6115"),
                InlineKeyboardButton.WithUrl("Заказать сборку","https://t.me/Artem6115")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Услуги \U0001F4CB", "cans list"),
                DefaultState.GetBackInlineButton()

            }
        });
    }
}
