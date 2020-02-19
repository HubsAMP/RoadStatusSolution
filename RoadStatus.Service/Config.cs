using RoadStatus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Service
{
    public class Config : IConfig
    {
        public Config()
        {
            var settings = ConfigurationManager.AppSettings;
            Url = settings["url"]?.ToString() ?? "";
            AppID = settings["app_id"]?.ToString() ?? "";
            AppKey = settings["app_key"]?.ToString() ?? "";
        }

        public string Url { get; set; }
        public string AppID { get; set; }
        public string AppKey { get; set; }
    }
}