using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Student : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
