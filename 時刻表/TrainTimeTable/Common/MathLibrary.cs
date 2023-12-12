using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// MathLibraryクラス
    /// </summary>
    public class MathLibrary
    {
        public static double CalculateAngle(Point point1, Point point2)
        {
            // Calculate the angle in radians
            double angleRadians = Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            // Convert radians to degrees
            double angleDegrees = angleRadians * (180.0 / Math.PI);

            return angleDegrees;
        }
    }
}
