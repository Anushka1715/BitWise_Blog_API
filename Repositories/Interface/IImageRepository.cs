﻿using BitWiseBlog.API.Models.Domain;
using System.Net;

namespace BitWiseBlog.API.Repositories.Interface
{
    public interface IImageRepository
    {
       Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
