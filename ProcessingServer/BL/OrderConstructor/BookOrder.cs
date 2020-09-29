using MoreLinq;
using MoreLinq.Extensions;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor
{
    class BookOrder<T> where T : Book
    {
        public string OrderName { get; set; }
        public List<T> Books { get; set; } = new List<T>();
        public List<T> MissingBooks { get; set; } = new List<T>();

        /// <summary>
        ///     Абстракция заказа.
        /// </summary>
        /// <param name="booksConstructor"></param>
        /// <param name="workTable"></param>
        /// <param name="bookConstuctor"></param>
        public BookOrder(IBooksConstructor<T> booksConstructor, DataTable workTable, Func<DataRow, T> bookConstuctor)
        {
            ReadTable(booksConstructor, workTable, bookConstuctor);
        }

        void ReadTable(IBooksConstructor<T> booksConstructor, DataTable workTable, Func<DataRow, T> bookConstuctor)
        {
            booksConstructor.CreateBooks(this, workTable, bookConstuctor);
            CalcCopies(Books);
        }

        /// <summary>
        ///     Количество листов на каждую позицию.
        /// </summary>
        public void CalcSheetsPerBook()
        {
            if (Books != null)
                foreach (var book in Books)
                {
                    book.NumberOfCopies =
                        Convert.ToInt32(Math.Ceiling((double)book.NumberOfCopies / book.Imposition * book.PrintСoefficient));
                }
        }

        /// <summary>
        ///     Суммирует тираж по каждой позиции.
        /// </summary>
        /// <param name="bookCollection"></param>
        void CalcCopies(IEnumerable<T> bookCollection)
        {
            var booksCount = MoreEnumerable.CountBy(bookCollection, books => books.Isbn);
            var duplicateBooks = booksCount.Where(books => books.Value > 1).Select(books => books.Key);
            var uniqBooksCollection = DistinctByExtension.DistinctBy(bookCollection, book => book.Isbn).ToList();

            foreach (var bookIsbn in duplicateBooks)
            {
                if (bookIsbn == null)
                    break;

                var calculatedCopies = bookCollection.Where(book => book.Isbn.Equals(bookIsbn)).Sum(book => book.NumberOfCopies);
                ChangeCopies(uniqBooksCollection, bookIsbn, calculatedCopies);
            }

            Books = uniqBooksCollection;
        }

        /// <summary>
        ///     Назначает тираж конкретной позиции.
        /// </summary>
        /// <param name="books"></param>
        /// <param name="isbn"></param>
        /// <param name="copies"></param>
        void ChangeCopies(IEnumerable<T> books, string isbn, int copies)
        {
            foreach (var book in books)
            {
                if (book.Isbn.Equals(isbn))
                    book.NumberOfCopies = copies;
            }
        }
    }
}