using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.EventArgs
{
    public class StationPropertiesUpdateEventArgs : System.EventArgs
    {
        public StationProperties Property { get; set; } = null;
    }
}
