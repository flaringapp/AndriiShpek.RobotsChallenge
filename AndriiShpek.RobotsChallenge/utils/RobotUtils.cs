using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    public static class RobotUtils
    {
        public static IList<Robot> FilterForeignRobots(this IList<Robot> robots)
        {
            return robots.Where(robot => !robot.IsMine()).ToList();
        }

        public static bool IsMine(this Robot robot) => robot.OwnerName == Constants.OwnerName;
    }
}
