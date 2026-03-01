 using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using WhiteLab.Telegram;

namespace WhiteLab.Bot;

internal class Program
{
    private static TelegramBotClient? _bot;
    static async Task Main(string[] args)
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
        await Execute(source.Token);
    }

    public static async Task Execute(CancellationToken ct)
    {
        while(!ct.IsCancellationRequested)
        {
            try
            {
                StartTg(ct);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.ToString());
                Console.ResetColor();
            }
            finally
            {
                await StopTg(ct);
                await Task.Delay(10000, ct);
            }

        }
        
    }

    public static void StartTg(CancellationToken ct)
    {
        var opt = new ReceiverOptions() { AllowedUpdates = [UpdateType.CallbackQuery, UpdateType.Message] };
        _bot = new TelegramBotClient(TelegramOptions.Token);
        _bot.StartReceiving<TelegramRecipient>(opt, ct);
        Console.WriteLine("Start bot");
    }

    public async static Task StopTg(CancellationToken ct)
    {
        try
        {
            if (_bot != null) await _bot.DeleteWebhook(false, ct);
            Console.WriteLine("Stop bot");
        }
        catch { }
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
