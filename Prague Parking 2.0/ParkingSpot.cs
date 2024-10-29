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
    }
}
