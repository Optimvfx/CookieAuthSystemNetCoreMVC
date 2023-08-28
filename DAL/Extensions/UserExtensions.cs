using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

public static class UserExtensions
{
    public static async Task<User> GetFirstOrDefaultByEmailFilterAsync(this IQueryable<User> query, string? email)
    {
        if (email != null) return await query.FirstOrDefaultAsync(x => x.Email.ToUpper().Contains(email.ToUpper()));
        return await query.FirstOrDefaultAsync();
    }

    public static async Task<User> GetFirstOrDefaultByNickFilterAsync(this IQueryable<User> query, string? nick)
    {
        if (nick != null) return await query.FirstOrDefaultAsync(x => x.Nick.ToUpper().Contains(nick.ToUpper()));
        return await query.FirstOrDefaultAsync();
    }
}
