using System.Collections.Generic;
using Newtonsoft.Json;
using ProcessingServer.BL.NetWork;
using ProcessingServer.BL.OrderConstructor.Interfaces;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using TransferObjects;

namespace ProcessingServer.BL.OrderConstructor.Customers.OzonRu
{
    class ServerRequestForOzonBlocks : IDbServerRequest<Block>
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ServicesAdress _cfgNetwork = CfgDirs.GetServicesAdress();

        /// <summary>
        ///     Реализация запроса к серверу базы данных.
        /// </summary>
        /// <param name="books"></param>
        /// <returns>Пропущенные книги и блоки с указанными путями на фаловом сервере.</returns>
        public (List<Block[]>, List<Block>) GetResponseFromDbServer(IEnumerable<Block> books)
        {
            var json = JsonConvert.SerializeObject(books);

            return ConnectionForBuildOrder<Block>.RunConnection(_cfgNetwork.OzonBlockConnection.Ip, _cfgNetwork.OzonBlockConnection.Port,
                json);
        }
    }
}