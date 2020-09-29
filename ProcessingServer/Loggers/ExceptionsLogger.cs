using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingServer.Loggers
{
    /// <summary>
    ///     Потокобезопасный логгер исключений. *Код честно спизженный* :)
    /// </summary>
    internal static class ExceptionsLogger
    {
        private static readonly BlockingCollection<string> BlockingCollection = new BlockingCollection<string>();

        private static readonly string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                               "\\PrintStation\\Logs\\ProcessingServer\\";

        private static readonly Task LogTask;

        static ExceptionsLogger()
        {
            if (!IsLogFileExist(LogPath + "Exceptions.txt"))
                return;

            LogTask = Task.Factory.StartNew(() =>
                {
                    using (var streamWriter = new StreamWriter(LogPath + "Exceptions.txt", true, Encoding.UTF8))
                    {
                        streamWriter.AutoFlush = true;

                        foreach (var s in BlockingCollection.GetConsumingEnumerable())
                            streamWriter.WriteLine(s);
                    }
                },
                TaskCreationOptions.LongRunning);
        }

        public static void WriteLog(string action, int errorCode, string errorDiscription)
        {
            BlockingCollection.Add(
                $"{DateTime.Now} действие: {action}, код: {errorCode}, описание: {errorDiscription} ");
        }

        public static void Flush()
        {
            BlockingCollection.CompleteAdding();
            LogTask.Wait();
        }

        private static bool IsLogFileExist(string logFilePath)
        {
            if (!File.Exists(logFilePath))
                try
                {
                    Directory.CreateDirectory(Directory.GetParent(logFilePath)?.ToString()
                                              ?? throw new InvalidOperationException($"{nameof(logFilePath)} {MethodBase.GetCurrentMethod()?.Name}"));
                    var fs = File.Create(logFilePath);
                    fs.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

            return true;
        }
    }
}