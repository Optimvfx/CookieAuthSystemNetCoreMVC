using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyInjection
    {
        private static Action<DbContextOptionsBuilder> _emptyUserCustomOptions = builder => {}; 
        
        public static IServiceCollection AddDatabase(this IServiceCollection
            services, Action<DbContextOptionsBuilder>? useCustomOptions = null)
        {
            useCustomOptions = useCustomOptions ?? _emptyUserCustomOptions;
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                useCustomOptions(options);
            });

            return services;
        }
    }
}
