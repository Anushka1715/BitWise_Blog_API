using BitWiseBlog.API.Data;
using BitWiseBlog.API.Models.Domain;
using BitWiseBlog.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BitWiseBlog.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext) {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
           return await dbContext.BlogImages.ToListAsync();

        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1- Upload the Image to API/Images folder
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath,FileMode.Create);
            await file.CopyToAsync(stream);

            //2-Update the database , insert details of Image
            //https://bitwiseblog.com/images/somefilename.jpg //thw url we want to construct
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();

            return blogImage;   
        }
    }
}
