namespace Robot.Common
{
    static class Constants
    {
        public const bool LogToFile = false;

        public const int MapSize = 100;

        public const int InitialRobotsCount = 10;
        public const int MaxRobotsCount = 100;

        public const int InitialRobotEnegrgy = 100;

        public const int MaxEnergyGrowth = 50;
        public const int MinEnergyGrowth = 10;

        public const int MaxStationEnergy = 20000;

        public const int CollectingDistance = 4;
        public const int CollectingRadius = 2;
        public const int MaxEnergyCanCollect = 500;

        public const int EnergyLossToCreateNewRobot = 1000;

        public const float StoleRateEnergyAtAttack = 0.3f;
        public const int AttackEnergyLoss = 20;

        public const int MinEnegryAfterClone = 125;

        public const int MinEnegryAfterMove = 25;

        public const int EnegrgySpentToCreateRobot = InitialRobotEnegrgy + EnergyLossToCreateNewRobot;

        public const int RoundsCount = 50;
        public const int RushRoundsCount = 5;
        public const int BaseRoundsCount = RoundsCount - RushRoundsCount;

        public const int AttackMoveDistance = 3;
        public const int AttackMoveEnergy = 9;

        public const int MaxMovementDisplacement = 2;

        public const string OwnerName = "Andrii Shpek";

    }
}
