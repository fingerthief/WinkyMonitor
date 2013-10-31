using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Threading;

namespace TestBed
{
    class Weather
    {
        private static XmlDocument condition = new XmlDocument();

        public string GetWeatherXMLAndReturnString(string strZip)
        {
            while (true)
            {
                try
                {
                    condition.Load("http://weather.yahooapis.com/forecastrss?z=" + strZip);

                    // Set up namespace manager for XPath     
                    XmlNamespaceManager NameSpaceMgrCondition = new XmlNamespaceManager(condition.NameTable);
                    NameSpaceMgrCondition.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

                    XmlNodeList NodeCondition = condition.SelectNodes("/rss/channel/item/yweather:condition", NameSpaceMgrCondition);
                    XmlNodeList NodeForcastList = condition.SelectNodes("/rss/channel/item/yweather:forecast", NameSpaceMgrCondition);
                    XmlNodeList NodeUpdated = condition.SelectNodes("/rss/channel/item/title", NameSpaceMgrCondition);

                    string temps = "", now = "", lastUpdated = "", format = "  ", currentCondition = "";

                    temps = NodeForcastList[0].Attributes["high"].InnerText + "°F" + " | " + "Low: " + NodeForcastList[0].Attributes["low"].InnerText + "°F";
                    now = NodeCondition[0].Attributes["temp"].Value;
                    currentCondition = NodeCondition[0].Attributes["text"].Value;
                    lastUpdated = NodeUpdated[0].InnerText;

                    NameSpaceMgrCondition = null;

                    return string.Format(" Now: {2}°F, {0}{1} High: {4}{1} {5}",
                        currentCondition, Environment.NewLine, now, format, temps, lastUpdated.Remove(lastUpdated.Length - 3).Substring(15));

                }
                catch (Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (504) Gateway Timeout.")
                    {
                        // Try again.
                        Thread.Sleep(30000);
                        continue;
                    }
                    return "Loading failed!";
                } 
            }
        }
    }
}
