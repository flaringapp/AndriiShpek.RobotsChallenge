namespace Robot.Common
{
    public static class FormattingUtils
    {

        public static string Format(this Robot robot) => $"Pos. [{robot.Position.X};{robot.Position.Y}] - en. {robot.Energy}";

        public static string Format(this EnergyStation station) => $"Pos. [{station.Position.X};{station.Position.Y}] - en. {station.Energy}";

    }
}
