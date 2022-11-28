using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Models.Interfaces
{
    public interface IHistoricalData<T>
    {
        List<T> GetHistoricalData(string csvName);
    }
}
