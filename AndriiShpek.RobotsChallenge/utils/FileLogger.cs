using System;
using System.IO;

namespace Robot.Common
{
    public static class FileLogger
    {

        private static StreamWriter writer;

        public static void Log(String line = "")
        {
            if (!Constants.LogToFile) return;

            if (writer == null) writer = File.AppendText("AndriiShpekLog.txt");

            writer.WriteLine(line); 
        }

    }
}
