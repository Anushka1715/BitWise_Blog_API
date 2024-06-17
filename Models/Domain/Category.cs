namespace BitWiseBlog.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }//creating many to many relationship that one category can have multiple blogs
                                                            
    }
}
