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

namespace ProcessingServer.BL.OrderConstructor.Customers.OzonRu
{
    class OzonScriptWriter : IOrderWriter<Block>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly BlackPrintDirectories _cfgPrintDirs = CfgDirs.GetBlackPrintDirectories();
        private readonly ProgramDirectories _cfgProgDirs = CfgDirs.GetProgramDirectories();

        /// <summary>
        ///     Создать скрипт файл для копирования блоков на сервер.
        /// </summary>
        /// <param name="queues"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public string CreateFiles(IEnumerable<Block[]> queues, string orderName)
        {
            try
            {
                foreach (var queue in queues)
                {
                    if (queue[0].SheetFormat == "A4" && queue[0].Imposition == 2)
                    {
                        var scriptPatterns = GetScriptPatternsArray(queue, orderName);
                        WriteScript(scriptPatterns, orderName);
                    }
                    else if (queue[0].SheetFormat == "A3" && queue[0].Imposition == 4)
                    {
                        WriteA5OnA3Data(queue, orderName);
                    }
                }

                return "Файлы скриптов успешно сформированы.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                return e.Message;
            }
        }

        /// <summary>
        ///     Запись книг А5 для печати на А3 листах.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="fileName"></param>
        void WriteA5OnA3Data(IEnumerable<Block> queue, string fileName)
        {
            using (var sw = new StreamWriter($@"{_cfgProgDirs.A5BlockA3SheetPath}\{fileName}.txt", false))
            {
                foreach (var book in queue)
                    sw.WriteLine($"Isbn: {book.Isbn}  Тираж в листах: {book.NumberOfCopies} \r\n" +
                                 $"Наименование: {book.Name} \r\n" +
                                 "\r\n");
            }
        }

        /// <summary>
        ///     Получить список шаблонов.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        string[] GetScriptPatternsArray(IEnumerable<Block> blocks, string orderName)
        {
            var arrCount = blocks.Sum(block => block.NumberOfCopies);
            var scriptPatterns = new string[arrCount];
            var scriptPatternsIndex = 0;

            foreach (var block in blocks)
            {
                var destName = $"{orderName}_{block.BookFormat}_{block.Isbn}";

                for (var copiesIndex = 0; copiesIndex < block.NumberOfCopies; copiesIndex++)
                {
                    scriptPatterns[scriptPatternsIndex] = GetTemplate(block.FullPath, destName);
                    scriptPatternsIndex++;
                }
            }

            return scriptPatterns;
        }

        /// <summary>
        ///     Запись списка шаблонов в скрипт файл.
        /// </summary>
        /// <param name="scriptPatterns"></param>
        /// <param name="orderName"></param>
        void WriteScript(IEnumerable<string> scriptPatterns, string orderName)
        {
            using (var sw = new StreamWriter($@"{_cfgProgDirs.ScriptFilesPath}\{orderName}.ps1", false,
                Encoding.UTF8))
            {
                sw.WriteLine(_cfgPrintDirs.A5BooksKbs);

                foreach (var item in scriptPatterns) sw.WriteLine(item);
            }
        }

        /// <summary>
        ///     Шаблон powershell для копирования блоков на сервер печати. *Писался под nuvera-288 со своими особенностями.
        /// Крайне не советую менять что либо.
        /// </summary>
        /// <param name="itemPath"></param>
        /// <param name="bookType"></param>
        /// <returns></returns>
        string GetTemplate(string itemPath, string bookType)
        {
            itemPath = itemPath.Replace("\"", "`\"");

            return $"$file = Get-ChildItem -LiteralPath \"{itemPath}\"\n" +
                   "DO\n" +
                   "{\n" +
                   "   if ((Get-ChildItem $dest -force | Select-Object -First 1 | Measure-Object).Count -eq 0) \n" +
                   "   {\n" +
                   "       $i = 0\n" +
                   $"      $f =  ('{bookType}.pdf') \n" +
                   "       write-host 'Копирование файла' $f'...'\n" +
                   "       copy-Item $file.FullName (Join-Path $dest $f)\n" +
                   "       write-host 'Файл скопирован' $f'.'\n" +
                   "       write-host 'Ожидание отклика сервера печати...'\n" +
                   "   }\n" +
                   "   else \n" +
                   "   {\n" +
                   "   $i = 1 \n" +
                   "   start-sleep 5\n" +
                   "   }\n" +
                   "}\n" +
                   "While ($i -eq 1)\n";
        }
    }
}