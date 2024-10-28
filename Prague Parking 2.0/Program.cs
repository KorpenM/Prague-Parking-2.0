using PragueParking_2._0;

internal class Program
{
    private static void Main()
    {
        //Initialize garage with parking spaces
        //Garage.InitGarage();

        ////Create car and park
        //Car one = new Car("ABC123");
        //one.ParkCar();

        ////Create mc and park
        //MC mc = new MC("QWE123");
        //mc.ParkMC();


        ////Test print to console
        //Console.WriteLine(one.Space);   
        //Console.WriteLine(one.TypeOfVehicle);
        //Console.WriteLine(one.Rate);
        //Console.WriteLine(one.RegNumber);

        //Meny
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
                Car automobile = new Car(Console.ReadLine());
                automobile.ParkCar();
                
                Console.Write("\n\nPress random key to continue...");
                Console.ReadKey();
                return;
            }            
            else if (chosenVehicleType == 2)
            {
                MC motorcycle = new MC(Console.ReadLine());
                motorcycle.ParkMC();
                
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

            Console.Write("\n\nPress random key to continue...");
            Console.ReadKey();
            return;
        }

    }
}