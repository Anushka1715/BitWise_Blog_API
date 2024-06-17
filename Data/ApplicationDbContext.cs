using BitWiseBlog.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BitWiseBlog.API.Data
{
    public class ApplicationDbContext:DbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<BlogImage> BlogImages { get; set; }

    }
}

//we can interact with our data base from application with this and the properties will be taken from our model
