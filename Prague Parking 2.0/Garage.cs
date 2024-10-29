namespace PragueParking_2._0
{
    public class Garage : ParkingSpot
    {
        public int listSize = 101;
        public List<string> parkingList = new List<string>();

        public void createParking()
        {
            for (int i = 1; i < listSize; i++)
            {
                string parkingNumber = i.ToString();
                parkingList.Add(parkingNumber);
            }
        }

        public void addToParking(string vehicleInfo, string moveTo)
        {
            for (int i = 1; i < listSize; i++)
            {
                int index = parkingList.FindIndex(s => s == moveTo);

                if (index != -1)
                {
                    parkingList[index] = vehicleInfo;

                    Console.WriteLine("{0} has been placed at parking spot {1}", vehicleInfo, moveTo);
                }
            }
        }

        public void printParking()
        {
            parkingList.ForEach(Console.WriteLine);
        }
    }
}
