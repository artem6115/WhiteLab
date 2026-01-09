using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using WhiteLab.Telegram;

namespace WhiteLab.Bot;

internal class Program
{
    static int Main(string[] args)
    {
        var source = new CancellationTokenSource();
        Console.CancelKeyPress += (s, o) => Exit(source);
        AppDomain.CurrentDomain.ProcessExit += (s, o) => Exit(source);
        AppDomain.CurrentDomain.UnhandledException += UnhandleError;
        string? token = null;
#if DEBUG
    var secretPath = "C:\\Users\\mikov_6gmdl0l\\AppData\\Roaming\\Microsoft\\UserSecrets\\TelegramSecret\\secrets.txt";
        if (File.Exists(secretPath))
        {
            token = File.ReadAllText(secretPath);
        }
#endif
#if !DEBUG
    if(Environment.GetEnvironmentVariables().Contains("token"))
    {
        token = Environment.GetEnvironmentVariables()["token"]?.ToString();
    }
    if(token == null && args.Length > 0)
    {
        token = args[0].Split("=").Last();
    }
#endif

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Telegram token wasn`t given");
        }

        token = token.
             Trim()                                    // Пробелы по краям
            .Replace("\uFEFF", "")                     // BOM  
            .Replace("\u200B", "")                     // Zero-Width Space
            .Replace("\r", "").Replace("\n", "");      // Лишние переносы
        TelegramOptions.Token = token;
        var opt = new ReceiverOptions() { AllowedUpdates = [UpdateType.CallbackQuery, UpdateType.Message] };
        var bot = new TelegramBotClient(token);
        bot.StartReceiving<TelegramRecipient>(opt, source.Token);
        Console.WriteLine("Start bot");
        Console.ReadLine();
        return 0;
    }

    private static void Exit(CancellationTokenSource source)
    {
        source.Cancel();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Stopping...");
        Console.ResetColor();
        Thread.Sleep(1500);
    }

    private static void UnhandleError(object sender, UnhandledExceptionEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.ToString());
    }
}
