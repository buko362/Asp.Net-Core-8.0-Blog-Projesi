using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDemo.Models
{
    public class ViewModel
    {
        public string Sehir { get; set; }
        public IList<SelectListItem> Sehirler { get; set; }

        public ViewModel() //constructor
        {
            Sehirler = new List<SelectListItem>();
        }
    }
 
}
