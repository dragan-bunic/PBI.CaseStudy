using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models.Interfaces
{
    public interface IStatistic<T1, T2>
    {
        T1 GetStatisticsData(string id, List<T2> historicalData);
    }
}

