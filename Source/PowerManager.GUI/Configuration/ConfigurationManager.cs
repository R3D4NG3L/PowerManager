using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace PowerManager.GUI
{
    /// <summary>
    /// Configuration Manager
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public Configuration Configuration;

        /// <summary>
        /// Configuraiton Path
        /// </summary>
        private string _configurationPath;

        /// <summary>
        /// Configuration File Name
        /// </summary>
        private const string _configurationFileName = "Configuration.xml";

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationManager()
        {
            // Configurations never taken 
            // Get App Data Path
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Set Configuration Folder Path
            _configurationPath = $"{appDataPath}\\GTechSolutions\\PowerManager";
            // Create Folder if doesnt exists
            var dir = Directory.CreateDirectory(_configurationPath);
            _configurationPath += $"\\{_configurationFileName}";

            // Try to get saved configuarations
            try { Get(); }
            catch
            {
                // There is not a configuration, create a new empty one
                Configuration = new Configuration();
                Save();
            }

            // Subscribe to Events
            Configuration.ValueChanged += (o, e) => Save();
        }
        /// <summary>
        /// Get Configuration
        /// </summary>
        /// <returns></returns>
        private void Get()
        {
            using (StreamReader sr = new StreamReader(_configurationPath))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                using (var reader = XmlReader.Create(_configurationPath))
                {
                    Configuration = (Configuration)serializer.Deserialize(reader);
                }
            }
        }
        
        /// <summary>
        /// Save currnt Configuration
        /// </summary>
        private void Save()
        {
            // Save Current Configuration
            using (FileStream fs = new FileStream(_configurationPath, FileMode.Create))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(Configuration));
                xSer.Serialize(fs, Configuration);
            }
        }

    }
}
