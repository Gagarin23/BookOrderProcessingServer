using ProcessingServer.Controllers;
using ProcessingServer.Loggers;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ProcessingServerUnitTest")]
namespace ProcessingServer
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                new MainController();

                while (true) Thread.Sleep(100000000);
                {
                    
                }
            }
            catch (Exception e)
            {
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
            }
            finally
            {
                ExceptionsLogger.Flush();
                EventsLogger.Flush();
            }
        }
    }
}