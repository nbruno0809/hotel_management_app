using HotelManagement.DAL.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Hosting
{
    public static class HostDataExtensions
    {
        public static async Task<IHost> MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using(var scope =  host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();

                var adminCreator = serviceProvider.GetRequiredService<IAdminCreator>();
                await adminCreator.CreateAdminRole();
                await adminCreator.CreateAdminUser();
            }
            return host;
        }
    }
}
