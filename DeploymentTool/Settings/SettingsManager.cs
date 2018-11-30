using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeploymentTool.Settings
{
    public class SettingsManager
    {
        private static Settings instance = null;

        public static void Initialize()
        {
            string settingsPath = ConfigurationManager.AppSettings.Get("settingsFilePath");
            ConfigFilePath = settingsPath;
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
            FileInfo fi = new FileInfo(ConfigFilePath);
            if (fi.Exists)
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
                StreamReader myXmlReader = new StreamReader(ConfigFilePath);
                try
                {

                    instance = (Settings)mySerializer.Deserialize(myXmlReader);
                    myXmlReader.Close();
                }
                catch (Exception e)
                {
                    instance = (Settings)Activator.CreateInstance(typeof(Settings));
                    MessageBox.Show("LoadConfig Error\n" + e.Message);
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

        public static void SaveConfig()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
            var fileFullPath = Path.GetFullPath(ConfigFilePath);
            string configDirectory = Path.GetDirectoryName(fileFullPath);
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            StreamWriter myXmlWriter = new StreamWriter(fileFullPath);
            try
            {
                mySerializer.Serialize(myXmlWriter, instance);
            }
            catch (Exception e)
            {
                MessageBox.Show("SaveConfig Error\n" + e.Message);
            }
            finally
            {
                myXmlWriter.Dispose();
            }
        }
    }
}