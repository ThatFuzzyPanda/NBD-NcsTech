﻿using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Labour
    {
        public int ID { get; set; }

        [Display(Name = "Labor Type")]
        public string LabourType { get; set; }
        [Display(Name = "Labor Pirce")]
        public decimal LabourPrice { get; set; }
        [Display(Name = "Labor Cost")]
        public decimal LabourCost { get; set; }

        public ICollection<LabourRequirments> LabourRequirments { get; set; } = new HashSet<LabourRequirments>();
    }
}
