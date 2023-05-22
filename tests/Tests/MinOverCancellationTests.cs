namespace ConsoleApp1
{    
    public class MinOverCancellationTests
    {
        [Theory]
        [MemberData(nameof(GetMultiplePotentialSubsetData))]
        public void When_MultiplePotentialSubsetsExist_Then_ReturnsMinOverCancellationSubset(
            List<Order> orders, int cancelTarget, List<int> expectedOrderIds)
        {
            var actualOrderIds = MinOverCancellation.Collect(orders, cancelTarget);

            Assert.NotEmpty(actualOrderIds);
            Assert.Equal(expectedOrderIds, actualOrderIds);
        }

        [Theory]
        [MemberData(nameof(GetNoPotentialSubsetData))]
        public void When_NoPotentialSubsetsExist_Then_ReturnsEmptySet(
            List<Order> orders, int cancelTarget)
        {
            var actualOrderIds = MinOverCancellation.Collect(orders, cancelTarget);

            Assert.NotNull(actualOrderIds);
            Assert.Empty(actualOrderIds);
        }

        [Fact]
        public void When_EmptyListSpecified_Then_ReturnsEmptySet()
        {
            var actualOrderIds = MinOverCancellation.Collect(new List<Order>(), 10);

            Assert.NotNull(actualOrderIds);
            Assert.Empty(actualOrderIds);
        }

        public static IEnumerable<object[]> GetMultiplePotentialSubsetData()
        {
            var allData = new List<object[]>
            {
                new object[] { CreateOrders(3, 2, 3, 1, 5), 2, new List<int>{ 2 } },
                new object[] { CreateOrders(3, 2, 9, 1, 5), 7, new List<int>{ 2, 5 } },
                new object[] { CreateOrders(5, 2, 7, 2), 3, new List<int>{ 2, 4 } },
                new object[] { CreateOrders(22, 10, 18, 6, 15, 55), 12, new List<int>{ 5 } }
            };

            return allData;
        }

        public static IEnumerable<object[]> GetNoPotentialSubsetData()
        {
            var allData = new List<object[]>
            {
                new object[] { CreateOrders(3, 2, 3, 1, 5), 100 },
                new object[] { CreateOrders(-3, -2, 9, 1, -5), 20 },
                new object[] { CreateOrders(-5, 2, -7, 2), 30 },
                new object[] { CreateOrders(22, 10, 18, 6, 15, 55), 200 }
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
