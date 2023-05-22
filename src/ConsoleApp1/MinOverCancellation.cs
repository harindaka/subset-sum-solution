namespace ConsoleApp1
{
    public class MinOverCancellation
    {     
        public record ExceedingOrderSubset(int ExceedsBy, List<Order> Orders);

        public static List<int> Collect(List<Order> orders, int cancelTarget)
        {
            //Assessment mentions to assume orders share the same price thus validate this
            if (orders.GroupBy(o => o.Price).Count() > 1)
            {
                throw new ArgumentException("Price needs to be the same in specified orders list", nameof(orders));
            }

            var sortedOrders = orders.OrderBy(o => o.Quantity).ToList().AsReadOnly();
            var root = new QuantityAssociation
            {
                TargetIndex = -1,
                TotalWeight = 0
            };

            //Start calculating total weight for each subset combination recursively
            var exceedingOrderSubsets = new List<ExceedingOrderSubset>();
            CalculateExceedingOrderSubsets(root, cancelTarget, sortedOrders, exceedingOrderSubsets);

            //Pick the one with the minimum "exceeds by" value
            var orderSubsetWithMinExcessiveSum = exceedingOrderSubsets.MinBy(oa => oa.ExceedsBy);
            if (orderSubsetWithMinExcessiveSum == null)
            {
                return new List<int>();
            }

            return orderSubsetWithMinExcessiveSum.Orders.Select(o => o.Id).Order().ToList();
        }

        private static void CalculateExceedingOrderSubsets(QuantityAssociation currentNode, int cancelTarget,
            IReadOnlyList<Order> sortedOrders, List<ExceedingOrderSubset> exceedingOrderSubsets)
        {
            //Calculate the next element index and exit if at the bottom of the tree
            int nextIndex = currentNode.TargetIndex + 1;
            if (nextIndex >= sortedOrders.Count)
            {
                return;
            }

            //Calculate and set the next weight if the current element is included
            currentNode.Incusion = new QuantityAssociation
            {
                Parent = currentNode,
                TargetIndex = nextIndex,
                IsIncluded = true,
                TotalWeight = currentNode.TotalWeight + sortedOrders[nextIndex].Quantity
            };

            //Check to see if running total so fax exceeds cancel target
            //If so add to list
            if (currentNode.Incusion.TotalWeight >= cancelTarget)
            {
                List<Order> exceedingOrders = new List<Order>();
                PopulateOrderSubsetFromNode(exceedingOrders, currentNode.Incusion, sortedOrders);
                var exceedingOrderSubset = new ExceedingOrderSubset(currentNode.Incusion.TotalWeight - cancelTarget, exceedingOrders);
                exceedingOrderSubsets.Add(exceedingOrderSubset);
                
                //Check if running total exactly matches cancel target which is ideal
                //If so exit as there's no point in continuing the calculation
                if(exceedingOrderSubset.ExceedsBy == 0)
                {
                    return;
                }
            }

            //Add a node to represent the next possible subset if the current element is excluded
            currentNode.Exclusion = new QuantityAssociation
            {
                Parent = currentNode,
                TargetIndex = nextIndex,
                IsIncluded = false,
                TotalWeight = currentNode.TotalWeight
            };

            //Recursively calculate other subsets
            CalculateExceedingOrderSubsets(currentNode.Incusion, cancelTarget, sortedOrders, exceedingOrderSubsets);
            CalculateExceedingOrderSubsets(currentNode.Exclusion, cancelTarget, sortedOrders, exceedingOrderSubsets);
        }

        private static void PopulateOrderSubsetFromNode(IList<Order> orderSubset, QuantityAssociation? currentNode, 
            IReadOnlyList<Order> sortedOrders)
        {
            //Exit on root
            if (currentNode is null || currentNode.TargetIndex < 0)
            {
                return;
            }

            if (currentNode.IsIncluded)
            {
                orderSubset.Add(sortedOrders[currentNode.TargetIndex]);
            }

            PopulateOrderSubsetFromNode(orderSubset, currentNode.Parent, sortedOrders);
        }
    }
}
