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
                    return;
                }
            }
            Console.WriteLine("Parking spot {0} is currently occupied and can not house another vehicle", moveTo);
        }

        public void removeFromParking(string vehicleInfo)
        {
            for (int i = 1; i < listSize; i++)
            {
                if (parkingList[i] == vehicleInfo)
                {
                    int vehicleNumber = i + 1;
                    parkingList[i] = vehicleNumber.ToString();

                    Console.WriteLine("{0} was removed from parking spot {1}", vehicleInfo, vehicleNumber);
                    return;
                }
            }
            Console.WriteLine("{0} couldn't be found in any of the parking spots", vehicleInfo);
        }

        public void printParking()
        {
            parkingList.ForEach(Console.WriteLine);
        }
    }
}
