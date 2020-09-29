using System;
using System.Collections.Generic;
using System.Data;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс чтения DataTable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IBooksConstructor<T> where T : Book
    {
        public void CreateBooks(BookOrder<T> order, DataTable workTable, Func<DataRow, T> bookConstuctor);
    }
}