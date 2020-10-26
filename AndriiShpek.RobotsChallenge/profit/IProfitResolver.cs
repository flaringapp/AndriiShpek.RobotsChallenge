using System.Collections;
using System.Collections.Generic;

namespace Robot.Common
{
    interface IProfitResolver<T>
    {
        IList<T> ResolveAccessibleItems(RoundData data);

        ProfitData CalculateProfit(RoundData data, T item);

        RobotCommand ProcessStep(RoundData data, T item, ProfitData profit);
    }
}
