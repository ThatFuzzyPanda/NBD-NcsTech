using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.ViewModels
{
    public class UserVM
    {
        public string ID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
		[Display(Name = "Password")]
		public string Password { get; set; }


		[Display(Name = "Roles")]
        public List<string> UserRoles { get; set; }
    }
}
