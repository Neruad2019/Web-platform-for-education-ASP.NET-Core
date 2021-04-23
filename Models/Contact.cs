using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UniversityCommittee.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "'Email' field required")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }
        public string Message { get; set; }
        [Required(ErrorMessage = "'Name' field required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "'Surname' field required")]
        public string Surname { get; set; }
        [Required]
        public string Phone_Number { get; set; }
      
    }
}