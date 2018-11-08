using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeploymentTool.Core.Settings
{
    public class SettingsManager
    {
        private static Settings instance = null;
        
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    LoadConfig();
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

        public static void LoadConfig()
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
                    instance = new Settings();
                    MessageBox.Show("Ошибка сериализации LoadConfig\n" + e.Message);
                }

                finally
                {
                    myXmlReader.Dispose();
                }
            }
            else
            {
                instance = new Settings();
                SaveConfig();
            }
        }

        public static void SaveConfig()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
            StreamWriter myXmlWriter = new StreamWriter(ConfigFilePath);
            try
            {
                mySerializer.Serialize(myXmlWriter, instance);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сериализации SaveConfig\n" + e.Message);
            }
            finally
            {
                myXmlWriter.Dispose();
            }
        }
    }
}
