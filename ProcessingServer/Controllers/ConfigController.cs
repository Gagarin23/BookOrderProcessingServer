using Newtonsoft.Json;
using ProcessingServer.Configs;
using ProcessingServer.Loggers;
using System;
using System.IO;
using System.Reflection;

namespace ProcessingServer.Controllers
{
    /// <summary>
    ///     Контроллер управления файлами конфигурации.
    /// </summary>
    internal class ConfigController
    {
        private static readonly string ConfPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                  "\\PrintStation\\Configuration\\ProcessingServer\\";

        public string ServicesAdress { get; } = ConfPath + "ServicesAdress.cfg";

        public string ProgramDirectories { get; } = ConfPath + "ProgramDirectories.cfg";

        public string BlackPrintDirectories { get; } = ConfPath + "BlackPrintDirectories.cfg";


        public ServicesAdress GetServicesAdress()
        {
            return LoadConfig<ServicesAdress>(ServicesAdress);
        }

        public ProgramDirectories GetProgramDirectories()
        {
            return LoadConfig<ProgramDirectories>(ProgramDirectories);
        }

        public BlackPrintDirectories GetBlackPrintDirectories()
        {
            return LoadConfig<BlackPrintDirectories>(BlackPrintDirectories);
        }

        private T LoadConfig<T>(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveConfig<T>(T cfg, string path)
        {
            if (!File.Exists(path))
                CreateConfigFile(path);

            var json = JsonConvert.SerializeObject(cfg);

            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine(json);
            }
        }

        void CreateConfigFile(string configPath)
        {
            try
            {
                Directory.CreateDirectory(Directory.GetParent(configPath)?.ToString()
                                          ?? throw new InvalidOperationException($"{nameof(configPath)} {MethodBase.GetCurrentMethod()?.Name}"));
                var stream = File.Create(configPath);
                stream.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
            }
        }
    }
}