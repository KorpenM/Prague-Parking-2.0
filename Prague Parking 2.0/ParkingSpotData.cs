using System.Collections.Generic;

namespace Prague_Parking_2._0
{
    public class ParkingSpotData
    {
        public int ID { get; set; }
        public List<VehicleData> Vehicles { get; set; }

        public ParkingSpotData()
        {
            Vehicles = new List<VehicleData>(); // Initiate list
        }
    }
}
