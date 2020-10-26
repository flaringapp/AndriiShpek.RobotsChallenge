using System;

namespace Robot.Common 
{
    class ProfitData : IComparable<ProfitData>
    {

        public readonly int Profit;
        public readonly float Distance;
        public readonly int PreferredStepsCount;
        public readonly Position MovePosition;

        public ProfitData(
            int profit, 
            float distance,
            int preferredStepsCount,
            Position movePosition
        ) {
            Profit = profit;
            Distance = distance;
            PreferredStepsCount = preferredStepsCount;
            MovePosition = movePosition;
        }

        public int CompareTo(ProfitData other)
        {
            if (Profit > other.Profit) return 1;
            else if (Profit < other.Profit) return -1;
            else return 0;
        }

        public override string ToString()
        {
            return $"Total profit: {Profit}, " +
                $"Distance: {Distance}, " +
                $"Preferred steps count: {PreferredStepsCount}, " +
                $"Move position: {MovePosition}";
        }
    }
}
