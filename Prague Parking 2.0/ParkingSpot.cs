using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageTest")]
namespace Prague_Parking_2._0
{
    public class ParkingSpot
    {
        public int VehicleSpace { get; set; }

        public List<Vehicle> Spots { get; set; }
        public int ID { get; set; }

        public Vehicle? ParkedVehicle { get; set; }

        public int SpotCapacity { get; set; } = 4;

        public bool Occupied { get; set; } = false;

        public int UsedCapacity { get; set; } = 0;
        public int Available { get; set; } = 4;

        public void SetVehicleSpace(Vehicle vehicle)
        {
            VehicleSpace = vehicle.Space;
        }

        public ParkingSpot(int id)
        {
            ID = id;
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

        //public bool CanAcceptVehicle(Vehicle vehicle)
        //{
        //    // Bussar kan endast parkera på platser 0-49
        //    if (vehicle.Type == "Bus" && ID >= 50)
        //    {
        //        return false;
        //    }

        //    // Spots 0-49 kan acceptera alla typer av fordon
        //    if (ID < 50)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        // Spots 50-99 kan bara acceptera Bike, MC och Car
        //        return vehicle.Type == "Bike" ||
        //               vehicle.Type == "MC" ||
        //               vehicle.Type == "Car";
        //    }
        //}
    }
}