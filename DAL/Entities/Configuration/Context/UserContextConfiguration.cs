using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Entities.Configuration.Context;

public class UserContextConfiguration : IContextConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Nick);
        builder.HasIndex(user => user.Email);
    }
}