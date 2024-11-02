using System;
using System.Collections.Generic;
using Prague_Parking_2._0;

internal class Program
{
    //Create new Garage and initialize
    private static Garage garage = new Garage();

    private static void Main()
    {
        string menuChoice;
        do
        {
            Console.Clear();
            Console.WriteLine("======================");
            Console.WriteLine("   PRAGUE PARKING");
            Console.WriteLine("======================\n");

            Console.WriteLine("Choose option:");
            Console.WriteLine("1. Park Vehicle");
            Console.WriteLine("2. Retrieve/Remove Vehicle");
            Console.WriteLine("3. Move Vehicle");
            Console.WriteLine("4. Search");
            Console.WriteLine("5. Show Parking Spots");
            Console.WriteLine("6. Show Parking Spots | COLOURED-GRID |");
            Console.WriteLine("7. Show all registered vehicles");
            Console.WriteLine("8. Optimize parking");
            Console.WriteLine("9. Exit");

            menuChoice = Console.ReadLine();
            // string choice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    AddVehicle();
                    break;
                case "2":
                    RemoveVehicle();
                    break;
                case "3":
                    MoveVehicle();
                    break;
                case "4":
                    SearchVehicle();
                    break;
                case "5":
                    ShowParking();
                    break;
                case "6":
                    ShowColorParking();
                    break;
                case "7":
                    ShowRegisteredVehicles();
                    break;
                case "8":
                    OptimizeParking();
                    break;
                case "9":
                    Console.WriteLine("Exiting the program...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadKey();
        } while (menuChoice != "9");
        // while (choice != "9");
    }

    private static void AddVehicle()
    {
        Console.Clear();
        Console.Write("What type of vehicle are you trying to park? \n\n[1] Bike \n[2] Motorcycle \n[3] Car \n[4] Bus \n\nType in the number of the corresponding vehicle type: ");
        int chosenVehicleType = int.Parse(Console.ReadLine());

        Console.Clear();
        Console.Write("Type in the registration plate of the vehicle in question: ");
        string regNumber = Console.ReadLine();
        Vehicle vehicle = null;

        if (chosenVehicleType == 1)
        {
            vehicle = new Bike(regNumber);
        }
        else if (chosenVehicleType == 2)
        {
            vehicle = new MC(regNumber);
        }
        else if (chosenVehicleType == 3)
        {
            vehicle = new Car(regNumber);
        }
        else if (chosenVehicleType == 4)
        {
            vehicle = new Bus(regNumber);
        }
        else
        {
            Console.WriteLine("Invalid vehicle type selected.");
            Console.ReadKey();
            return;
        }

        if (garage.ParkVehicle(vehicle))
        {
            // Skriver redan ut i ParkVehicle metoden - annars:
            // Console.WriteLine($"{vehicle.TypeOfVehicle} with registration number {regNumber} has been parked on spot {spot.ID + 1}.");
        }
        else
        {
            Console.WriteLine("No available parking spots.");
        }

        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void RemoveVehicle()
    {
        Console.Clear();
        Console.Write("Enter the registration plate of the vehicle you wish to remove: ");
        string regNumber = Console.ReadLine();

        if (garage.RemoveVehicle(regNumber))
        {
            Console.WriteLine($"Vehicle with registration number {regNumber} has been removed.");
        }
        else
        {
            Console.WriteLine("Vehicle not found.");
        }

        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void MoveVehicle()
    {
        Console.Clear();
        Console.WriteLine("Enter the registration number of the vehicle you want to move:");
        string regNumber = Console.ReadLine();

        Console.WriteLine("Enter the parking spot to move from:");
        if (!int.TryParse(Console.ReadLine(), out int fromSpot) || fromSpot < 0)
        {
            Console.WriteLine("Invalid spot number.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Enter the parking spot to move to:");
        if (!int.TryParse(Console.ReadLine(), out int toSpot) || toSpot < 0)
        {
            Console.WriteLine("Invalid spot number.");
            Console.ReadKey();
            return;
        }

        if (garage.MoveVehicle(regNumber, fromSpot, toSpot))
        {
            Console.WriteLine("Vehicle moved successfully.");
        }
        else
        {
            Console.WriteLine("Failed to move vehicle. Check if it exists and if spots are valid.");
        }

        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void SearchVehicle()
    {
        Console.Clear();
        Console.Write("Enter the registration plate of the vehicle you wish to search for: ");
        string regNumber = Console.ReadLine();

        var spot = garage.FindVehicle(regNumber);
        if (spot != null)
        {
            Console.WriteLine($"Vehicle with registration number {regNumber} is parked at spot {spot.ID + 1}.");
        }
        else
        {
            Console.WriteLine("Vehicle not found.");
        }

        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void ShowParking()
    {
        Console.Clear();
        Console.WriteLine("Showing parkingspots..");
        garage.PrintGarage();
        Console.WriteLine("Parkingspots shown.");
        //Console.Write("\n\nPress random key to continue...");
        //Console.ReadKey();
    }

    private static void ShowColorParking()
    {
        Console.Clear();
        garage.ShowColorParkingSpots();
        // garage.PrintColorGarage();
        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void ShowRegisteredVehicles()
    {
        Console.Clear();
        garage.PrintRegisteredVehicles();
        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void OptimizeParking()
    {
        Console.Clear();
        garage.OptimizeParking();
        Console.Write("\n\nPress random key to continue...");
        Console.ReadKey();
    }
}