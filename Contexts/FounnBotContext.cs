using FounnBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace FounnBot.Contexts{
    public class FounnBotContext : DbContext{
        private readonly string ConnectionString;

        public DbSet<Guild> Guilds {get; set;}
        public DbSet<TrustedPerson> TrustedPersons {get; set;}
        public DbSet<User> Users {get; set;}

        public FounnBotContext(string connectionString)
        {
            this.ConnectionString = connectionString;   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
            => opt.UseMySql(this.ConnectionString);
    }
}