using System.Collections.Generic;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс записи в файл пропущенных книг.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IMissingBookWriter<T> where T : Book
    {
        void WriteMissingBook(IEnumerable<T> books, string orderName);
    }
}