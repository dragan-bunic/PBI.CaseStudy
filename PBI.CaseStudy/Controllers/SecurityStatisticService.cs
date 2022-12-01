using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBI.CaseStudy.Models.Interfaces;
using PBI.CaseStudy.Models;

namespace PBI.CaseStudy.Controllers
{
    public class SecurityStatisticService : IStatistic<SecurityStatistic, SecurityHistoricData>
    {
        private readonly ISecurity _securitySettingsService;
        public SecurityStatisticService(ISecurity securitySettingsService)
        {
            _securitySettingsService = securitySettingsService;
        }

        public SecurityStatistic GetStatisticsData(string id, List<SecurityHistoricData> historicalData)
        {

            var model = new SecurityStatistic();

            model.MaxClose = new Statistic { Value = historicalData.Max(data => data.Close), Date = historicalData.OrderByDescending(data => data.Close).FirstOrDefault().Date };

            model.MinClose = new Statistic { Value = historicalData.Min(data => data.Close), Date = historicalData.OrderBy(data => data.Close).FirstOrDefault().Date };

            var maxSpike = historicalData.FindAll(data => data.PercentChange > 0)?.Max(data => data.Spike);
            model.MaxSpike = maxSpike.HasValue ? new Statistic { Value = maxSpike.Value, Date = historicalData.Find(data => data.Spike == maxSpike.Value).Date } : null;

            //Return on Investment(ROI)
            var securitySettings = _securitySettingsService.GetSettingsData().Find(s => s.Id == id);
           if (securitySettings != null && maxSpike.HasValue)
            {
                int numberOfShares = securitySettings.NumberOfshares;

                double currentInvestmentSpike = historicalData.Find(data => data.Date == model.MaxSpike.Date).Close * numberOfShares;
                double currentInvestmentMax = historicalData.Find(data => data.Date == model.MaxClose.Date).Close * numberOfShares;
                double currentInvestmentMin = historicalData.Find(data => data.Date == model.MinClose.Date).Close * numberOfShares;
                double invested = historicalData.First(data => data.Date >= securitySettings.InvestedDate).Close * numberOfShares;
                double roiSpike = ((currentInvestmentSpike - invested) / invested) * 100;
                double roiMax = ((currentInvestmentMax - invested) / invested) * 100;
                double roiMin = ((currentInvestmentMin - invested) / invested) * 100;
                model.ROISpikeClose = new ROIStatistic { Value = Math.Round(roiSpike, 2), Date = model.MaxSpike.Date, InvestedDate = securitySettings.InvestedDate, NumberOfShares = numberOfShares };
                model.ROIMaxClose = new ROIStatistic { Value = Math.Round(roiMax, 2), Date = model.MaxClose.Date, InvestedDate = securitySettings.InvestedDate, NumberOfShares = numberOfShares };
                model.ROIMinClose = new ROIStatistic { Value = Math.Round(roiMin, 2), Date = model.MinClose.Date, InvestedDate = securitySettings.InvestedDate, NumberOfShares = numberOfShares };

            }
            return model;
        }

    }
}
