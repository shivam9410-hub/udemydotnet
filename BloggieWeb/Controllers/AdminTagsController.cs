using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BloggieWeb.Controllers
{
    public class AdminTagsController : Controller
    {

        private BloggieDbContext _bloggieDbContext;
        public AdminTagsController(BloggieDbContext bloggieDbContext) {
          
        _bloggieDbContext=bloggieDbContext;
       
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
       
        }

        [HttpPost]

        [ActionName("Add")]
        public IActionResult
            Add(AddTagRequest addTagRequest)
        {
            //mapping addtorequst to tag domain model
            var tag = new Tag
        { Name = addTagRequest.Name,
          DisplayName = addTagRequest.DisplayName
        
          };

            _bloggieDbContext.Tags.Add(tag);
            _bloggieDbContext.SaveChanges();

            return View("Add");
        }
    }
}