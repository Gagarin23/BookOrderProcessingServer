using System.Data;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс чтения тех задания.
    /// </summary>
    public interface IExcelReader
    {
        public DataTable GetWorkTable(string path);
    }
}