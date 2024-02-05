using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
	public class Client 
	{

		public int ID { get; set; }

		[Display(Name = "Name")]
		[Required(ErrorMessage = "You cannot leave the first name blank.")]
		[StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
		public string Name { get; set; }

		[Display(Name = "Contact")]
		[Required(ErrorMessage = "You cannot leave the contact name blank.")]
		[StringLength(50, ErrorMessage = "Contact name cannot be more than 50 characters long.")]
		public string ContactPerson { get; set; }

		[Display(Name = "Phone Number")]
		[Required(ErrorMessage = "You cannot leave the phone number blank.")]
		[RegularExpression("^\\d{10}$", ErrorMessage = "The phone number must be exactly 10 numeric digits.")]
		[DataType(DataType.PhoneNumber)]
		[StringLength(10)]
		public string Phone { get; set; }

		[Display(Name = "Street Name")]
		//[Required(ErrorMessage = "You cannot leave the street name blank.")]
		[StringLength(50, ErrorMessage = "Street name cannot be more than 50 characters long.")]
		public string Street { get; set; }  

        [Display(Name = "City")]
		[Range(1,int.MaxValue,ErrorMessage ="You must select a city.")]
        public int? CityID { get; set; }
		public City City { get; set; }

        [Display(Name = "Postal Code")]
		[StringLength(7, ErrorMessage = "Postal code cannot be more than 6 characters long.")]
		[RegularExpression("^[A-Za-z]\\d[A-Za-z] \\d[A-Za-z]\\d$", ErrorMessage = "Invalid postal code.")]
		public string PostalCode { get; set; }

		public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

		
	}
}

