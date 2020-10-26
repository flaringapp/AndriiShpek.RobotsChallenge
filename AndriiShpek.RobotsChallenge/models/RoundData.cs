using System.Collections.Generic;

namespace Robot.Common
{
    class RoundData
    {

        public readonly Map Map;
        public readonly IList<Robot> Robots;
        public readonly Robot MyRobot;
        public readonly Position MyPosition;
        public readonly int MyEnergy;

        public RoundData(Map map, IList<Robot> robots, Robot myRobot)
        {
            Map = map;
            Robots = robots;
            MyRobot = myRobot;
            MyPosition = myRobot.Position;
            MyEnergy = myRobot.Energy;
        }

    }
}
