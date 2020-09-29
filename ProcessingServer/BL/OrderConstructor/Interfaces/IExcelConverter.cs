using System.Data;

namespace ProcessingServer.BL.OrderConstructor.Interfaces
{
    /// <summary>
    ///     Интерфейс чтения тех задания.
    /// </summary>
    public interface IExcelConverter
    {
        public DataTable GetWorkTable(string path);
    }
}