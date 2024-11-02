using System.Reflection.Metadata.Ecma335;

namespace PragueParking_2._0
{
    internal class ParkingSpot
    {

        private int spotCapacity = 4;

        private int available;

        private bool occupied;

        private int usedCapacity;

        private int vehicleSpace;

        public List<Vehicle> Spots { get; set; }
        public int ID { get; set; }

        public Vehicle? ParkedVehicle { get; set; }

        public int SpotCapacity

        {
            get { return spotCapacity; }
            set { spotCapacity = value; }
        }



        public int UsedCapacity
        {
            get { return usedCapacity; }
            set { usedCapacity = value; }
        }



        public bool Occupied { get; set; } = false;


        public void SetVehicleSpace(Vehicle vehicle)
        {
            vehicleSpace = vehicle.Space;
        }
        public int Available
        {
            get
            {
                if (Spots.Count == 0)
                {
                    return available = 4;
                }
                else
                {
                    return available = spotCapacity - usedCapacity;
                }
            }
            set { available = value; }
        }






        public ParkingSpot(int id)
        {
            //this.ParkedVehicle = vehicle;
            this.ID = id;
            this.Occupied = false;
            this.Spots = new List<Vehicle>();
            //this.Available = 4;
            this.SpotCapacity = spotCapacity;
            this.UsedCapacity = usedCapacity;

        }



        public virtual void ParkVehicle()
        {
            Console.WriteLine("ParkVehicle in ParkingSPot");
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




        /*public bool CanAcceptVehicle(Vehicle vehicle)
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
        }*/

    }
}