using PBI.CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace PBI.CaseStudy.Helper
{
    public class SecurityHistoricalData : HistoricalData<SecurityHistoricData>
    {
        public SecurityHistoricalData(IWebHostEnvironment webHostEnvironment, ILogger<HistoricalData<SecurityHistoricData>> logger)
        : base(webHostEnvironment, logger)
        {
        }

        public override List<SecurityHistoricData> GetHistoricalData(string csvName)
        {

            LoadHistoricalData(csvName);
            return _historicalData;
        }

        protected override SecurityHistoricData GetCSV(string historicDataLine)
        {
            if (string.IsNullOrEmpty(historicDataLine) || !historicDataLine.Contains(Settings.Separator))
            {
                return null;
            }

            try
            {
                var dataValues = GetFormattedHistoricalDataLine(historicDataLine).Split(Settings.Separator);

                var model = new SecurityHistoricData
                {
                    Date = DateTime.Parse(dataValues[(int)PBI.CaseStudy.Helper.HistoricalData.Date], new CultureInfo("en-US")),
                    Open = GetHistoricValue(dataValues, HistoricalData.Open),
                    High = GetHistoricValue(dataValues, HistoricalData.High),
                    Low = GetHistoricValue(dataValues, HistoricalData.Low),
                    Close = GetHistoricValue(dataValues, HistoricalData.Close),
                    Volume = int.Parse(dataValues[(int)HistoricalData.Volume])
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Problem with convertig csv line '{historicDataLine}' into HistoricDataModel");
                return null;
            }
        }

        private double GetHistoricValue(string[] dataValues, HistoricalData field)
        {
            return double.Parse(dataValues[(int)field]);
        }

        private string GetFormattedHistoricalDataLine(string historicDataLine)
        {
            var volumePattern = Regex.Match(historicDataLine, "\"(.*?)\"").Value;
            if (string.IsNullOrEmpty(volumePattern))
            {
                return historicDataLine;
            }

            var formatedVolume = volumePattern.Replace("\"", "").Replace(",", "");
            return historicDataLine.Replace(volumePattern, formatedVolume);
        }

    }
}
