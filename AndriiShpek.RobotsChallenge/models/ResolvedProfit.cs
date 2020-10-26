namespace Robot.Common
{
    class ResolvedProfit<T>
    {

        public readonly T Item;
        public readonly ProfitData Profit;

        public ResolvedProfit(T item, ProfitData profit)
        {
            Item = item;
            Profit = profit;
        }

        public override string ToString()
        {
            return $"{Item}. Profit: ${Profit}";
        }
    }
}
