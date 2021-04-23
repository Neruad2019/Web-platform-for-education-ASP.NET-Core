using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UniversityCommittee.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string CafedreName { get; set; }
        public string GroupName { get; set; }

        public ICollection<User> Users { get; set; }
        public Group()
        {
            Users = new List<User>();
        }
    }
}