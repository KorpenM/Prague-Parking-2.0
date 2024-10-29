using PragueParking_2._0;

internal class Program
{
    private static void Main(string[] args)
    {
        Garage myPark = new Garage();
        myPark.createParking();

        int menu = 0;
        do
        {
            Console.Clear();

            Console.WriteLine("=== Welcome to Prague Parking ===");

            Console.WriteLine("\n\n[0] Exit Program");
            Console.WriteLine("[1] Park Vehicle");
            Console.WriteLine("[2] Remove Vehicle");
            Console.WriteLine("[3] Show Parking");

            Console.Write("\n\nType in the number of the corresponding function \nthat you'd like to use: ");

            menu = int.Parse(Console.ReadLine());
            switch (menu)
            {
                case 1:
                    addVehicle();
                    break;
                case 2:
                    removeVehicle();
                    break;
                case 3:
                    showParking();
                    break;
                case 0:
                    Console.Write("\n\nPress random key to exit program...");
                    Console.ReadKey();
                    break;
            }

        } while (menu != 0);

        void addVehicle()
        {
            Console.Clear();

            Console.Write("What type of vehicle are you trying to park? \n\n[1] Car \n[2] Motorcycle \n\nType in the number of the corresponding vehicle type: ");
            int chosenVehicleType = int.Parse(Console.ReadLine());

            Console.Clear();

            Console.Write("Type in the registration plate of the vehicle in question: ");
            if (chosenVehicleType == 1)
            {
                Car myCar = new Car(Console.ReadLine());
                string myCarData = myCar.vehicleType + "#" + myCar.regPlate;

                ParkingSpot mySpot = new ParkingSpot();
                mySpot.vehicleData = myCarData;

                Console.Clear();

                Console.Write("Type in the number of the parking spot that you'd like to park your car at: ");
                mySpot.parkToNumber = Console.ReadLine();

                myPark.addToParking(mySpot.vehicleData, mySpot.parkToNumber);

                Console.Write("\n\nPress random key to continue...");
                Console.ReadKey();
                return;
            }
            else if (chosenVehicleType == 2)
            {
                MC myMC = new MC(Console.ReadLine());
                string myMCData = myMC.vehicleType + "#" + myMC.regPlate;

                ParkingSpot mySpot = new ParkingSpot();
                mySpot.vehicleData = myMCData;

                Console.Clear();

                Console.Write("Type in the number of the parking spot that you'd like to park your car at: ");
                mySpot.parkToNumber = Console.ReadLine();

                myPark.addToParking(mySpot.vehicleData, mySpot.parkToNumber);

                Console.Write("\n\nPress random key to continue...");
                Console.ReadKey();
                return;
            }

        }

        void removeVehicle()
        {

        }

        void showParking()
        {
            Console.Clear();

            //Problem: Det skappas flera listor när man använder metoden flera gånger
            myPark.printParking();

            Console.Write("\n\nPress random key to continue...");
            Console.ReadKey();
            return;
        }

    }
}