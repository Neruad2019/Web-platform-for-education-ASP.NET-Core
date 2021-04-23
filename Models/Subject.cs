using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UniversityCommittee.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CreditAmount { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Subject()
        {
            Users = new List<User>();
        }
    }
}