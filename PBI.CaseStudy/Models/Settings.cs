using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models
{
    public class Settings
    {
        public string Id { get; set; }
        public string Security { get; set; }
        public string File { get; set; }    
        public DateTime InvestedDate { get; set; }
        public int NumberOfshares { get; set; }
    }
}
