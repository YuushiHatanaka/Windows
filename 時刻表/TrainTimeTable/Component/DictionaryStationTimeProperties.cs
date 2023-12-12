using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.Component
{
    public class DictionaryStationTimeProperties : Dictionary<int, List<StationTimeProperty>>
    {
        public void Add(int hour, StationTimeProperty property)
        {
            if (!ContainsKey(hour))
            {
                Add(hour, new List<StationTimeProperty>());
            }
            this[hour].Add(property);
        }

        public int GetColumnMax()
        {
            int columnMax = 0;
            foreach (var properties in this)
            {
                if (this[properties.Key].Count > columnMax)
                {
                    columnMax = this[properties.Key].Count;
                }
            }
            return columnMax;
        }
    }
}
