using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    class PositionHelper
    {
        public static Position FindNearestStationPositon(Position currentPosition, Position stationPosition, IList<Robot> robots)
        {
            IList<Position> robotPositions = robots.Select(robot => robot.Position).ToList();
            return FindNearestStationPositon(currentPosition, stationPosition, robotPositions);
        }

        public static Position FindNearestStationPositon(Position currentPosition, Position stationPosition, IList<Position> blockedPositions)
        {
            return ResolveAccessibleStationPositions(stationPosition, blockedPositions)
                .MinBy(position => DistanceUtils.DistanceBetweenPoints(currentPosition, position));
        }

        public static IList<Position> ResolveAccessibleStationPositions(Position stationPosition, IList<Position> blockedPositions)
        {
            return ResolveAllStationPositions(stationPosition).Where(position => 
                !blockedPositions.Any(blockedPosition =>
                {
                    return position.X == blockedPosition.X && position.Y == blockedPosition.Y;
                })
            ).ToList();
        }

        public static IList<Position> ResolveAllStationPositions(Position stationPosition)
        {
            IList<Position> positions = new List<Position>();
            for (
                int x = Math.Max(0, stationPosition.X - Constants.CollectingRadius);
                x < Math.Min(Constants.MapSize - 1, stationPosition.X + Constants.CollectingRadius);
                x++
            ) {
                for (
                    int y = Math.Max(0, stationPosition.Y - Constants.CollectingRadius);
                    x < Math.Min(Constants.MapSize - 1, stationPosition.Y + Constants.CollectingRadius);
                    x++
                ) {
                    positions.Add(new Position(x, y));
                }
            }
            return positions;
        }
    }
}
