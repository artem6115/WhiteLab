using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using WhiteLab.Telegram;

namespace WhiteLab.Bot;

internal class Program
{
    static void Main(string[] args)
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
#endif

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Telegram token wasn`t given");
        }
        var opt = new ReceiverOptions();// choose current update types
        var bot = new TelegramBotClient(token);
        bot.StartReceiving<TelegramRecipient>(opt, source.Token);
        Console.ReadLine();
    }

    private static void Exit(CancellationTokenSource source)
    {
        if (!source.IsCancellationRequested)
        {
            source.Cancel();
            Thread.Sleep(1500);
        }
    }

    private static void UnhandleError(object sender, UnhandledExceptionEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.ToString());
    }
}
