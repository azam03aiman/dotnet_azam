using Microsoft.EntityFrameworkCore;
using Friends_Web_App.Models;

namespace Friends_Web_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options) 
        {
            
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Hobby> Hobby { get; set; }
        public DbSet<Places> Places { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<GroupTask> GroupTask { get; set; }
        public DbSet<BucketList> BucketList { get; set; }
    }
}
