using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    class EnergyStationProfitResolver: IProfitResolver<EnergyStation>
    {
        private static float[] AverageStepDistances = new float[4] { 1f, 1.42f, 2.24f, 2.83f };

        public IList<EnergyStation> ResolveAccessibleItems(RoundData data)
        {
            return DistanceUtils.FilterByAccessibleRange(
                data.MyPosition,
                data.Map.Stations,
                new Func<EnergyStation, Position>(station => station.Position),
                data.MyEnergy
            );
        }

        public ProfitData CalculateProfit(RoundData data, EnergyStation station)
        {
            float distance = DistanceUtils.DistanceBetweenPoints(data.MyPosition, station.Position);

            KeyValuePair<float, int> bestStepDistanceToProfit = AverageStepDistances.ToDictionary(
                stepDitance => stepDitance,
                stepDistance => ResolveProfitWithAverageStepDistance(station, distance, stepDistance)
            )
                .ToList()
                .MaxBy(entry => entry.Value);

            float averageStepDistance = bestStepDistanceToProfit.Key;
            int stepsCount = (int)Math.Ceiling(distance / averageStepDistance);
            if (stepsCount == 0) stepsCount = 1;

            int profit = bestStepDistanceToProfit.Value;

            Position positionToCollectEnergy = PositionHelper.FindNearestStationPositon(
                    data.MyPosition,
                    station.Position,
                    data.Robots.FilterForeignRobots(),
                    data.Map.Stations
            );
            Position movePosition;

            if (positionToCollectEnergy == null)
            {
                movePosition = data.MyPosition;
            }
            else movePosition = DistanceUtils.ResolveMovePosition(
                data.MyPosition,
                positionToCollectEnergy,
                stepsCount,
                data.Map,
                data.Robots
            );

            if (movePosition == null)
            {
                FileLogger.Log("Failed to find move position");
                movePosition = data.MyPosition;
            }
            
            float moveDistance = DistanceUtils.DistanceBetweenPoints(data.MyPosition, movePosition);
            float maxMoveDistanceWithDisplacement = distance / stepsCount + Constants.MaxMovementDisplacement;
            if (moveDistance > maxMoveDistanceWithDisplacement && movePosition != positionToCollectEnergy)
            {
                FileLogger.Log("Availabel move position is too far");
                movePosition = data.MyPosition;
            }

            return new ProfitData(
                profit - DistanceUtils.ResolveMoveEnergy(data.MyPosition, station.Position, stepsCount, data.Map, data.Robots),
                distance,
                stepsCount,
                movePosition
            );
        }

        private int ResolveProfitWithAverageStepDistance(EnergyStation station, float distance, float stepDistance)
        {
            return ResolveTotalStationProfit(station, (int)Math.Ceiling(distance / stepDistance));
        }

        private int ResolveTotalStationProfit(EnergyStation station, int stepsCount)
        {
            int profit = 0;
            int currentStationEnergy = station.Energy;
            int round = stepsCount;

            int lastEnergyCollect = Math.Min(currentStationEnergy, Constants.MaxEnergyCanCollect);

            while (currentStationEnergy > Constants.AttackEnergyLoss)
            {
                profit += (int)(lastEnergyCollect * Math.Min(1f, 1.75f / round));
                currentStationEnergy -= lastEnergyCollect;
                round++;

                lastEnergyCollect = (int)(currentStationEnergy * Constants.StoleRateEnergyAtAttack);
            }

            return profit;
        }

        public RobotCommand ProcessStep(RoundData data, EnergyStation station, ProfitData profit)
        {
            if (DistanceUtils.DistanceFromStation(data.MyPosition, station.Position) <= Constants.CollectingRadius)
            {
                FileLogger.Log("Collecting energy from stations nearby");
                return new CollectEnergyCommand();
            }
            else
            {
                FileLogger.Log($"Moving to station {station.Format()} at position {profit.MovePosition}");
                return new MoveCommand() { NewPosition = profit.MovePosition };
            }
        }
    }
}
