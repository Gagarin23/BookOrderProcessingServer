using ProcessingServer.BL.OrderConstructor;
using ProcessingServer.BL.OrderConstructor.Customers.OzonRu;
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
            var randomOrder = UsefulExtentions.DataTableTestStub.GetRandomOrder(10);

            //Act
            var bookOrder = new OzonCoverConstructor(excelParameters);
            BookOrder<Cover> order = bookOrder.GetOrder(randomOrder);

            //Result
            Assert.Equal(10, order.Books.Count);
        }
    }
}
