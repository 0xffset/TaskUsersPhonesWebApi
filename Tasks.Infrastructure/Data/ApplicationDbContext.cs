using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Taks.Core.Entities.General;
using Tasks.Core.Entities.General;


namespace Tasks.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {

        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region DbSets
        public DbSet<User> Users { get; set; }  
        public DbSet<Phone> Phones { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var connectionString = "Data Source=0xffset\\SQLEXPRESS;Initial Catalog=WebApiUserManager;Integrated Security=True;Trust Server Certificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
                
            }


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationDbContextConfigurations.Configure(builder);
        }

        
    }
}
