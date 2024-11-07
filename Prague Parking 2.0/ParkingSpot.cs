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

        /*public void ResetSpot()
        {
            UsedCapacity = 0;
            Available = SpotCapacity - UsedCapacity;

            if (Available > 0)
            {
                Occupied = false;
            }
        }*/

        public void ResetSpot()
        {
            // Återställ UsedCapacity för alla parkerade fordon på denna plats
            UsedCapacity = 0;
            Available = SpotCapacity - UsedCapacity;

            // Om det finns någon ledig plats, markera som icke-ockuperad
            Occupied = Available != SpotCapacity;  // Om inte hela platsen är ledig, markera som ockuperad
        }


        /*public void UpdateSpot(Vehicle vehicle)
        {
            UsedCapacity += vehicle.Space;
            Available = SpotCapacity - UsedCapacity;

            if (Available == 0)
            {
                Occupied = true;
            }
        }*/

        public void UpdateSpot(Vehicle vehicle)
        {
            // Om det finns flera parkeringsplatser för denna fordonstyp (t.ex. en buss)
            UsedCapacity += vehicle.Space;
            Available = SpotCapacity - UsedCapacity;

            // Om hela platsen är ockuperad, sätt Occupied till true
            Occupied = UsedCapacity >= SpotCapacity;
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