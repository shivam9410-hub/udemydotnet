using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModel;
using BloggieWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BloggieWeb.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly  IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository) {

            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;   
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.ToString() })

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription=addBlogPostRequest.ShortDescription,   
                FeatureImageUrl= addBlogPostRequest.FeatureImageUrl,
                UrlHandle =addBlogPostRequest.UrlHandle,
                PublishedDate= addBlogPostRequest.PublishedDate,    
                Author=addBlogPostRequest.Author,   
                Visible=addBlogPostRequest.Visible, 

            };
            var selectedTags=new List<Tag>();
            
            foreach(var selectedTagId in addBlogPostRequest.SelectedTags){
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
               var existingTag= await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            //maping tags back to domain model
            blogPost.Tags=selectedTags;
            await blogPostRepository.AddAsync(blogPost);



            return RedirectToAction("Add");
            
        }
    
        public async Task<IActionResult> List()
        { var blogPosts= await blogPostRepository.GetAllAsync();    

            return View(blogPosts);
        }
    
    }
}
