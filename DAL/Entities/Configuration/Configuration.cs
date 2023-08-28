namespace DAL.Entities.Configuration;

public static class MailConfiguration
{
    public const int MaxLength = 150;
}

public static class NickConfiguration
{
    public const int MaxLength = 50;
    public const int MinLength = 5;
}

public static class RoleConfiguration
{
    public const int MaxNameLength = 100;
    public const int MinNameLength = 1;
}

public static class PasswordConfiguration
{
    public const int MaxLength = 50;
    public const int MinLength = 5;
}