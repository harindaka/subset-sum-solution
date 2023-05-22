namespace ConsoleApp1
{    
    public class InternalTransferTests
    {
        [Theory]
        [MemberData(nameof(GetMultiplePotentialSubsetData))]
        public void When_MultiplePotentialSubsetsExist_Then_ReturnsLargestSetOfOffsettingOrderIds(
            List<Order> orders, List<int> expectedOrderIds)
        {
            var actualOrderIds = InternalTransfer.Collect(orders);

            Assert.NotEmpty(actualOrderIds);
            Assert.Equal(expectedOrderIds, actualOrderIds);
        }

        [Theory]
        [MemberData(nameof(GetNoPotentialSubsetData))]
        public void When_NoPotentialSubsetsExist_Then_ReturnsEmptySet(List<Order> orders)
        {
            var actualOrderIds = InternalTransfer.Collect(orders);

            Assert.NotNull(actualOrderIds);
            Assert.Empty(actualOrderIds);
        }

        [Fact]
        public void When_EmptyListSpecified_Then_ReturnsEmptySet()
        {
            var actualOrderIds = InternalTransfer.Collect(new List<Order>());

            Assert.NotNull(actualOrderIds);
            Assert.Empty(actualOrderIds);
        }

        public static IEnumerable<object[]> GetMultiplePotentialSubsetData()
        {
            var allData = new List<object[]>
            {
                new object[] { CreateOrders(10, -5, -5, 5, -5, 2), new List<int>{ 1, 2, 3, 4, 5 } },
                new object[] { CreateOrders(10, -3, -5, 5, -2), new List<int>{ 1, 2, 3, 5 } },
                new object[] { CreateOrders(10, -8, -2, 8, -2, -5, -1), new List<int>{ 1, 2, 3, 4, 5, 6, 7 } }
            };

            return allData;
        }

        public static IEnumerable<object[]> GetNoPotentialSubsetData()
        {
            var allData = new List<object[]>
            {
                new object[] { CreateOrders(10, -5, 3, 8) },
                new object[] { CreateOrders(9, -3, -1, 10, -1) },
                new object[] { CreateOrders(-2, -8, -2, 1, -5, -6, -7) }
            };

            return allData;
        }

        private static List<Order> CreateOrders(params int[] quantities)
        {
            return quantities.Select((quantity, index) => new Order
            {
                Id = index + 1,
                Quantity = quantity,
                Price = 100m
            }).ToList();
        }
    }
}
