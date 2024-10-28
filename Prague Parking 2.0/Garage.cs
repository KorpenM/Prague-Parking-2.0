namespace PragueParking_2._0
{
    internal class Garage
    {
        private int capacity = 100;
        public List<ParkingSpot> garageList = [];

        public ParkingSpot? GarageList { get; set; }

        public static void InitGarage() //Creates a garage
        {
            Garage garageList = new Garage();
            garageList.InitializeGarage();
            garageList.PrintGarage();
        }

        public void InitializeGarage() //Adds ParkingSpots to garage
        {
            for (int i = 0; i < capacity; i++)
            {
                garageList.Add(new ParkingSpot());
                garageList[i].ID = i;
                garageList[i].Occupied = false;
            }
        }

        public void PrintGarage()   //Prints each ParkingSpot to console
        {
            foreach (ParkingSpot spot in garageList)
            {
                Console.WriteLine(spot.ID + " " + spot.Occupied);
            }
        }
    }
}
