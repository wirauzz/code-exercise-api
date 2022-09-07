using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models
{
    public class EnrollmentDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
