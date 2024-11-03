
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
            Console.WriteLine("ParkVehicle in ParkingSPot");
        }

        public bool CanAcceptVehicle(Vehicle vehicle)
        {
            // Bus can only park at 1-50
            if (vehicle.TypeOfVehicle == VehicleType.Bus && ID >= 50)
            {
                return false;
            }

            // Spots 0-49 can accept all vehicles
            if (ID < 50)
            {
                return true;
            }
            else
            {
                // Spots 50-99 can only accept Bike, MC and Car
                return vehicle.TypeOfVehicle == VehicleType.Bike ||
                       vehicle.TypeOfVehicle == VehicleType.MC ||
                       vehicle.TypeOfVehicle == VehicleType.Car;
            }
        }
    }
}