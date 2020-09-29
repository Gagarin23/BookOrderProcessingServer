using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using ProcessingServer.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.IzdatelskieResheniya
{
    class IzdScriptWriter : IOrderWriter<Cover>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ProgramDirectories _cfgProgDirs = CfgDirs.GetProgramDirectories();
        public string OrderName { get; }

        public IzdScriptWriter(string excelPath)
        {
            OrderName = Path.GetFileNameWithoutExtension(excelPath);
        }

        /// <summary>
        ///     Сортировка очередей и запись в скрипт файл.
        /// </summary>
        /// <param name="queues"></param>
        /// <param name="orderName"></param>
        /// <returns>Сообщение о результате операции.</returns>
        public string CreateFiles(IEnumerable<Cover[]> queues, string orderName)
        {
            try
            {
                var sortedOrder = queues.OrderBy(covers => covers[0].Lamination)
                    .ThenBy(covers => covers[0].BookFormat)
                    .ThenBy(covers => covers[0].BookMount)
                    .ThenByDescending(covers => covers[0].Imposition)
                    .ToList();

                var scriptPatterns = GetPatterns(sortedOrder);
                WriteScript(scriptPatterns, OrderName);

                return $"PowerShell скрипт успешно записан для заказа {OrderName}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                return e.Message;
            }
        }

        /// <summary>
        ///     Получить массив шаблонов.
        /// </summary>
        /// <param name="queues"></param>
        /// <returns></returns>
        List<string> GetPatterns(IEnumerable<Cover[]> queues)
        {
            var scriptPatterns = new List<string>();

            foreach (Cover[] queue in queues)
            {
                foreach (Cover cover in queue)
                {
                    scriptPatterns.Add(GetTemplate(cover, OrderName));
                }
            }

            return scriptPatterns;
        }

        /// <summary>
        ///     Запись щаблонов в скрипт файл.
        /// </summary>
        /// <param name="scriptPatterns"></param>
        /// <param name="orderName"></param>
        void WriteScript(IEnumerable<string> scriptPatterns, string orderName)
        {
            using (var sw = new StreamWriter($@"{ _cfgProgDirs.CoversQueuesPath}\{orderName}.ps1", false, Encoding.UTF8))
            {
                foreach (var item in scriptPatterns) sw.WriteLine(item);
            }
        }

        /// <summary>
        /// Фундервафля для павершелла. Лучше ничего не менять.
        /// </summary>
        /// <param name="cover"></param>
        /// <param name="orederName"></param>
        /// <returns></returns>
        string GetTemplate(Cover cover, string orederName)
        {
            cover.FullPath = cover.FullPath.Replace("\"", "`\"");

            return $"$dest = \"{_cfgProgDirs.HotFolderPath}\\{cover.Imposition}_na_list\\\" \n" +
                   $"$file = Get-ChildItem -LiteralPath \"{cover.FullPath}\"\n" +
                   "DO\n" +
                   "{\n" +
                   "    if ((Get-ChildItem $dest -force).Count -lt 3) \n" +
                   "    {\n" +
                   $"    $i = ($c = Get-Content -LiteralPath \"{_cfgProgDirs.HotFolderPath}\\{cover.Imposition}_na_list\\[_EFI_HotFolder_]\\Folder.cfg\") | \n" +
                   "    Select-String -Pattern '    <KEYVALUE Key=\"num copies\"' | \n" +
                   "    Select-Object -ExpandProperty LineNumber\n" +
                   $"    $c = $c[0..($i - 2)], ('    <KEYVALUE Key=\"num copies\" Value=\"{cover.NumberOfCopies}\" KeyDisplayValue=\"Копии\" LocDisplayValue=\"1\"/>'), $c[$i..$c.Length] \n" +
                   $"    $c | Out-File -LiteralPath \"{_cfgProgDirs.HotFolderPath}\\{cover.Imposition}_na_list\\[_EFI_HotFolder_]\\Folder.cfg\" -Encoding default \n" +
                   "    $i = 0 \n" +
                   $"    $f = \"{orederName}_\" + $file.BaseName + \"_{cover.BookFormat}_{cover.Imposition}_{cover.Lamination}\" + $file.Extension \n" +
                   "    write-host 'Копирование файла' $f'...' \n" +
                   "    copy-Item $file (Join-Path $dest $f) \n" +
                   "    write-host 'Файл скопирован' $f. \n" +
                   "    write-host 'Ожидание отклика сервера печати...' \n" +
                   "	start-sleep 5  \n" +
                   "    } \n" +
                   "	else \n" +
                   "    { \n" +
                   "	    $i = 1 \n" +
                   "    } \n" +
                   "	start-sleep 5 \n" +
                   "} \n" +
                   "While ($i -eq 1) \n" +
                   "\n";
        }
    }
}