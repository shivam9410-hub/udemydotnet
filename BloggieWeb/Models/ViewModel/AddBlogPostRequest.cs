using BloggieWeb.Models.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloggieWeb.Models.ViewModel
{
    public class AddBlogPostRequest
    {

    
        public string Heading { get; set; }

        public string PageTitle { get; set; }

        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeatureImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }
        public bool Visible { get; set; }

        //display tags 
        public  IEnumerable<SelectListItem>Tags{ get; set; }
        //Collect Tags 
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

    }
}
