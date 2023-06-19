using BloggieWeb.Data;
using BloggieWeb.Models.Domain;

namespace BloggieWeb.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext) 
        {
            this.bloggieDbContext = bloggieDbContext;
            
        }
        public async Task<BlogPost> AddAsync(BlogPost blogpost)
        {
            await bloggieDbContext.AddAsync(blogpost);
            await bloggieDbContext.SaveChangesAsync();  
            return blogpost;    
            
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
