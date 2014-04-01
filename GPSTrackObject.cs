using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class GPSTrackObject
    {
        Double latitude;
        Double longitude;
        Double unknownParam;

        public GPSTrackObject(Double _lat, Double _lon, Double _unk)
        {
            latitude = _lat;
            longitude = _lon;
            unknownParam = _unk;
        }
    }
}
