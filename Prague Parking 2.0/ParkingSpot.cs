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

        public bool Occupied { get; set; } = false;
        public Vehicle? ParkedVehicle { get; set; }
        public int SpotCapacity { get; set; } = 4;

        public int UsedCapacity { get; set; } = 0;

        //public ParkingSpot[] parkingSpots = new ParkingSpot[4];

 
        
        //public static ParkingSpot Spot(Vehicle vehicle)
        //{
        //    var spot = new ParkingSpot(vehicle);
        //    return spot;
        //}

        public ParkingSpot(int id)
        {
            //this.ParkedVehicle = vehicle;
            this.ID = id;
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

        public bool CanAcceptVehicle(Vehicle vehicle)
        {
            // Bussar kan endast parkera på platser 0-49
            if (vehicle.Type == "Bus" && ID >= 50)
            {
                return false;
            }

            // Spots 0-49 kan acceptera alla typer av fordon
            if (ID < 50)
            {
                return true;
            }
            else
            {
                // Spots 50-99 kan bara acceptera Bike, MC och Car
                return vehicle.Type == "Bike" ||
                       vehicle.Type == "MC" ||
                       vehicle.Type == "Car";
            }
        }
    }
}
