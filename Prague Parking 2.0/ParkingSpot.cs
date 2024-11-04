using PragueParking_2._0;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Prague_Parking_2._0
{
    public class ParkingSpot
    {
        public int ID { get; set; }
        public List<Vehicle> Spots { get; set; }
        public int SpotCapacity { get; set; } = 4;
        public int UsedCapacity { get; set; } = 0;
        public int Available { get; set; } = 4;
        public bool Occupied { get; set; } = false;

        public bool CanPark { get; set; } = true;



        public ParkingSpot(int id)
        {
            ID = id;
            Spots = new List<Vehicle>();
        }

        public void CheckAvailability(Vehicle vehicle)
        {
            if (Available == 0 || vehicle.Space > Available)
            {
                CanPark = false;
            }
            else if (vehicle.Space <= Available)
            {
                CanPark = true;
            }
        }


        public void Add(Vehicle vehicle)
        {
            if (vehicle.Space <= Available)
            {
                Spots.Add(vehicle);
                UsedCapacity += vehicle.Space;
                Available = SpotCapacity - UsedCapacity;
            }
            else
            {
                Console.WriteLine("No spots available.");
            }
        }

        public void Remove(Vehicle vehicle)
        {
            Spots.Remove(vehicle);
            UsedCapacity -= vehicle.Space;
            Available = SpotCapacity - UsedCapacity;

        }

        public override string ToString()
        {
            string[] info = [];

            foreach (Vehicle vehicle in Spots)
            {
                string s = vehicle.RegNumber + " " + vehicle.Space + " " + vehicle.GetType().Name;
                info.Append(s);
            }

            return info.ToString();


        }
    }
}