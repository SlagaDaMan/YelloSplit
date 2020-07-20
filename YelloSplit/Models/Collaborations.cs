using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YelloSplit.Models
{

    public class UserDetails
    {
        public string Name { get; set; }

        public string LastName { get; set; }
        public string Credits { get; set; }
        public List<Collaborations> varCollaborations { get; set; }
   

    }

    public class Upload
    {
        public string Name { get; set; }

        public string LastName { get; set; }
        public string Credits { get; set; }
        public List<Category> varCategory { get; set; }
        public List<SubCategory> varSubCategory { get; set; }

    }

    public class Category
    {
     public string Description { get; set; }

    }

    public class SubCategory
    {
        public string Description { get; set; }

    }
    public class Collaborations
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public string Audio { get; set; }
        public string Description { get; set; }

    }
}
