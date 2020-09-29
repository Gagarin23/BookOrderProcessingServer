using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс записи скрипт файла.
    /// </summary>
    internal interface IScriptWriter
    {
        string[] GetStringArray(Block[] blocks, string orderName);
        string GetTemplate(string itemPath, string bookType);

        /// <summary>
        ///     Запись скрипта для отправки на сервер.
        /// </summary>
        /// <param name="scriptPatterns"></param>
        /// <param name="orderName"></param>
        void WriteScript(string[] scriptPatterns, string orderName, string dest);
    }
}