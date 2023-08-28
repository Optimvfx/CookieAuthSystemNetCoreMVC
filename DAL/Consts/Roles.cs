using DAL.Entities;

namespace DAL.Consts;

public class Roles
{
    public const string UserId = "48497234-123a-4a7a-8a3c-ebbffa7a99c6";

    public static Role User => new Role()
    {
        Id = Guid.Parse(UserId),
        Title = "User",
        Users = new List<User>()
    };
}