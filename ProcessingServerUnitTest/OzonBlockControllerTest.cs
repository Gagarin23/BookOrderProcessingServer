using Newtonsoft.Json;
using ProcessingServer.BL.OrderConstructor;
using ProcessingServer.BL.OrderConstructor.Customers.OzonRu;
using TransferObjects;
using Xunit;

namespace ProcessingServerUnitTest
{
    public class OzonBlockControllerTest
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
            var bookOrder = new OzonBlockConstructor(excelParameters);
            BookOrder<Block> order = bookOrder.GetOrder(randomOrder);

            var json = JsonConvert.SerializeObject(order.Books);

            //Result
            Assert.Equal(10, order.Books.Count);
        }
    }
}