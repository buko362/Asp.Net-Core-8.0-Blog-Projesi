using Microsoft.AspNetCore.Http; //eklemeyi unutma yoksa IformFile diğer tarafta hata verir
namespace CoreDemo.Models
{
    public class UserUpdateViewModel
    {
        public string namesurname{ get; set; }
        public string username{ get; set; }
        public string mail{ get; set; }
        public string? imageurl{ get; set; }
        public IFormFile? imagefile{ get; set; }
        public string? password { get; set; }
        public bool ChangePassword { get; set; }
        public string? about { get; set; }
    }
}
