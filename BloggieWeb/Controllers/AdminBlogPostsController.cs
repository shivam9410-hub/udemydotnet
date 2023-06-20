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
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })

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
                FeaturedImageUrl= addBlogPostRequest.FeaturedImageUrl,
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


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ///retriev the result from the repository ; 
            ///

            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await  tagRepository.GetAllAsync();

            //map the domain model into the view model 

            if (blogPost != null)
            {

         
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.PageTitle,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name
                        ,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model back to domain model 
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,

            };
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags) { 

                if(Guid.TryParse(selectedTag,out var tag))
                {
            var foundTag=await tagRepository.GetAsync(tag);
                     if(foundTag != null) {

                        selectedTags.Add(foundTag);
                    }   


                }


            }

        
            blogPostDomainModel.Tags= selectedTags;

         var updatedBlog =     await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                //show success notification ;
                return RedirectToAction("Edit");   
            }
            else
            {
                return RedirectToAction("Edit");
            }
         
            //submit information to repository to update 
            //redirect to gete 
        }

        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
      var    deletedBlogPost=   await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            /// show error notification ; 
            /// 
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }
    }
}
