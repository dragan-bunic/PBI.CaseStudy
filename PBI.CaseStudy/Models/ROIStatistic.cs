using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models
{
    public class ROIStatistic : Statistic
    {
        public DateTime InvestedDate { get; set; }
        public int NumberOfShares { get; set; }
    }
}
