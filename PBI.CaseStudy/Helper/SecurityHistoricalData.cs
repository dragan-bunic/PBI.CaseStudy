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
            SetSpikeChangeAndChangePercent();
            return _historicalData;
        }
        protected override SecurityHistoricData GetCSV(string historicDataLine)
        {
            if (string.IsNullOrEmpty(historicDataLine) || !historicDataLine.Contains(Resources.Separator))
            {
                return null;
            }
            try
            {
                var dataValues = GetFormattedHistoricalDataLine(historicDataLine).Split(Resources.Separator);

                var model = new SecurityHistoricData
                {
                    Date = DateTime.Parse(dataValues[(int)HistoricalData.Date], new CultureInfo("en-US")),
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
        private void SetSpikeChangeAndChangePercent()
        {
            for (int i = 0; i < _historicalData.Count; i++)
            {
                var currentData = _historicalData[i];
                if (!currentData.Change.HasValue)
                {
                    if (i == 0)
                    {
                        currentData.Change = 0;
                        currentData.PercentChange = 0;
                    }
                    else
                    {
                        double previousClose = _historicalData[i - 1].Close;
                        currentData.Change = Math.Round(currentData.Close - previousClose, 2);
                        currentData.PercentChange = Math.Round((currentData.Change.Value / previousClose) * 100, 2);

                    }
                }

                currentData.Spike = currentData.High - currentData.Low;
            }
        }

    }
}
