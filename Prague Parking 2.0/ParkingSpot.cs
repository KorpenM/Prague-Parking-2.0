using PragueParking_2._0;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Prague_Parking_2._0
{
    public class ParkingSpot
    {
        public int ID { get; set; }
        public int VehicleSpace { get; set; }
        public List<Vehicle> Spots { get; set; }
        public Vehicle? ParkedVehicle { get; set; }
        public int SpotCapacity { get; set; } = 4;
        public int UsedCapacity { get; set; } = 0;
        public int Available { get; set; } = 4;
        public bool Occupied { get; set; } = false;

        public void SetVehicleSpace(Vehicle vehicle)
        {
            VehicleSpace = vehicle.Space;
        }

        public ParkingSpot(int id)
        {
            ID = id;
            Spots = new List<Vehicle>();
            Spots = new List<Vehicle>(); 
        }

        public void ResetSpot()
        {
            UsedCapacity = 0;
            Available = SpotCapacity - UsedCapacity;

            if (Available > 0)
            {
                Occupied = false;
            }
        }

        public void UpdateSpot(Vehicle vehicle)
        {
            UsedCapacity += vehicle.Space;
            Available = SpotCapacity - UsedCapacity;

            if (Available == 0)
            {
                Occupied = true;
            }
        }

        public override string ToString()
        {
            string info = "";

            foreach (Vehicle vehicle in Spots)
            {
                return info =  vehicle.RegNumber + " " + vehicle.Space + " " + vehicle.GetType().Name;
            }

            return info;
        }
    }
}