using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PBI.CaseStudy.Helper;
using PBI.CaseStudy.Models.Interfaces;

public abstract class HistoricalData<T> : IHistoricalData<T>
{
    protected readonly IWebHostEnvironment _webHostEnvironment;
    protected readonly ILogger<HistoricalData<T>> _logger;
    protected List<T> _historicalData;
    private static object lockObject = new object();

    public HistoricalData(IWebHostEnvironment webHostEnvironment, ILogger<HistoricalData<T>> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
    protected abstract T GetCSV(string line);

    public abstract List<T> GetHistoricalData(string csvName);

    protected void LoadHistoricalData(string csvName)
    {
        string currentRootPath = _webHostEnvironment.ContentRootPath;
        string dataPath = Path.Combine(currentRootPath, Settings.DataImportFolder, csvName);

        if (!File.Exists(dataPath))
        {
            _logger.LogError($"File'{dataPath}' doesn't exist!");
            return;
        }

        try
        {
            lock (lockObject)
            {
                _historicalData = File.ReadAllLines(dataPath)
                                        .Skip(1)
                                        .Select(data => GetCSV(data))
                                        .Where(data => data != null)
                                        .ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Couldn't laod file: '{dataPath}'!");
        }
    }

}
