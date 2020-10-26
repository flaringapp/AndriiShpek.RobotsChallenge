using System;
using System.Collections.Generic;

namespace Robot.Common
{
    class RobotProfitResolver : IProfitResolver<Robot>
    {
        public IList<Robot> ResolveAccessibleItems(RoundData data) {
            return DistanceUtils.FilterByAccessibleRange(
                data.MyPosition,
                data.Robots,
                new Func<Robot, Position>(robot => robot.Position),
                data.MyEnergy
            ).FilterForeignRobots();
        }

        public ProfitData CalculateProfit(RoundData data, Robot robot)
        {
            float distance = DistanceUtils.DistanceBetweenPoints(robot.Position, data.MyPosition);

            int stepsCount = CalculateBestMoveStepsCount(data, robot, distance);
            float speed = distance / stepsCount;

            int profit = ResolveTotalRobotProfit(robot, stepsCount);

            Position movePosition;
            if (stepsCount == 1) movePosition = robot.Position;
            else movePosition = DistanceUtils.ResolveMovePosition(
                data.MyPosition, 
                robot.Position, 
                stepsCount, 
                data.Map, 
                data.Robots
            );

            float moveDistance = DistanceUtils.DistanceBetweenPoints(data.MyPosition, movePosition);
            float maxMoveDistanceWithDisplacement = speed + Constants.MaxMovementDisplacement;
            if (moveDistance > maxMoveDistanceWithDisplacement && movePosition != robot.Position)
            {
                movePosition = data.MyPosition;
            }

            return new ProfitData(
                profit - DistanceUtils.ResolveMoveEnergy(data.MyPosition, robot.Position, stepsCount, data.Map, data.Robots),
                distance,
                stepsCount,
                movePosition
            );
        }

        private int CalculateBestMoveStepsCount(RoundData data, Robot robot, float distance)
        {
            float instEnergy = robot.Energy * Constants.StoleRateEnergyAtAttack - Constants.AttackEnergyLoss;

            int stepsCount = 1;
            int energyToMove;

            while (distance / stepsCount > Constants.AttackMoveDistance)
            {
                energyToMove = DistanceUtils.ResolveMoveEnergy(data.MyPosition, robot.Position, stepsCount, data.Map, data.Robots);
                if (instEnergy > energyToMove && energyToMove < data.MyEnergy - Constants.MinEnegryAfterMove) return stepsCount;
                stepsCount++;
            }
            return stepsCount;
        }

        private int ResolveTotalRobotProfit(Robot robot, int stepsCount)
        {
            int profit = 0;
            int currentRobotEnergy = robot.Energy;
            int round = stepsCount;

            int lastEnergySteal = (int)(currentRobotEnergy * Constants.StoleRateEnergyAtAttack);
            int lastEnergyStealCoef = (int)(lastEnergySteal * Math.Min(1f, 1.75f / round));

            while (lastEnergyStealCoef > Constants.AttackEnergyLoss) {
                profit += lastEnergyStealCoef;
                currentRobotEnergy -= lastEnergySteal;
                round++;

                lastEnergySteal = (int)(currentRobotEnergy * Constants.StoleRateEnergyAtAttack);
                lastEnergyStealCoef = (int)(lastEnergySteal * Math.Min(1f, 1.75f / round));
            }

            return profit;
        }

        public RobotCommand ProcessStep(RoundData data, Robot robot, ProfitData profit)
        {
            if (data.MyPosition == robot.Position) FileLogger.Log($"Attacking robot: {robot.Format()} at position {profit.MovePosition}");
            else FileLogger.Log($"Moving to robot {robot} at position {profit.MovePosition}");

            return new MoveCommand() { NewPosition = profit.MovePosition };
        }
    }
}
