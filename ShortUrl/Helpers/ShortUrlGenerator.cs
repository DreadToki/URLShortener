namespace ShortUrl.Helpers;

public static class ShortUrlGenerator
{
    private const string charset = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static readonly Random random = new();

    public static string Generate(int length)
    {
        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = charset[random.Next(charset.Length)];
        }
        return new string(chars);
    }
}