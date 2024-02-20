using NuGet.Common;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class StaffPosition
    {
        public int ID { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }
    }
}