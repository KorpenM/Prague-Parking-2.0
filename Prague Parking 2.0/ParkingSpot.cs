using System.Reflection.Metadata.Ecma335;

namespace Prague_Parking_2._0
{
    internal class ParkingSpot
    {

        //Remoce fields
       

        private int available;

        private bool occupied;

        private int usedCapacity;

        private int vehicleSpace;

        public List<Vehicle> Spots { get; set; }
        public int ID { get; set; }

        public Vehicle? ParkedVehicle { get; set; }

        public int SpotCapacity { get; set; } = 4;




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

        //Set value in other method
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
                    return available = SpotCapacity - usedCapacity;
                }
            }
            set { available = value; }
        }






        public ParkingSpot(int id)
        {
            //this.ParkedVehicle = vehicle;
            ID = id;
            Occupied = false;
            Spots = new List<Vehicle>();
            //this.Available = 4;
            SpotCapacity = spotCapacity;
            UsedCapacity = usedCapacity;

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