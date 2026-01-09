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
                user.PreviewStates.Clear();
                user.PCAssembly = null;
                user.CurrentState = new BudgetState();
                await user.CurrentState.SendPage(client, user, ct);
                break;
            case "back":
                user.PreviewStates.Clear();
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
        await client.SendMessage(user.ChatId, $"Сборка Успешно создана! ✅{Environment.NewLine}{user.PCAssembly!.Price}", ParseMode.Html, replyMarkup: DefaultState.GetButtonsKeyboard(), cancellationToken: ct);

        var n = Environment.NewLine;
        var msg = new TelegramStringBuilder();
        msg
            .AddBoldStrHtml($"Итоговая стоимость: {user.PCAssembly!.Price}₽{n}")
            .AddLineStr()
            .AddItalicStrHtml("Итоговые комплектующие:" + n);
        foreach (var c in user.PCAssembly!.Components)
        {
            msg
                .AddBoldStrHtml(c.Type + n)
                .AddLineStr("✳" + c.Name)
                .AddLineStr($"Стоимость: {c.Price}₽")
                .AddLineStr(c.Description ?? "");
            if(c.SimilarModels != null && c.SimilarModels.Any()) 
                msg.AddLineStr($"Схожие модели: {c.SimilarModels}{n}");
        }

        var inputPhoto = new InputFileUrl("https://img.pikbest.com/wp/202405/illuminated-neon-pink-computer-in-3d-rendering_9826613.jpg!f305cw");
        await client.SendPhoto(user.ChatId, inputPhoto, caption: msg.ToString(), parseMode: ParseMode.Html, replyMarkup: GetInlineButtons(), cancellationToken: ct);
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
