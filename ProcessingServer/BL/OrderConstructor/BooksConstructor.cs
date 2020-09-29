using ProcessingServer.BL.OrderConstructor.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor
{
    class BooksConstructor<T> : IBooksConstructor<T>
        where T : Book
    {
        /// <summary>
        ///     Реализация компановки списка книг.
        /// </summary>
        /// <param name="workTable"></param>
        /// <param name="bookConstuctor"></param>
        /// <returns></returns>
        public void CreateBooks(BookOrder<T> order, DataTable workTable, Func<DataRow, T> bookConstuctor)
        {
            foreach (DataRow row in workTable.Rows)
            {
                if (row.ItemArray[0].GetType() != typeof(double))
                    continue;

                try
                {
                    var book = bookConstuctor(row);

                    if (book.PrintСoefficient != 0)
                        order.Books.Add(book);

                    else
                        order.MissingBooks.Add(book);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}