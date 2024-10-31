using System;
using System.Collections.Generic;
using PragueParking_2._0;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static Garage garage = new Garage();

    private static void Main()
    {
        garage.LoadSettings();

        garage.InitGarage();

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
            Console.WriteLine("9. Edit Parking Settings"); // Json config
            Console.WriteLine("10. Show Current/Updated Settings");
            Console.WriteLine("11. Exit");

            menuChoice = Console.ReadLine();

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
                    EditSettings();
                    break;
                case "10":
                    ShowSettings();
                    break;
                case "11":
                    Console.WriteLine("Exiting the program...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadKey();

            if (menuChoice != "11")
            {
                Console.WriteLine("\nPress Enter to return to the menu...");
                Console.ReadLine();
            }

        } while (menuChoice != "11"); // Program closes only at case 11, to easier view JSON
    }
    private static void EditSettings()
    {
        Console.Clear();
        Console.WriteLine("Editing Parking Settings:");
        Console.Write("Enter total parking spots: ");
        garage.settings.TotalSpots = int.Parse(Console.ReadLine());

        Console.Write("Enter free parking minutes: ");
        garage.settings.FreeParkingMinutes = int.Parse(Console.ReadLine());

        // Check and initiate VehicleTypes if it's null
        if (garage.settings.VehicleTypes == null)
        {
            garage.settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>();
        }

        foreach (var vehicleType in garage.settings.VehicleTypes)
        {
            Console.WriteLine($"\n--- {vehicleType.Key} Settings ---");
            Console.Write("Enter space required: ");
            vehicleType.Value.SpaceRequired = int.Parse(Console.ReadLine());

            Console.Write("Enter rate per hour: ");
            vehicleType.Value.RatePerHour = int.Parse(Console.ReadLine());

            Console.Write("Enter allowed spots: ");
            vehicleType.Value.AllowedSpots = Console.ReadLine();
        }

        garage.SaveSettings();
    }

    private static void ShowSettings()
    {
        Console.Clear();
        Console.WriteLine("Current Parking Settings:");

        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        if (File.Exists(jsonFilePath))
        {
            // Read and show JSON file
            string json = File.ReadAllText(jsonFilePath);
            var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented);

            Console.WriteLine(formattedJson);
        }
        else
        {
            Console.WriteLine("Config file not found. Default settings may be in use.");
        }

        Console.WriteLine("\nPress Enter to return to the menu...");
        Console.ReadLine();
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
    }

    private static void ShowColorParking()
    {
        Console.Clear();
        garage.ShowColorParkingSpots();
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