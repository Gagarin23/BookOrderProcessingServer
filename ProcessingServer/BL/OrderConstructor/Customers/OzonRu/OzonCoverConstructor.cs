using ProcessingServer.BL.OrderConstructor.Interfaces;
using System;
using System.Data;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.OzonRu
{
    class OzonCoverConstructor : IOrderConstructor<Cover>
    {
        private readonly ExcelReaderTemplate _exTemplate;

        public OzonCoverConstructor(ExcelReaderTemplate exTemplate)
        {
            _exTemplate = exTemplate;
        }

        /// <summary>
        ///     Реализация базового конструктора обложек для Ozon.ru
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public BookOrder<Cover> GetOrder(DataTable dataTable)
        {
            var order = new BookOrder<Cover>(new BooksConstructor<Cover>(), dataTable, CoverConstructor);
            order.CalcSheetsPerBook();

            return order;
        }

        /// <summary>
        ///     Конструктор обложек.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Cover CoverConstructor(DataRow row)
        {
            if(row[_exTemplate.Isbn] is string isbn
               && row[_exTemplate.Name] is string name
               && row[_exTemplate.BookFormat] is string bookFormat
               && row[_exTemplate.Lamination] is string lamination
               && row[_exTemplate.BookMount] is string bookMount
               && row[_exTemplate.NumberOfCopies] is double numberOfCopies
               && row[_exTemplate.Imposition] is double imposition)
            {
                return new Cover()
                {
                    Isbn = isbn,
                    Name = name,
                    NumberOfCopies = Convert.ToInt32(numberOfCopies),
                    BookFormat = bookFormat,
                    Lamination = lamination,
                    BookMount = bookMount,
                    Imposition = Convert.ToInt32(imposition),
                    PrintСoefficient = 1.11
                };
            }
            else if(row[_exTemplate.Isbn] is string lostIsbn)
            {
                return new Cover() { Isbn = lostIsbn, PrintСoefficient = 0};
            }

            return new Cover() { Isbn = $"Не удалось прочесть isbn строки №{row.ItemArray[0]}", PrintСoefficient = 0 };
        }
    }
}
