namespace ConsoleApp1
{
    public class InternalTransfer
    {
        public static List<int> Collect(List<Order> orders)
        {
            //Assessment mentions to assume orders share the same price thus validate this
            //Alternatively each price group can be subjected to the funtion below individually
            if(orders.GroupBy(o => o.Price).Count() > 1)
            {
                throw new ArgumentException("Price needs to be the same in specified orders list", nameof(orders));
            }

            //var totalQuantity = orders.Sum(o => o.Quantity);
            var sortedOrders = orders.OrderBy(o => o.Quantity).ToList().AsReadOnly();
            var root = new QuantityAssociation
            {
                TargetIndex = -1,
                TotalWeight = 0,
                //RemainingWeight = totalQuantity
            };

            //Start calculating total weight for each subset combination recursively
            List<List<Order>> zeroSumSubsets = new List<List<Order>>();
            CalculateZeroSumOrderSubsets(root, sortedOrders, zeroSumSubsets);
            
            //Pick the one with the most number of elements
            var subsetWithMostOrders = zeroSumSubsets.MaxBy(oa => oa.Count);
            if (subsetWithMostOrders == null)
            {
                return new List<int>();
            }

            return subsetWithMostOrders.Select(o => o.Id).Order().ToList();
        }

        private static void CalculateZeroSumOrderSubsets(QuantityAssociation currentNode, 
            IReadOnlyList<Order> sortedOrders, List<List<Order>> zeroSumOrderSubsets)
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
                TotalWeight = currentNode.TotalWeight + sortedOrders[nextIndex].Quantity,
                //Possible optimization here. Needs more thought / time
                //RemainingWeight = currentNode.RemainingWeight - sortedOrders[nextIndex].Quantity
            };

            //Check if the total weight sums up to zero. If so we have a result
            //Add it to the list and proceed with the calculation
            if (currentNode.Incusion.TotalWeight == 0)
            {
                var orderSubset = new List<Order>();
                PopulateOrderSubsetFromNode(orderSubset, currentNode.Incusion, sortedOrders);
                zeroSumOrderSubsets.Add(orderSubset.ToList());
            }

            //Add a node to represent the next possible subset if the current element is excluded
            currentNode.Exclusion = new QuantityAssociation
            {
                Parent = currentNode,
                TargetIndex = nextIndex,
                IsIncluded = false,
                TotalWeight = currentNode.TotalWeight, //No change to the total weight since the current element is exluded
                //Possible optimization here. Needs more thought / time
                //RemainingWeight = currentNode.RemainingWeight - sortedOrders[nextIndex].Quantity
            };

            //Recursively calculate other subsets
            CalculateZeroSumOrderSubsets(currentNode.Incusion, sortedOrders, zeroSumOrderSubsets);
            CalculateZeroSumOrderSubsets(currentNode.Exclusion, sortedOrders, zeroSumOrderSubsets);
        }

        private static void PopulateOrderSubsetFromNode(IList<Order> orderSubset, QuantityAssociation? currentNode, IReadOnlyList<Order> sortedOrders)
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
