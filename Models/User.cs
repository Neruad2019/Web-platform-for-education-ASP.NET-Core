using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UniversityCommittee.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "'Email' field required")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "'Name' field required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "'Surname' field required")]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        public string PictureURL { get; set; }
        public int? GroupId { get; set; }

        public Group Group { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public User()
        {
            Subjects = new List<Subject>();
        }

    }
}