using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// DateTimeLibraryクラス
    /// </summary>
    public class DateTimeLibrary
    {
        public static string GetTimeString(string baseStrings)
        {
            if (baseStrings.Length == 3)
            {
                baseStrings = "0" + baseStrings;
            }
            return baseStrings;
        }
    }
}
