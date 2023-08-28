using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

public static class TaskExtensions
{
    public static async Task<TaskEntity> GetFirstOrDefaultAsync(this IQueryable<TaskEntity> query, Guid id,
        Guid? authorId = null)
    {
        if (authorId == null) return await query.FirstOrDefaultAsync(t => t.Id == id);
        return await query.FirstOrDefaultAsync(t => t.Id == id && t.AuthorId == authorId);
    }
}