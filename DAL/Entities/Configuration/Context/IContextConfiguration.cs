using Microsoft.EntityFrameworkCore;

namespace DAL.Entities.Configuration.Context;

public interface IContextConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
    where TEntity : class
{
    
}