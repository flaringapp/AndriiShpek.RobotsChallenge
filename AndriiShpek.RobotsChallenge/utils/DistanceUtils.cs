using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    class DistanceUtils
    {
        public static float DistanceBetweenPoints(Position first, Position second)
        {
            return (float)Math.Sqrt(ResolveDistanceEnergy(first, second));
        }

        public static int ResolveDistanceEnergy(Position first, Position second)
        {
            return (int)(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }

        public static int DistanceFromStation(Position position, Position stationPosition)
        {
            return Math.Max(Math.Abs(position.X - stationPosition.X), Math.Abs(position.Y - stationPosition.Y));
        }

        public static List<T> FilterByAccessibleRange<T>(
            Position targetPosition, 
            IList<T> items, 
            Func<T, Position> resolveItemPosition
        ) {
            return items.Where(item => DistanceBetweenPoints(targetPosition, resolveItemPosition(item)) <= Constants.MaxNearbyRadius).ToList();
        }

        public static Position ResolveMovePosition(Position from, Position to, int stepsCount, Map map, IList<Robot> robots)
        {
            float x = from.X + (to.X - from.X) / stepsCount;
            float y = from.Y + (to.X - from.X) / stepsCount;

            Position position = new Position((int)Math.Round(x), (int)Math.Round(y));
            return map.FindFreeCell(position, robots);
        }

        public static int ResolveMoveEnergy(Position from, Position to, int stepsCount, Map map, IList<Robot> robots)
        {
            IList<Position> positions = new List<Position>();
            positions.Add(from);
            for (int i = 0; i < stepsCount; i++)
            {
                positions.Add(ResolveMovePosition(positions[i], to, stepsCount - i, map, robots));
            }
            positions.Add(to);

            int energy = 0;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                energy += ResolveDistanceEnergy(positions[i], positions[i + 1]);
            }

            return energy;
        }
    }
}
