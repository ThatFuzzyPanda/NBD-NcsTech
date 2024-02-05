using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class LabourRequirments
    {
        public int ID { get; set; }
        [Display(Name = "Hours Worked")]

        public int Hours { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name =" Unit Price" )]
        public double UnitPrice { get; set; }
        [Display(Name = " Extended Price")]
        public double ExtendedPrice { get; set; }

        public int LabourID { get; set; }

        public Labour Labour { get; set; }

    }
}
