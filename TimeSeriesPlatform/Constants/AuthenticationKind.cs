namespace Iiroki.TimeSeriesPlatform.Constants;

public static class AuthenticationKind
{
    public const string Admin = "admin";
    public const string Reader = "reader";
    public const string Integration = "integration";

    public const string ReaderOrAdmin = $"{Reader}, {Admin}";
}
