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
    public class SettingsManager<T> where T : class
    {
        private static T instance = null;
        
        public static T Instance
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

        private static void LoadConfig()
        {
            FileInfo fi = new FileInfo(ConfigFilePath);
            if (fi.Exists)
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                StreamReader myXmlReader = new StreamReader(ConfigFilePath);
                try
                {

                    instance = (T)mySerializer.Deserialize(myXmlReader);
                    myXmlReader.Close();
                }
                catch (Exception e)
                {
                    instance = (T)Activator.CreateInstance(typeof(T));
                    MessageBox.Show("LoadConfig Error\n" + e.Message);
                }

                finally
                {
                    myXmlReader.Dispose();
                }
            }
            else
            {
                instance = (T)Activator.CreateInstance(typeof(T));
                SaveConfig();
            }
        }

        public static void SaveConfig()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamWriter myXmlWriter = new StreamWriter(ConfigFilePath);
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
