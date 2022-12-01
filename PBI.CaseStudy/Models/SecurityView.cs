using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models
{
    public class SecurityView
    {
        public string Id { get; private set; }
        public string SecurityName { get; private set; }
        public List<SecurityHistoricData> HistoricData { get; private set; }
        public SecurityStatistic SecurityStatistic { get; private set; }
        //public List<object> HistoricalChartData { get; private set; }
        public SecurityView(string id,
                             string securityName,
                             List<SecurityHistoricData> historicData,
                             SecurityStatistic securityStatistic//,
                             /*List<object> historicalChartData*/)
        {
            Id = id;
            SecurityName = securityName;
            HistoricData = historicData;
            SecurityStatistic = securityStatistic;
            //HistoricalChartData = historicalChartData;

        }
    }
}
