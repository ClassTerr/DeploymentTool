using DeploymentTool.Core.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace DeploymentTool.API.Settings
{
    public class SettingsManager
    {
        private static Settings instance = null;

        public static void Initialize()
        {
            string settingsPath = ConfigurationManager.AppSettings.Get("settingsFilePath");
            ConfigFilePath = HostingEnvironment.MapPath(settingsPath);
        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    Initialize();
                }

                return instance;
            }
            set
            {
                instance = value;
                SaveConfig();
            }
        }

        private static string configFileName = null;
        public static string ConfigFilePath
        {
            get
            {
                return configFileName;
            }
            set
            {
                configFileName = value;
                LoadConfig();
            }
        }

        private static void LoadConfig()
        {
            FileInfo fi = new FileInfo(ConfigFilePath ?? "~/config/settings.config");
            if (fi.Exists)
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
                StreamReader myXmlReader = new StreamReader(ConfigFilePath);
                try
                {
                    instance = (Settings)mySerializer.Deserialize(myXmlReader);
                    myXmlReader.Close();

                    if (!instance.Tokens.Any(x => !x.IsExpired))
                    {
                        instance.Tokens.Add(new Token() { Id = Guid.NewGuid().ToString(), ExpirationDate = DateTime.Now.AddMonths(1) });
                        SaveConfig();
                    }
                }
                catch (Exception e)
                {
                    instance = (Settings)Activator.CreateInstance(typeof(Settings));
                    //MessageBox.Show("LoadConfig Error\n" + e.Message);
                }

                finally
                {
                    myXmlReader.Dispose();
                }
            }
            else
            {
                instance = (Settings)Activator.CreateInstance(typeof(Settings));
                SaveConfig();
            }
        }

        public void CreateSettingsInstance()
        {
            var instance = (Settings)Activator.CreateInstance(typeof(Settings));
            var accessToken = ConfigurationManager.AppSettings.Get("accessToken");

        }

        public static void SaveConfig()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));

            string configDirectory = Path.GetDirectoryName(ConfigFilePath);
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            StreamWriter myXmlWriter = new StreamWriter(ConfigFilePath);
            try
            {
                mySerializer.Serialize(myXmlWriter, instance);
            }
            catch (Exception e)
            {
                //MessageBox.Show("SaveConfig Error\n" + e.Message);
                // TODO : Log error
            }
            finally
            {
                myXmlWriter.Dispose();
            }
        }
    }
}