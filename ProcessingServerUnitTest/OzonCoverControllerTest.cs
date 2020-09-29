﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProcessingServer.BL.NetWork;
using ProcessingServer.BL.OrderConstructor;
using ProcessingServer.BL.OrderConstructor.Customers.OzonRu;
using ProcessingServer.Controllers;
using TransferObjects;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ProcessingServerUnitTest
{
    
    public class OzonCoverControllerTest
    {
        [Fact]
        public void OrderTest()
        {
            var excelParameters = new ExcelReaderTemplate()
            {
                Isbn = 1,
                Name = 2,
                NumberOfCopies = 3,
                BookFormat = 5,
                BookMount = 4,
                Lamination = 6,
                Imposition = 7,
            };
            var randomOrder = UsefulExtentions.Randomizer.GetRandomOrder(10);

            //Act
            var bookOrder = new OzonCoverConstructor(excelParameters);
            BookOrder<Cover> order = bookOrder.GetOrder(randomOrder);

            //Result
            Assert.Equal(10, order.Books.Count);
        }
    }
}