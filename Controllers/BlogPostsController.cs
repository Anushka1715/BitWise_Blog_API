﻿using BitWiseBlog.API.Models.Domain;
using BitWiseBlog.API.Models.DTO;
using BitWiseBlog.API.Repositories.Implementation;
using BitWiseBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitWiseBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRepository categoryRepository) {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }


        //POST:{apibaseurl}/api/blogposts
        [HttpPost]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            //convert DTO to DOmain model
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach(var categoryGuid in request.Categories) {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

           blogPost = await blogPostRepository.CreateAsync(blogPost);

            //convert domain model back to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok();
        }


        //GET:{apibaseurl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            //Convert Domain Model to DTO
            var response = new List<BlogPostDto>(); 
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }
            return Ok(response);
        }

        //GET:{apibaseurl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id) { 
            //Get the BlogPost from Repo
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if(blogPost == null)
            {
                return NotFound();
            }

            //COnvert Domain model to DTO
            var response = new BlogPostDto
            {

                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };
            return Ok(response);
        }

        //get for the user side blog detail on the clicked blog to read more but based on url handle instead of Id fro improving SEO 
        //GET:{apibaseurl}/api/blogposts/{urlhandle}
        [HttpGet]
        [Route("{urlHandle}")]

        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            //get blogpost details from respository
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost == null)
            {
                return NotFound();
            }

            //COnvert Domain model to DTO
            var response = new BlogPostDto
            {

                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };
            return Ok(response);


        }

        //PUT:{apibaseurl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            //covert DTO to domain model
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

                //Call Repository to update BlogPost Domain Model
                var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

                if(updatedBlogPost == null) {
                    return NotFound();
                }

                //Convert Domain Model to DTO
                var response = new BlogPostDto
                {

                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()

                };

                return Ok(response);


            }


        //DELETE:{apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]


        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

            if (deletedBlogPost == null) {  return NotFound(); }

            //convert domain model to DTO
            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                Title = deletedBlogPost.Title,
                UrlHandle = deletedBlogPost.UrlHandle
            };

            return Ok(response);
        }
    }

   
}



