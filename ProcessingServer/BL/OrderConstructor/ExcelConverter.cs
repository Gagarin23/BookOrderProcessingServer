using ExcelDataReader;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Loggers;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace ProcessingServer.BL.OrderConstructor
{
    class ExcelConverter : IExcelConverter
    {
        /// <summary>
        ///     Конвертер excel файла в DataTable.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable GetWorkTable(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader;

                    if (Path.GetExtension(path).ToUpper() == ".XLS")
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                    else
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    var config = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    };

                    DataSet dataSet = excelReader.AsDataSet(config);
                    return dataSet.Tables[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                return new DataTable();
            }
        }
    }
}