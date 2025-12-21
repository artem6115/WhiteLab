using System.Collections.Concurrent;

namespace WhiteLab.Telegram;

public static class UserProvider
{
    public static ConcurrentDictionary<long, UserData> UsersData { get; } = new();
}

