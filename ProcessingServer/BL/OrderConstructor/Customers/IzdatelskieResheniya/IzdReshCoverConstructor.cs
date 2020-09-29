using ProcessingServer.BL.OrderConstructor.Interfaces;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.IzdatelskieResheniya
{
    class IzdReshCoverConstructor : IOrderConstructor<Cover>
    {
        public string[] FilesArray { get; }
        private readonly ExcelReaderTemplate _exTemplate;

        /// <summary>
        ///     Реализация сборки заказа для Издательского Решения.
        /// </summary>
        public IzdReshCoverConstructor(string excelPath, ExcelReaderTemplate excelReaderTemplate)
        {
            var orderPath = Directory.GetParent(excelPath)?.ToString();
            _exTemplate = excelReaderTemplate;
            FilesArray = Directory.GetFiles(orderPath
                                            ?? throw new InvalidOperationException($"{nameof(orderPath)} {MethodBase.GetCurrentMethod()?.Name}"),
                                            "*.pdf", SearchOption.AllDirectories);
        }

        /// <summary>
        ///     Сборка базового конструктора заказа.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public BookOrder<Cover> GetOrder(DataTable dataTable)
        {
            var order = new BookOrder<Cover>(new BooksConstructor<Cover>(), dataTable, CoverConstructor);
            order.CalcSheetsPerBook();

            return order;
        }

        private Cover CoverConstructor(DataRow row)
        {
            var name = row[_exTemplate.Name] as string;
            var path = GetPath(name);

            if (path != null
               && row[_exTemplate.BookFormat] is string bookFormat
               && row[_exTemplate.Lamination] is string lamination
               && row[_exTemplate.BookMount] is string bookMount
               && row[_exTemplate.NumberOfCopies] is double numberOfCopies
               && row[_exTemplate.Imposition] is double imposition)
            {

                if (path != null)
                    return new Cover()
                    {
                        Isbn = Path.GetFileNameWithoutExtension(path),
                        Name = name,
                        NumberOfCopies = Convert.ToInt32(numberOfCopies),
                        BookFormat = bookFormat,
                        Lamination = lamination,
                        BookMount = bookMount,
                        Imposition = Convert.ToInt32(imposition),
                        PrintСoefficient = 1.11,
                        FullPath = path
                    };
            }
            else if (name != null)
            {
                return new Cover() { Name = name, PrintСoefficient = 0 };
            }

            return new Cover() { Name = $"Не удалось прочесть имя строки №{row.ItemArray[0]}", PrintСoefficient = 0 };
        }

        /// <summary>
        ///     Получить полный путь к файлу исходя из бар-кода в названии позиции.
        /// </summary>
        /// <param name="bookName"></param>
        /// <returns></returns>
        private string GetPath(string bookName)
        {
            var bookBarCode = bookName.Substring(0, 6);
            return Array.Find(FilesArray, s => s.Contains(bookBarCode) && s.Contains("cover"));
        }
    }
}
