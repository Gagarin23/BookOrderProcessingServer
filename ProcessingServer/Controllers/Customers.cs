using ProcessingServer.BL.OrderConstructor;
using ProcessingServer.BL.OrderConstructor.Customers.IzdatelskieResheniya;
using ProcessingServer.BL.OrderConstructor.Customers.OzonRu;
using TransferObjects;

namespace ProcessingServer.Controllers
{
    /// <summary>
    ///     Класс хранящий логику обработки заказа под отдельных контрагентов.
    /// </summary>
    static class Customers
    {
        /// <summary>
        ///     Инициализация конструктора обложек для ozon.ru
        /// </summary>
        /// <returns>Возвращает "Успех" или строку ошибки.</returns>
        public static string OzonCoverOrder(ExcelReaderTemplate excelReaderTemplate, string excelPath)
        {
            var controller = new OrderController<Cover>(excelPath,
                new ExcelConverter(),
                new OzonCoverConstructor(excelReaderTemplate),
                new ServerRequestForOzonCovers(),
                new MissingBookWriter<Cover>(),
                new PdfsCombiner());

            var result = controller.Result;
            controller.Dispose();
            return result;
        }

        /// <summary>
        ///     Инициализация конструктора блоков для ozon.ru
        /// </summary>
        /// <returns>Возвращает "Успех" или строку ошибки.</returns>
        public static string OzonBlockOrder(ExcelReaderTemplate excelReaderTemplate, string excelPath)
        {
            var controller = new OrderController<Block>(excelPath,
                new ExcelConverter(),
                new OzonBlockConstructor(excelReaderTemplate),
                new ServerRequestForOzonBlocks(),
                new MissingBookWriter<Block>(),
                new OzonScriptWriter());

            var result = controller.Result;
            controller.Dispose();
            return result;
        }

        /// <summary>
        ///     Инициализация конструктора обложек для "Издательские Решения".
        /// </summary>
        /// <returns></returns>
        public static string IzdCoverOrder(ExcelReaderTemplate excelReaderTemplate, string excelPath)
        {
            var controller = new OrderController<Cover>(excelPath,
                new ExcelConverter(),
                new IzdReshCoverConstructor(excelPath, excelReaderTemplate),
                new ServerRequestForIzdCovers(),
                new MissingBookWriter<Cover>(),
                new IzdScriptWriter(excelPath));

            var result = controller.Result;
            controller.Dispose();
            return result;
        }
    }
}