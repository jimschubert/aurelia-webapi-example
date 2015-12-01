using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;

namespace AureliaWebApi.Models
{
    public class BlogContext : DbContext
    {   
        public BlogContext() { }
        public BlogContext(IServiceProvider services, DbContextOptions options) : base(services, options) { }
        
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // One way to configure database...
            // var appEnv = CallContextServiceLocator.Locator.ServiceProvider
            //                 .GetRequiredService<IApplicationEnvironment>();
            // optionsBuilder.UseSqlite($"Data Source={ appEnv.ApplicationBasePath }/blog.db");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>(p =>
            {
                p.HasKey(_ => _.PostId);
                p.Property(_ => _.PostId).ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}