using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class SelectProgramsState : IDialogState
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

        if (message.Text.Contains("Пропустить"))
        {
            user.PreviewStates.Push(this);
            user.CurrentState = new OverclockingSupportState();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        user.Requirements!.Programs = message.Text.Split(',').ToList();
        var programsNorm = await PCConfigurator.Core.PCConfigurator.GetNormilizeProgramsName(user.Requirements!.Programs, ct);
        if (programsNorm.Count > 0)
        {
            await client.SendMessage(user.ChatId, $"Будут учтены программы:{Environment.NewLine}{string.Join(Environment.NewLine, programsNorm)}", cancellationToken: ct);
            await Task.Delay(2000);
        }
        user.PreviewStates.Push(this);
        user.CurrentState = new OverclockingSupportState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var str = new TelegramStringBuilder();
        str
            .AddItalicStrHtml("Шаг 6/n ✅")
            .AddLineStr()
            .AddBoldStrHtml("Выбор конкретных программ")
            .AddLineStr()
            .AddLineStr($"Напиши через запятую конкретные игры или программы (пример: Unity, Unreal Engine, cs2, dota 2){Environment.NewLine}Известные сокращения распознаются cs -> Counter Strice 2, но лучше писать целиком{Environment.NewLine}Если некторых программ нету, а они были указаны, значит бот не имеет в базе данную программу или она была написана с ошибкой");

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
                    new KeyboardButton("Пропустить"),
                    new KeyboardButton("Назад \U000021A9")
                },
            },
            ResizeKeyboard = true
        };
    }
}
