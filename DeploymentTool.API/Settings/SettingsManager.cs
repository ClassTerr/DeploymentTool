﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace DeploymentTool.Settings
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