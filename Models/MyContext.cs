using Microsoft.EntityFrameworkCore;

namespace CSharpExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}
        public DbSet<User> Users {get;set;}
        public DbSet<Event> Events {get;set;}
        public DbSet<Invite> Invites {get;set;}
    }
}