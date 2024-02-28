using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
	public class Project : IValidatableObject
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "You must enter a bid date and time.")]
		[Display(Name = "Bid Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime BidDate { get; set; } = DateTime.Today; //Setting the bid date to current date

		[Display(Name = "Project Site")]
		[Required(ErrorMessage = "You must enter project site name.")]
		[StringLength(60, ErrorMessage = "Project site cannot be more than 60 characters long.")]
		public string ProjectSite { get; set; }

		[Required(ErrorMessage = "You must enter a est. begin date and time.")]
		[Display(Name = "Est. Begin Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime Est_BeginDate { get; set; } = DateTime.Today.Date.AddDays(7); //Setting the begin date to 1 week after

		[Required(ErrorMessage = "You must enter a est. complete date.")]
		[Display(Name = "Est. Complete Date")]
		[DataType(DataType.Date)]

		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime Est_CompleteDate { get; set; }

		[Required(ErrorMessage = "You must enter the bid amount.")]
		[Display(Name = "Bid Amount")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[DataType(DataType.Currency)]
		public double BidAmount { get; set; }

		//foreign keys
		[Display(Name = "Organization Name")]
		[Required(ErrorMessage = "You must select a Client.")]
		public int ClientId { get; set; }
		public Client Client { get; set; }

		[Display(Name = "Staffs")]
		public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();

		[Display(Name = "Design Bids")]
		public ICollection<DesignBid> DesignBids { get; set; } = new HashSet<DesignBid>();

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Est_BeginDate < BidDate)
			{
				yield return new ValidationResult("Estimated begin date cannot be before bid date.", new[] { "Est_BeginDate" });
			}
			if (Est_CompleteDate < Est_BeginDate)
			{
				yield return new ValidationResult("Estimated completion date cannot be before estimated begin date.", new[] { "Est_CompleteDate" });
			}
		}
	}
}


