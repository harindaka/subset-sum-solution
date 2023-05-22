namespace ConsoleApp1
{
    public class QuantityAssociation
    {
        public bool IsIncluded { get; init; }

        public QuantityAssociation? Parent { get; set; }
        public int TargetIndex { get; init; }
        public int TotalWeight { get; init; }
        
        //public int RemainingWeight { get; init; }

        public QuantityAssociation? Incusion { get; set; }
        public QuantityAssociation? Exclusion { get; set; }
    }
}
