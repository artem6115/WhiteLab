using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.Telegram.DialogStates;

internal class BuildPcConfigtState : IDialogState
{
    public Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        message.Text ??= "";
        if (message.Text.Contains("К предыдущему вопросу"))
        {
            user.CurrentState = user.PreviewState ?? new MainState();
            await user.CurrentState.SendPage(client, user, ct);
            return;
        }

        if (!message.Text.Contains("Собрать по новой"))
        {
            await client.SendMessage(user.ChatId, "Пожалуйста нажмине на кнопку", ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
            return;
        }

        user.CurrentState = new BudgetState();
        await user.CurrentState.SendPage(client, user, ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var cnfBuilder = new PCConfigurator.Core.PCConfigurator();
        var cnfResult = await cnfBuilder.Configure(user.Requirements!, ct);
        if(cnfResult.IsSuccess)
        {
            user.PCAssembly = cnfResult.Config;
            user.CurrentState = new PCConfigurationState();
            user.PreviewState = null;
            await user.CurrentState.SendPage(client, user, ct);
        }
        else
        {
            var msg = cnfResult.ConfigError?.Message ?? "По заданым требованиям не удалось подобрать комплектующие, можете обратится в поддержку или попробовать по новой";
            var str = new TelegramStringBuilder();
            str
                .AddBoldStrHtml("Не удалось создать сборку ❌")
                .AddLineStr()
                .AddLineStr(msg);

            await client.SendMessage(user.ChatId, str.ToString(), ParseMode.Html, replyMarkup: GetButtonsKeyboard(), cancellationToken: ct);
        }
    }

    private ReplyMarkup GetButtonsKeyboard()
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
                    new KeyboardButton("Собрать по новой 🖥")
                },
                new[]
                {
                    new KeyboardButton("К предыдущему вопросу\U000021A9")
                },
            },
            ResizeKeyboard = true
        };
    }
}
