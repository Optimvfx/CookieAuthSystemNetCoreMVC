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
    public const int MaxTitleLength = 100;
    public const int MinTitleLength = 1;
}

public static class PasswordConfiguration
{
    public const int MaxLength = 50;
    public const int MinLength = 5;
}

public static class TaskConfiguration
{
    public const int TitleMaxLength = 50;
    public const int TitleMinLength = 5;
    
    public const int DescriptionMaxLength = 1250;
    public const int DescriptionMinLength = 0;
}