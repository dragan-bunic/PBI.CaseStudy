using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models
{
    public class SecurityStatistic
    {
        public Statistic MinClose { get; set; }
        public Statistic MaxClose { get; set; }
        public Statistic MaxSpike { get; set; }
        public ROIStatistic ROIMaxClose { get; set; }
        public ROIStatistic ROIMinClose { get; set; }
        public ROIStatistic ROISpikeClose { get; set; }
    }
}
