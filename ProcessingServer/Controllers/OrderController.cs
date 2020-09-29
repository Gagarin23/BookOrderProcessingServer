using ProcessingServer.BL.OrderConstructor;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using TransferObjects;

namespace ProcessingServer.Controllers
{
    /// <summary>
    ///     Универсальный класс для обработки заказа.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OrderController<T> : IDisposable
        where T : Book
    {
        private readonly string _excelPath;
        private DataTable _workTable;

        private BookOrder<T> _order;

        private readonly IEnumerable<T[]> _queuesForPrinting;
        public string Result { get; }

        public OrderController(string excelPath, IExcelConverter excelReader, IOrderConstructor<T> orderConstructor,
            IDbServerRequest<T> serverRequest, IMissingBookWriter<T> misWriter, IOrderWriter<T> orderWriter)
        {
            _excelPath = excelPath;
            try
            {
                SetWorkTable(excelReader);
                GetOrder(orderConstructor);
                _order.OrderName = Path.GetFileNameWithoutExtension(_excelPath);
                (List<T[]> Queues, List<T> MissingBooks) booksFromServer = GetServerRespone(serverRequest);
                _queuesForPrinting = booksFromServer.Queues;
                _order.MissingBooks = booksFromServer.MissingBooks;

                if (_order.MissingBooks.Count() > 1)
                    WriteMissingBooks(misWriter);

                if (booksFromServer.Queues.Count == 0)
                {
                    Result = "Сервер базы данных вернул пустой ответ.";
                    return;
                }


                WriteOrderData(orderWriter);
                Result = $"{_order.OrderName} успешно обработан.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                Result = e.Message;
            }
        }

        void SetWorkTable(IExcelConverter excelReader) =>
            _workTable = excelReader.GetWorkTable(_excelPath);

        void GetOrder(IOrderConstructor<T> orderConstructor) =>
            _order = orderConstructor.GetOrder(_workTable);


        (List<T[]>, List<T>) GetServerRespone(IDbServerRequest<T> serverRequest) =>
            serverRequest.GetResponseFromDbServer(_order.Books);

        void WriteOrderData(IOrderWriter<T> orderWriter) =>
            orderWriter.CreateFiles(_queuesForPrinting, _order.OrderName);

        void WriteMissingBooks(IMissingBookWriter<T> misWriter) =>
            misWriter.WriteMissingBook(_order.MissingBooks, _order.OrderName);

        public void Dispose() =>
            DisposeManagedResources();

        protected virtual void DisposeManagedResources() { }
    }
}