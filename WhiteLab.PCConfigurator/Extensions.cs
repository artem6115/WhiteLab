namespace WhiteLab.PCConfigurator;

static internal class Extensions
{
    public static T ThrowIfNull<T>(this T? value)
    {
        return value ?? throw new ArgumentNullException(nameof(value) + " is null");
    }

    public static T ThrowIfDataIsNull<T>(this T? value, string message)
    {
        return value ?? throw new InvalidDataException(message);
    }
}
