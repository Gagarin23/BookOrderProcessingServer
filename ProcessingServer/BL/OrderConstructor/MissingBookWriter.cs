using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using ProcessingServer.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor
{
    class MissingBookWriter<T> : IMissingBookWriter<T> where T : Book
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ProgramDirectories _cfgProgDirs = CfgDirs.GetProgramDirectories();

        /// <summary>
        ///     Реализация записи в файл пропущенных книг.
        /// </summary>
        /// <param name="missingBooks"></param>
        /// <param name="orderName"></param>
        public void WriteMissingBook(IEnumerable<T> missingBooks, string orderName)
        {
            try
            {
                using (var sw = new StreamWriter($@"{_cfgProgDirs.MissingBooksPath}\{orderName}_Blocks.txt", false,
                    Encoding.UTF8))
                {
                    foreach (var misBook in missingBooks)
                        sw.WriteLine($"{misBook.Isbn}  Тираж в листах: {misBook.NumberOfCopies}\r\n" +
                                     $"  Наименование: {misBook.Name}" +
                                     "\r\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
            }
        }
    }
}
