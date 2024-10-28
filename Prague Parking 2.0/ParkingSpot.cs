namespace PragueParking_2._0
{
    internal class ParkingSpot : Garage
    {
        private int _id;
        private bool occupied;

        public int ID { get; set; }
        public bool Occupied { get; set; }
        public ParkingSpot()
        {
            this._id = ID;
            this.occupied = Occupied;
        }

        public virtual void ParkVehicle()
        {
            Console.WriteLine("The vehicle is now parked");
        }
    }
}
