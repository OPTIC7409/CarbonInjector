using CarbonInjector.Properties;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarbonInjector
{
    [Serializable]
    public struct Config
    {

        public bool RPC;
        public bool autoInject;
        public bool openGame;
        public string DLLPath;

        public static Config LoadDaDefault()
        {
            return new Config()
            {
                RPC = true,
                DLLPath = "",
                autoInject = false,
                openGame = false

            };
        }

        public static void SaveDaConfig(Config configuration)
        {
            var formatting = new BinaryFormatter();
            using (var stream = new FileStream(Utility.InjectorPath + "\\config.carbon", FileMode.Create))
            {
                formatting.Serialize(stream, configuration);
            }
        }

        public static Config LoadConfig()
        {
            if (!File.Exists(Utility.InjectorPath + "\\config.carbon")) return LoadDaDefault();

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(Utility.InjectorPath + "\\config.carbon", FileMode.Open, FileAccess.Read))
            {
                var config = (Config)formatter.Deserialize(stream);
                return config;
            }
        }
    }
}
