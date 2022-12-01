using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBI.CaseStudy.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using PBI.CaseStudy.Helper;
using PBI.CaseStudy.Models.Interfaces;
using System.Xml.Serialization;

namespace PBI.CaseStudy.Models
{
    public class SecuritySettings : ISecurity
    {
        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly ILogger<SecuritySettings> _logger;
        private List<Settings> _securitySettings;

        public SecuritySettings(IWebHostEnvironment webHostEnvironment, ILogger<SecuritySettings> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public List<Models.Settings> GetSettingsData()
        {
            if (_securitySettings == null)
            {
                LoadData();
            }
            return _securitySettings;
        }

        private void LoadData()
        {
            string webRootPath = _webHostEnvironment.ContentRootPath;
            var securitiesFile = Path.Combine(webRootPath, Resources.HelperFolder, Resources.SecuritiesFile);
            try
            {
                //var mySerializer = new XmlSerializer(typeof(List<Settings>));
                //using var myFileStream = new FileStream(securitiesFile, FileMode.Open);
                //_securitySettings = (List<Settings>)mySerializer.Deserialize(myFileStream);
                _securitySettings = JsonConvert.DeserializeObject<List<Settings>>(File.ReadAllText(securitiesFile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not load file '{securitiesFile}'!");
            }

        }
    }
}
