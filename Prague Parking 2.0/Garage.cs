namespace PragueParking_2._0
{
    public class Garage : ParkingSpot
    {
        private static int listSize = 100;
        private List<string> parkingList = new List<string>();

        public void createParking()
        {
            for (int i = 0; i < listSize; i++)
            {
                int parkingNumber = i + 1;
                parkingList.Add(parkingNumber.ToString());
            }
        }

        public void addToParking(string vehicleInfo, string moveTo)
        {
            for (int i = 0; i < listSize; i++)
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
            for (int i = 0; i < listSize; i++)
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

        public void moveParking()
        {

        }

        public void searchForParking(string vehicleInfo)
        {
            for (int i = 0; i < listSize; i++)
            {
                if (parkingList[i] == vehicleInfo)
                {
                    int vehicleNumber = i + 1;

                    Console.WriteLine("{0} was found at parking spot {1}", vehicleInfo, vehicleNumber);
                    return;
                }
            }
            Console.WriteLine("{0} couldn't be found in any of the parking spots", vehicleInfo);
        }

        public void printParking()
        {
            for (int i = 0; i < listSize;i++)
            {
                string splitString = parkingList[i];
                int searchVehicle = splitString.IndexOf('#');
                int searchSeperator = splitString.IndexOf('|');
                int searchMC = splitString.IndexOf("MC");

                if (searchVehicle != -1)
                {
                    if (searchMC != -1 && searchSeperator == -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(parkingList[i]);
                        Console.ResetColor();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(parkingList[i]);
                    Console.ResetColor();
                }
                else if (searchVehicle == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(parkingList[i]);
                    Console.ResetColor();
                }
            }
        }
    }
}
