using BitWiseBlog.API.Data;
using BitWiseBlog.API.Models.Domain;
using BitWiseBlog.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BitWiseBlog.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }


        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
          return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
           return await dbContext.BlogPosts.Include(x=> x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
           var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if(existingBlogPost == null) {
                return null;
            }

            //Update BlogPost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }


        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var exsistingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(exsistingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(exsistingBlogPost);
                await dbContext.SaveChangesAsync();
                return exsistingBlogPost;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await dbContext.BlogPosts.Include(x=> x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);

        }
    }
}
