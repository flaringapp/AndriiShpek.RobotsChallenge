using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    public class AndriiShpekAlgorithm : IRobotAlgorithm
    {

        private readonly IProfitResolver<EnergyStation> stationProfitResolver = new EnergyStationProfitResolver();
        private readonly IProfitResolver<Robot> robotProfitResolver = new RobotProfitResolver();

        private int currentRound = 0;

        int robotsCount = Constants.InitialRobotsCount;

        // TODO taret stations and robots

        public AndriiShpekAlgorithm()
        {
            Logger.OnLogRound += (s, e) => currentRound++;
            Logger.OnLogMessage += (s, e) => FileLogger.Log($"{e.OwnerName} --- {e.Message}");
        }
        public string Author => "Andrii Shpek";

        public RobotCommand DoStep(IList<Robot> robots, int robotToMoveIndex, Map map)
        {
            FileLogger.Log("\n\nMY MOVE\n\n");

            Robot myRobot = robots[robotToMoveIndex];

            if (CanClone(myRobot))
            {
                FileLogger.Log("Decided to clone");
                robotsCount++;
                return new CreateNewRobotCommand();
            }

            RoundData data = new RoundData(map, robots, myRobot);

            ResolvedProfit<EnergyStation> bestStation = FindMostProfitableItem(data, stationProfitResolver);
            ResolvedProfit<Robot> bestRobot = FindMostProfitableItem(data, robotProfitResolver);

            FileLogger.Log($"Round {currentRound}, Robot #{robotToMoveIndex} - {myRobot.Format()}");
            FileLogger.Log($"Best station: {bestStation.Item.Format()} with profit {bestStation.Profit}");
            FileLogger.Log($"Best robot: {bestRobot.Item.Format()} with profit {bestRobot.Profit}");

            if (bestStation == null && bestRobot == null) return new MoveCommand() { NewPosition = data.MyPosition };

            int accessibleEnergy = ResolveAccessibleStationEnergy(data.MyPosition, data.Map.Stations);
            if (accessibleEnergy > bestStation?.Profit?.Profit && accessibleEnergy > bestRobot?.Profit?.Profit)
            {
                FileLogger.Log("Decided to collect energy now");
                return new CollectEnergyCommand();
            }

            if (bestStation?.Profit?.CompareTo(bestRobot.Profit) > 0)
            {
                FileLogger.Log("Decided to work with station");
                return stationProfitResolver.ProcessStep(data, bestStation.Item, bestStation.Profit);
            } else
            {
                FileLogger.Log("Decided to work with robot");
                return robotProfitResolver.ProcessStep(data, bestRobot.Item, bestRobot.Profit);
            }
        }

        private ResolvedProfit<T> FindMostProfitableItem<T>(RoundData data, IProfitResolver<T> resovler)
        {
            IList<T> items = resovler.ResolveAccessibleItems(data); 
            if (items.Count == 0) return null;

            T bestItem = items.First();
            ProfitData bestProfit = resovler.CalculateProfit(data, bestItem);

            for (int i = 1; i < items.Count; i++)
            {
                // TODO multiple best items
                T item = items[i];
                ProfitData profit = resovler.CalculateProfit(data, item);
                if (profit.CompareTo(bestProfit) > 0)
                {
                    bestItem = item;
                    bestProfit = profit;
                }
            }

            return new ResolvedProfit<T>(bestItem, bestProfit);
        }

        private bool CanClone(Robot robot)
        {
            if (robotsCount == Constants.MaxRobotsCount) return false;
            if (currentRound > Constants.BaseRoundsCount) return false;
            return robot.Energy - Constants.MinEnegryAfterClone > Constants.EnegrgySpentToCreateRobot;
        }

        private int ResolveAccessibleStationEnergy(Position position, IList<EnergyStation> stations)
        {
            return stations.Where(station =>
                Math.Abs(position.X - station.Position.X) <= Constants.CollectingRadius &&
                Math.Abs(position.Y - station.Position.Y) <= Constants.CollectingRadius
            ).Sum(station => Math.Min(Constants.MaxEnergyCanCollect, station.Energy));
        } 
    }

}