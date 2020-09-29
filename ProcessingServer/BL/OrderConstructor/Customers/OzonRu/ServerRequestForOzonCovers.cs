using Newtonsoft.Json;
using ProcessingServer.BL.NetWork;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using System.Collections.Generic;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.OzonRu
{
    class ServerRequestForOzonCovers : IDbServerRequest<Cover>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ServicesAdress _cfgNetwork = CfgDirs.GetServicesAdress();

        /// <summary>
        ///     Отослать список обложек на сервер базы данных.
        /// </summary>
        /// <param name="books"></param>
        /// <returns>Пропущенные книги и очереди печати обложек с указанными путями на фаловом сервере.</returns>
        public (List<Cover[]>, List<Cover>) GetResponseFromDbServer(IEnumerable<Cover> books)
        {
            var json = JsonConvert.SerializeObject(books);

            return ConnectionForBuildOrder<Cover>.RunConnection(_cfgNetwork.OzonCoverConnection.Ip, _cfgNetwork.OzonCoverConnection.Port,
                json);
        }
    }
}
