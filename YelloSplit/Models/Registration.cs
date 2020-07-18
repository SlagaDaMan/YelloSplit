using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YelloSplit.Models
{
    public class Registration
    {
        public string ApplicationType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }

    }
}
