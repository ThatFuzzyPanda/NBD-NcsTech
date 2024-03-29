﻿using MedicalOffice.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NBDProjectNcstech.Models
{
    public class Client: Auditable
    {
        public string FormatedPhoneNumber
        {
            get
            {
                return Regex.Replace(Phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }
        }
        public string ContactPerson { get { return $"{ContactPersonFirst} {ContactPersonLast}";} }

        public int ID { get; set; }

        [Display(Name = "Organization Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Contact Person First Name")]
        [Required(ErrorMessage = "You cannot leave the contact first name blank.")]
        [StringLength(50, ErrorMessage = "Contact first name cannot be more than 50 characters long.")]
        public string ContactPersonFirst { get; set; }

		[Display(Name = "Contact Person Last Name")]
		[Required(ErrorMessage = "You cannot leave the contact last name blank.")]
		[StringLength(50, ErrorMessage = "Contact last  name cannot be more than 50 characters long.")]
		public string ContactPersonLast { get; set; }

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
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        public int? CityID { get; set; }
        public City City { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(7, ErrorMessage = "Postal code cannot be more than 6 characters long.")]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z] \\d[A-Za-z]\\d$", ErrorMessage = "Invalid postal code.")]
        public string PostalCode { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();


    }
}

