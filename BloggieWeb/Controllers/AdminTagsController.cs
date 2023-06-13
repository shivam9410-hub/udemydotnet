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
  
            var tag = new Tag
        { Name = addTagRequest.Name,
          DisplayName = addTagRequest.DisplayName
        
          };

            _bloggieDbContext.Tags.Add(tag);
            _bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {   // user dbcontext to read the tags 

         var tags=   _bloggieDbContext.Tags.ToList();
            return View(tags);
        }
    }
}