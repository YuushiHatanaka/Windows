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
        public string OldStationName { get; set; } = string.Empty;

        public string NewStationName { get; set; } = string.Empty;

        public StationProperties OldProperties { get; set; } = new StationProperties();

        public StationProperties Properties { get; set; } = null;

        public StationSequenceProperties Sequences { get; set; } = null;
    }
}
