using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KendoUI_Demo.Models
{
    public class PersonVM
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}