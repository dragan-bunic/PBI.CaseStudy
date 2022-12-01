using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PBI.CaseStudy.Models;
using PBI.CaseStudy.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PBI.CaseStudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHistoricalData<SecurityHistoricData> _securityHistoricData;
        private readonly IStatistic<SecurityStatistic, SecurityHistoricData> _securityStatisticsDataService;
        private readonly List<Settings> _securitiesSettings;
        public HomeController(ILogger<HomeController> logger,
                              IHistoricalData<SecurityHistoricData> securityHistoricData,
                              IStatistic<SecurityStatistic, SecurityHistoricData> securityStatisticsDataService,
                              ISecurity securitySettings)
        {
            _logger = logger;
            _securityHistoricData = securityHistoricData;
            _securityStatisticsDataService = securityStatisticsDataService;
            _securitiesSettings = securitySettings.GetSettingsData();
        }

        public IActionResult Index()
        {
            return View("Index", GetSecuritiesModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<SecurityView> GetSecuritiesModel()
        {
            List<SecurityView> listOfSecurities = new List<SecurityView>();

            foreach (var security in _securitiesSettings)
            {
                var historicalData = _securityHistoricData.GetHistoricalData(security.File);
                var statisticsData = _securityStatisticsDataService.GetStatisticsData(security.Id, historicalData);
                listOfSecurities.Add(new SecurityView(security.Id, security.Security, historicalData, statisticsData));
            }

            return listOfSecurities;
        }
    }
}
