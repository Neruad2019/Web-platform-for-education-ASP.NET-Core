using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UniversityCommittee.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string StartTime { get; set; }
        public string WeekDay { get; set; }
    }
}