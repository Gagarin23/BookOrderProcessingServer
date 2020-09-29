using System.Collections.Generic;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфес обращения к серверу базы данных.
    /// </summary>
    /// <param name="books"></param>
    /// <returns></returns>
    interface IDbServerRequest<T> where T : Book
    {
        public (List<T[]>, List<T>) GetResponseFromDbServer(IEnumerable<T> books);
    }
}