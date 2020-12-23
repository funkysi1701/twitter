using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Twitter.Model
{
    public class AuthContext : IdentityDbContext
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// AuthContext
        /// </summary>
        public AuthContext()
        {
        }

        /// <summary>
        /// AuthContext
        /// </summary>
        /// <param name="config"></param>
        public AuthContext(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// AuthContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="config"></param>
        public AuthContext(DbContextOptions<AuthContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        /// <summary>
        /// AuthContext
        /// </summary>
        /// <param name="options"></param>
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// User
        /// </summary>
        public virtual DbSet<User> User { get; set; }

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("Auth"));
            }
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.Env, e.Username })
                    .HasName("PK_Auth");

                entity.Property(e => e.Env).HasMaxLength(10);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TokenExpires).HasColumnType("datetime");
            });
        }
    }
}
