using System.Data;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс конструктора заказа.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IOrderConstructor<T> where T : Book
    {
        public BookOrder<T> GetOrder(DataTable dataTable);
    }
}