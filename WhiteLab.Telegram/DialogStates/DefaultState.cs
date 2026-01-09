using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal static class DefaultState
{
    public static async Task<bool> AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        var botMessage = new TelegramStringBuilder();
        var text = message.Text?.Trim().ToLower() ?? "";
        if (text == "/start")
        {
            botMessage.AddStr($"{message.Chat.FirstName ?? "Новый пользователь"} рад что вы обратились ко мне, буду рад помоч \U0001F91D");
            user = new UserData() { ChatId = user.ChatId };
            user.CurrentState = new MainState();
        }
        else if (text == "/help" || text.StartsWith("помощь"))
        {
            HelpMessage(botMessage);
        }
        else if (text == "/about" || text.StartsWith("информация о боте"))
        {
            AboutMessage(botMessage);
        }
        else if (text == "/support" || text.StartsWith("поддержка"))
        {
            SupporttMessage(botMessage);
        }
        else if (text == "/main" || text.StartsWith("главное меню"))
        {
            if(user.CurrentState != null) user.PreviewStates.Push(user.CurrentState);
            user.CurrentState = new MainState();
            await user.CurrentState.SendPage(client, user, ct);
            return true;
        }
        else if (text.StartsWith("/comment"))
        {
            text = text.Replace("/comment", "");
            if (string.IsNullOrWhiteSpace(text))
            {
                await client.SendMessage(user.ChatId, "Сообщение небыло оставлено \U000026A0", cancellationToken: ct);
                return true;

            }
            text = $"Username: {message.Chat.Username} ({message.Chat.FirstName}), date {DateTime.UtcNow.AddHours(5)}, Message: {text}{Environment.NewLine}";
            File.AppendAllText("Comment.txt", text);
            Console.WriteLine(text);
            await client.SendMessage(user.ChatId, "Сообщение успешно оставлено \u2705", cancellationToken: ct);
            return true;

        }
        else return false;

        await client.SendMessage(user.ChatId, botMessage.ToString(), replyMarkup: GetButtonsKeyboard(), replyParameters:
            new ReplyParameters() { ChatId = user.ChatId, MessageId = message.Id }, cancellationToken: ct);
        return true;
    }

    public static void HelpMessage(TelegramStringBuilder botMessage)
    {
        botMessage
                .AddLineStr("Бот хранит состояние диалога, а также сборки и заказы. Для каждого состояния есть список актуальных действий, которые отображаются под сообщением или в списке команд.")
                .AddLineStr("Есть список команд, доступный всегда. Если в результате работы с ботом диалог спутался или возникла ошибка, вы всегда можете ими воспользоваться.")
                .AddLineStr("-------------------------------------------")
                .AddLineStr()
                .AddLineStr("Список команд:")
                .AddLineStr("/start - Запуск (перезапуск) бота")
                .AddLineStr("/help - помощь и список команд")
                .AddLineStr("/main - главное меню")
                .AddLineStr("/support - поддержка")
                .AddLineStr("/comment - отзыв о работе бота");
    }

    public static void AboutMessage(TelegramStringBuilder botMessage)
    {
        botMessage
                .AddLineStr("PCConfigurator - бот который подбирает актульные комплектующие пк в рамках бюджета, учитывая цели клиента")
                .AddLineStr("Для каждой части из комплекта имеется группировка по признакам или тех. характеристиками с усреднёными значениями, по этому бот не даёт 100% гарантии совместимости компонентов")
                .AddLineStr("Для создания собственной сборки выберите команду 'Новая сборка' из главного меню, далее необходимо пройти список вопрос")
                .AddLineStr("После составления сборки она сохранится и вы можете заказать сборку");
    }

    public static void SupporttMessage(TelegramStringBuilder botMessage)
    {
        botMessage
            .AddLineStr("При наличии проблем или вопросов по заказы обращайтесь к - https://t.me/Artem6115")
            .AddLineStr("При наличии проблем с ботом или пожеланий, введите /comment, далее напишите свое обращение");
    }

    public static ReplyMarkup GetBackReplyMarkup()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Назад \U000021A9", "back")
            }
        });
    }

    public static InlineKeyboardButton[] GetBackButton()
    {
        return
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("Назад \U000021A9", "back")
        };

    }

    public static ReplyMarkup GetButtonsKeyboard()
    {

        return new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Главное меню \U0001F31F")
                },
                new[]
                {
                    new KeyboardButton("Информация о боте 📖"),
                },
                new[]
                {
                    new KeyboardButton("Помощь \U0001F64C"),
                    new KeyboardButton("Поддержка \U0001F6E0")
                }
            },
            ResizeKeyboard = true
        };
    }
}
