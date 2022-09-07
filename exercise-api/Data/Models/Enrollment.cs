using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Enrollment : Entity
    {
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
    }
}
