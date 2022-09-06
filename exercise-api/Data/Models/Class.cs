using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Class : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
