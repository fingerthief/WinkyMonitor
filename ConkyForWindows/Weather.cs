using System;
using System.Xml;
using System.Text.RegularExpressions;
using Winky.Properties;

namespace WeatherRSS
{
    class Weather
    {

        private string location = Settings.Default.textboxLocation;
        
        public string CurrentConditions()
        {
            string city = "";
            string weather = "";

            // http://weather.yahooapis.com/forecastrss?w=2464601
            
            // conditions
            // Create a new XmlDocument  
            XmlDocument condition = new XmlDocument();

            // Load data  
            condition.Load(location);
            
            // Set up namespace manager for XPath  
            XmlNamespaceManager NameSpaceMgrCondition = new XmlNamespaceManager(condition.NameTable);
            XmlNamespaceManager NameSpaceMgrCity = new XmlNamespaceManager(condition.NameTable);
            NameSpaceMgrCondition.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            NameSpaceMgrCity.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // Get forecast with XPath   
            XmlNodeList nodes = condition.SelectNodes("/rss/channel/item/yweather:condition", NameSpaceMgrCondition);
            XmlNodeList nodess = condition.SelectNodes("/rss/channel/yweather:location", NameSpaceMgrCity);

            foreach (XmlNode node in nodess)
            {
                city =  ("Location: " +
                     node.Attributes["city"].InnerText);
            }
           

            // To get forcasted high
            string temps = forcastTemps();

            foreach (XmlNode node in nodes)
            {
                
                weather = ( city + "\n" + "Weather: " +
                                    node.Attributes["text"].InnerText + "\n" +
                                    "Now: " +
                                    node.Attributes["temp"].InnerText + "\n" +
                                    "High: " +
                                    temps + "\n" +
                                    "Last Updated: " +      
                                   node.Attributes["date"].InnerText);       
            }
      
            return weather;
        }

        public string forcastTemps()
        {
            string weather = "";

            // Create a new XmlDocument  
            XmlDocument docc = new XmlDocument();

            // Load data  
            docc.Load(location);

            // Set up namespace manager for XPath  
            XmlNamespaceManager forcastNameSpaceMgr = new XmlNamespaceManager(docc.NameTable);
            forcastNameSpaceMgr.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // Get forecast with XPath  
            XmlNodeList nodess = docc.SelectNodes("/rss/channel/item/yweather:forecast", forcastNameSpaceMgr);
            int i = 0;
            foreach (XmlNode node in nodess)
            {
                if (i == 0)
                    weather = node.Attributes["high"].InnerText + "\nLow: " + node.Attributes["low"].InnerText;
                i++;
            }
            return weather;
        }

        private string Forcast()
        {
            string weather = "";

            // Create a new XmlDocument  
            XmlDocument docc = new XmlDocument();

            // Load data  
            docc.Load(location);

            // Set up namespace manager for XPath  
            XmlNamespaceManager forcastNameSpaceMgr = new XmlNamespaceManager(docc.NameTable);
            forcastNameSpaceMgr.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // Get forecast with XPath  
            XmlNodeList nodess = docc.SelectNodes("/rss/channel/item/yweather:forecast", forcastNameSpaceMgr);

            weather += "\nForcast";
            foreach (XmlNode node in nodess)
            {
                weather += ("\n" +
                                    node.Attributes["day"].InnerText + "\n" +
                                    "The Condition will be " +
                                    node.Attributes["text"].InnerText + "\n" +
                                    "High: " +
                                    node.Attributes["high"].InnerText + "\n" +
                                    "Low: " +
                                    node.Attributes["low"].InnerText);
            }
            return weather;
        }

        public string getImage()
        {

            // Create a new XmlDocument  
            XmlDocument doc = new XmlDocument();

            // Load data  
            doc.Load(location);

            // Set up namespace manager for XPath  
            XmlNamespaceManager ImageNameSpaceMgr = new XmlNamespaceManager(doc.NameTable);
            ImageNameSpaceMgr.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // Get forecast with XPath  
            XmlNodeList nodess = doc.SelectNodes("/rss/channel/item/description", ImageNameSpaceMgr);


            string xml = nodess[0].InnerXml;
            string desiredValue = Regex.Replace(xml
                                           .Replace("<br />", "\n")
                                           .Trim(),
                    @"\<br />", "\n");
            desiredValue = Regex.Replace(desiredValue
                                           .Replace("<br />", "\n")
                                           .Trim(),
                    @"\<BR />", "\n");

            desiredValue = desiredValue.Remove(57);
            desiredValue = desiredValue.Remove(0, 20);

            return desiredValue;
        }
    }
}