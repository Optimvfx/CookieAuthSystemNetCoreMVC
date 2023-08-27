namespace DAL;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext dbContext)
    {
        if (dbContext.IsInitialized())
            throw new ArgumentException("Db is already initialized.");
        
        dbContext.Roles.Add(Consts.Roles.User);
        dbContext.SaveChanges();
    }
}