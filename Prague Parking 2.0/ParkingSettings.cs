using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking_2._0
{
    public class ParkingSettings
    {
        public int TotalSpots { get; set; }
        public Dictionary<string, VehicleTypeInfo> VehicleTypes { get; set; }
        public int FreeParkingMinutes { get; set; }
        public Instructions Instructions { get; set; }
        public int WhichVehicle {  get; set; }

    }

    public class VehicleTypeInfo
    {
        public int SpaceRequired { get; set; }
        public int RatePerHour { get; set; }
        public string AllowedSpots { get; set; }
        public int NumberOfVehiclesPerSpot { get; set; }
    }


    public class Instructions
    {
        public string General { get; set; }
        public Dictionary<string, string> VehicleSizeInfo { get; set; }
        public string Rules { get; set; }
    }
}