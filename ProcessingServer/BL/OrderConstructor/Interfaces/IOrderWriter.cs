using System.Collections.Generic;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс записи очередей печати обложек.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IOrderWriter<T> where T : Book
    {
        string CreateFiles(IEnumerable<T[]> queues, string orderName);
    }
}