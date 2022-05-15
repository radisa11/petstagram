using Microsoft.EntityFrameworkCore;

namespace petstagram.Models
{
    public class MyContext : DbContext
    {
        
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        
        public DbSet<User> Users {get;set;}
        public DbSet<Post> Posts {get;set;}
        public DbSet<Like> LIkes {get;set;}
    }
}