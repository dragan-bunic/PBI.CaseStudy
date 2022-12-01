using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models
{
    public class SecurityHistoricData
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        public double? Spike { get; set; }
        public double? Change { get; set; }
        public double? PercentChange { get; set; }

    }
}
