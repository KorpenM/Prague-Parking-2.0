using System.Reflection.Metadata.Ecma335;

namespace PragueParking_2._0
{
    internal class ParkingSpot
    {
        public int ID { get; set; }
        public bool Occupied { get; set; }
        public Vehicle? ParkedVehicle { get; set; } 

        public ParkingSpot()
        {
            this.Occupied = false;
        }

        public virtual void ParkVehicle()
        {
            Console.WriteLine("The vehicle is now parked");
        }

        public bool CanAcceptVehicle(Vehicle vehicle)
        {
            if (ID < 50)
            {
                return true;
            }
            else
            {
                // Spots 51-100 (ID 50-99) can only accept Bike, MC and Car
                return vehicle.TypeOfVehicle == VehicleType.Bike ||
                       vehicle.TypeOfVehicle == VehicleType.MC ||
                       vehicle.TypeOfVehicle == VehicleType.Car;
            }
        }

    }
}
