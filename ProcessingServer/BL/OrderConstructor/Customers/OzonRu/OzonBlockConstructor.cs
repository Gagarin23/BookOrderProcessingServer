using ProcessingServer.BL.OrderConstructor.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.OzonRu
{
    class OzonBlockConstructor : IOrderConstructor<Block>
    {
        private readonly ExcelReaderTemplate _exTemplate;

        public OzonBlockConstructor(ExcelReaderTemplate exTemplate)
        {
            _exTemplate = exTemplate;
        }

        /// <summary>
        ///     Реализация базового конструктора блоков для Ozon.ru
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public BookOrder<Block> GetOrder(DataTable dataTable)
        {
            var order = new BookOrder<Block>(new BooksConstructor<Block>(), dataTable, BlockConstructor);
            order.Books = order.Books.OrderByDescending(books => books.Imposition).ToList();
            SetImposeAndSheetFormat(order.Books);
            order.CalcSheetsPerBook();

            return order;
        }

        /// <summary>
        ///     Конструктор блоков.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Block BlockConstructor(DataRow row)
        {
            if (row[_exTemplate.Isbn] is string isbn
               && row[_exTemplate.Name] is string name
               && row[_exTemplate.BookFormat] is string bookFormat
               && row[_exTemplate.BookMount] is string bookMount
               && row[_exTemplate.NumberOfCopies] is double numberOfCopies
               && row[_exTemplate.Imposition] is double imposition)
            {
                return new Block
                {
                    Isbn = isbn,
                    Name = name,
                    NumberOfCopies = Convert.ToInt32(numberOfCopies),
                    BookFormat = bookFormat,
                    BookMount = bookMount,
                    Imposition = Convert.ToInt32(imposition),
                    PrintСoefficient = 1.00
                };
            }
            else if (row[_exTemplate.Isbn] is string lostIsbn)
            {
                return new Block() { Isbn = lostIsbn, PrintСoefficient = 0 };
            }

            return new Block() { Isbn = $"Не удалось прочесть isbn строки №{row.ItemArray[0]}", PrintСoefficient = 0 };
        }

        /// <summary>
        ///     Установка формата печатного листа.
        /// </summary>
        /// <param name="blocks"></param>
        private void SetImposeAndSheetFormat(IEnumerable<Block> blocks)
        {
            foreach (var block in blocks)
                block.SheetFormat = block.SetSheetFormat(block.BookMount, block.BookFormat);
        }
    }
}