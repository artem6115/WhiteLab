using System.Text.Encodings.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhiteLab.Telegram.DialogStates;

internal class PCConfigurationState : IDialogState
{
    public async Task AcceptcCallback(ITelegramBotClient client, CallbackQuery callback, UserData user, CancellationToken ct)
    {
        switch (callback.Data)
        {
            case "new asm":
                user.PreviewState = null;
                user.PCAssembly = null;
                user.CurrentState = new BudgetState();
                await user.CurrentState.SendPage(client, user, ct);
                break;
            case "back":
                user.PreviewState = null;
                user.CurrentState = new MainState();
                await user.CurrentState.SendPage(client, user, ct);
                break;
        }
    }

    public async Task AcceptcMessage(ITelegramBotClient client, Message message, UserData user, CancellationToken ct)
    {
        await client.SendMessage(user.ChatId, "Выберите действие", ParseMode.Html, replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);
    }

    public async Task SendPage(ITelegramBotClient client, UserData user, CancellationToken ct)
    {
        var id = (await client.SendMessage(user.ChatId, $"Сборка готова ✅{Environment.NewLine}{user.PCAssembly!.Price}", ParseMode.Html, replyMarkup:  DefaultState.GetButtonsKeyboard(), cancellationToken: ct)).Id;
        await client.EditMessageText(user.ChatId, id, $"Сборка готова ✅{Environment.NewLine}{user.PCAssembly!.Price}", ParseMode.Html, replyMarkup: GetInlineButtons(), cancellationToken: ct);
    }

    private InlineKeyboardMarkup GetInlineButtons()
    {
        var msg = UrlEncoder.Default.Encode("Привет, хочу собрать такую сборку🖥" + Environment.NewLine + "Соберешь ?)");
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Новая сборка 🖥", "new asm"),
            },
            new []
            {
                InlineKeyboardButton.WithUrl("Заказать сборку",$"https://t.me/Artem6115?text={msg}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Назад \U000021A9", "back")
            },

        });
    }
}
