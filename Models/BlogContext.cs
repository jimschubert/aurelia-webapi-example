using Microsoft.Data.Entity;

namespace AureliaWebApi.Models
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // One way to configure database...
            // var appEnv = CallContextServiceLocator.Locator.ServiceProvider
            //                 .GetRequiredService<IApplicationEnvironment>();
            // optionsBuilder.UseSqlite($"Data Source={ appEnv.ApplicationBasePath }/blog.db");
        }
    }
}