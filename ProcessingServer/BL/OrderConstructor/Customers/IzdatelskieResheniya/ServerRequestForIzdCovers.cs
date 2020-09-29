using Newtonsoft.Json;
using ProcessingServer.BL.NetWork;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using System.Collections.Generic;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.IzdatelskieResheniya
{
    class ServerRequestForIzdCovers : IDbServerRequest<Cover>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ServicesAdress _cfgNetwork = CfgDirs.GetServicesAdress();

        /// <summary>
        ///     Отправка списка книг для создания очередей печати.
        /// </summary>
        /// <param name="books"></param>
        /// <returns>Пропущенные книги и список очередей (под различные шаблоны печати).</returns>
        public (List<Cover[]>, List<Cover>) GetResponseFromDbServer(IEnumerable<Cover> books)
        {
            var json = JsonConvert.SerializeObject(books);

            return ConnectionForBuildOrder<Cover>.RunConnection(_cfgNetwork.IzdatelskieCoverConnection.Ip, _cfgNetwork.IzdatelskieCoverConnection.Port,
                json);
        }
    }
}
