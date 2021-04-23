using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UniversityCommittee.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int UserId { get; set; }
        public double Midterm { get; set; }
        public double Endterm { get; set; }
        public double FinalExam { get; set; }
        public double FinalScore{ get; set; }

    }
}