using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using ProcessingServer.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor
{
    class PdfsCombiner : IOrderWriter<Cover>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ProgramDirectories _cfgProgDirs = CfgDirs.GetProgramDirectories();
        public bool IsSuccefulComplite { get; private set; }

        /// <summary>
        ///     Реализация компановки очереди печати.
        /// </summary>
        /// <param name="queues"></param>
        /// <param name="excelName"></param>
        /// <returns></returns>
        public string CreateFiles(IEnumerable<Cover[]> queues, string excelName)
        {
            foreach (var queue in queues)
            {
                try
                {
                    IsSuccefulComplite = false;

                    var template = queue[0];
                    var fileName = $"{excelName}_{template.BookFormat}_{template.BookMount}_{template.Imposition}nl.pdf";
                    var filePaths = DuplicateCovers(queue);
                    Combine(fileName, filePaths, _cfgProgDirs.CoversQueuesPath);

                    if (File.Exists($@"{_cfgProgDirs.CoversQueuesPath}\{fileName}"))
                        IsSuccefulComplite = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                    break;
                }
            }

            if (IsSuccefulComplite)
                return "Очереди печати успешно сформированы.";
            else
                return "Во время формирования очередей возникла ошибка.";
        }

        /// <summary>
        ///     Цикл для записи одного и того же макета в файл исходя из тиража.
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private List<string> DuplicateCovers(IEnumerable<Cover> queue)
        {
            var filePaths = new List<string>();

            foreach (var cover in queue)
                for (var i = 0; i < cover.NumberOfCopies; i++)
                    filePaths.Add(cover.FullPath);

            return filePaths;
        }

        /// <summary>
        ///     Склейщик pdf файлов.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pdfPaths"></param>
        /// <param name="fileDest"></param>
        private void Combine(string fileName, IEnumerable<string> pdfPaths, string fileDest)
        {
            fileName = fileName.Replace(@"*", "x");
            Console.WriteLine($"Create file: {fileName}");

            if (!Directory.Exists(fileDest))
                if (!IsDirectoryCreated(fileDest))
                    return;

            using (var outputDocument = new PdfDocument())
            {
                foreach (var path in pdfPaths)
                {
                    try // Не снимать try/catch! Некоторые пдфки выдают исключения.
                    {
                        outputDocument.AddPage(PdfReader.Open(path, PdfDocumentOpenMode.Import).Pages[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + ": " + Path.GetFileNameWithoutExtension(path));
                        ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                    }
                }

                outputDocument.Save($@"{fileDest}\{fileName}");
            }
        }

        private bool IsDirectoryCreated(string fileDest)
        {
            try
            {
                Directory.CreateDirectory(fileDest);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Создайте папку для очередей печати в ручную.");
                Console.ReadLine();
                return false;
            }
        }
    }
}
