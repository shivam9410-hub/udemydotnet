﻿using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
    var existingBlog=await bloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null; 
        }

        public async Task<IEnumerable<BlogPost>>GetAllAsync()
        {
          return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public  async Task<BlogPost?> GetAsync(Guid id)
        {
      return   await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
          var existingBlog=  await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id==blogPost.Id);

            if (existingBlog != null) { 
                existingBlog.Id= blogPost.Id;   
                existingBlog.Heading    = blogPost.Heading;
                existingBlog.PageTitle=blogPost.PageTitle;  
                existingBlog.Content= blogPost.Content; 
                existingBlog.ShortDescription=blogPost.ShortDescription;
                existingBlog.Author=blogPost.Author;
                existingBlog.FeaturedImageUrl=blogPost.FeaturedImageUrl;    
                existingBlog.UrlHandle=blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible; 
                existingBlog.PublishedDate= blogPost.PublishedDate; 
                existingBlog.Tags=blogPost.Tags;    
               await bloggieDbContext.SaveChangesAsync();
                return existingBlog; 



            }
            return null; 

        }
    }
}
