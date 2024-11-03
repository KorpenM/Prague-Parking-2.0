using System;
using System.Collections.Generic;

namespace Prague_Parking_2._0
{
    public class ParkingData
    {
        public List<ParkingSpotData> ParkingSpots { get; set; }

        public ParkingData()
        {
            ParkingSpots = new List<ParkingSpotData>();
        }
    }
}