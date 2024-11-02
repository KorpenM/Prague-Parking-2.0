using System;
using System.Collections.Generic;
using PragueParking_2._0;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using System.Xml;
using PragueParking_2;

internal class Program
{
    private static Garage garage = new Garage();

    private static void Main()
    {
        garage.LoadSettings();

        string menuChoice;

        do
        {
            Console.Clear();

            AnsiConsole.Write(new FigletText("Prague Parking")
                       .LeftJustified()
                       .Color(Color.Blue));

            var rule = new Rule();
            AnsiConsole.Write(rule);

            garage.ShowColorParkingSpots();

            AnsiConsole.Write(rule);

            var options = new[]
            {
                "Park Vehicle",
                "Retrieve/Remove Vehicle",
                "Move Vehicle",
                "Search",
                "Show all registered vehicles",
                "Edit Parking Settings", // Json config
                "Show Current/Updated Settings",
                "Exit"
            };

            var highlightStyle = new Style().Foreground(Color.Blue);
            menuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .HighlightStyle(highlightStyle)
                    .WrapAround(true)
                    .Title("[bold blue]Choose an option:[/]")
                    .AddChoices(options));

            switch (menuChoice)
            {
                case "Park Vehicle":
                    AddVehicle();
                    break;
                case "Retrieve/Remove Vehicle":
                    RemoveVehicle();
                    break;
                case "Move Vehicle":
                    MoveVehicle();
                    break;
                case "Search":
                    SearchVehicle();
                    break;
                case "Show all registered vehicles":
                    ShowRegisteredVehicles();
                    break;
                case "Edit Parking Settings":
                    EditSettings();
                    break;
                case "Show Current/Updated Settings":
                    ShowSettings();
                    break;
                case "Exit":
                    Console.Write("\nExiting program...");
                    break;
                default:
                    Console.Write("\nInvalid choice. Please try again...");
                    break;
            }
        } while (menuChoice != "Exit"); // Program closes only at case "Exit"
    }

    private static void EditSettings()
    {
        Console.Clear();
        AnsiConsole.Markup("[bold yellow]Editing Parking Settings:[/]\n");

        garage.settings.TotalSpots = AnsiConsole.Ask<int>("Enter total parking spots: ");
        garage.settings.FreeParkingMinutes = AnsiConsole.Ask<int>("Enter free parking minutes: ");

        // Check and initiate VehicleTypes if it's null
        if (garage.settings.VehicleTypes == null)
        {
            garage.settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>();
        }

        foreach (var vehicleType in garage.settings.VehicleTypes)
        {
            AnsiConsole.Markup($"\n--- [bold cyan]{vehicleType.Key} Settings ---[/]");
            vehicleType.Value.SpaceRequired = AnsiConsole.Ask<int>("Enter space required: ");
            vehicleType.Value.RatePerHour = AnsiConsole.Ask<int>("Enter rate per hour: ");
            vehicleType.Value.AllowedSpots = AnsiConsole.Ask<string>("Enter allowed spots: ");
            vehicleType.Value.NumberOfVehiclesPerSpot = AnsiConsole.Ask<int>("Enter number of vehicles per spot: ");
        }

        garage.SaveSettings();
    }

    private static void ShowSettings()
    {
        Console.Clear();
        AnsiConsole.Markup("[bold yellow]Current Parking Settings:[/]\n");

        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        if (File.Exists(jsonFilePath))
        {
            // Read and show JSON file
            string json = File.ReadAllText(jsonFilePath);
            var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Newtonsoft.Json.Formatting.Indented);
            AnsiConsole.Markup(formattedJson);
        }
        else
        {
            AnsiConsole.Markup("[red]Config file not found. Default settings may be in use.[/]");
        }
        AnsiConsole.Markup("\n[green]Press Enter to return to the menu...[/]");
        Console.ReadLine();
    }

    private static void AddVehicle()
    {
        Console.Clear();
        var vehicleTypes = new[]
        {
            "Bike",
            "Motorcycle",
            "Car",
            "Bus"
        };
        var highlightStyle = new Style().Foreground(Color.Blue);
        var chosenVehicleType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(highlightStyle)
                .WrapAround(true)
                .Title("What type of vehicle are you trying to park?")
                .AddChoices(vehicleTypes)
        );

        Console.Clear();
        string regNumber = AnsiConsole.Ask<string>("Type in the registration plate of the vehicle in question: \n");
        Vehicle vehicle = null;

        switch (chosenVehicleType)
        {
            case "Bike":
                vehicle = new Bike(regNumber);
                break;
            case "Motorcycle":
                vehicle = new MC(regNumber);
                break;
            case "Car":
                vehicle = new Car(regNumber);
                break;
            case "Bus":
                vehicle = new Bus(regNumber);
                break;
            default:
                Console.WriteLine("Invalid vehicle type selected");
                Console.Write("\nPress random key to continue...");
                Console.ReadKey();
                return;
        }

        if (garage.ParkVehicle(vehicle))
        {
            //AnsiConsole.Markup($"{vehicle.TypeOfVehicle} with registration number [blue]{regNumber}[/] has been parked");
        }
        else
        {
            Console.WriteLine("There is no available parking spots");
        }

        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void RemoveVehicle()
    {
        Console.Clear();
        string regNumber = AnsiConsole.Ask<string>("Enter the registration plate of the vehicle you wish to remove: ");

        if (garage.RemoveVehicle(regNumber))
        {
            AnsiConsole.Markup($"Vehicle with registration number [blue]{regNumber}[/] has been removed");
        }
        else
        {
            Console.WriteLine("Vehicle not found");
        }
        Console.Write("Press random key to continue...");
        Console.ReadKey();
    }

    private static void MoveVehicle()
    {
        Console.Clear();
        string regNumber = AnsiConsole.Ask<string>("Enter the registration number of the vehicle you want to move: ");
        if (!int.TryParse(AnsiConsole.Ask<string>("Enter the parking spot to move from: "), out int fromSpot) || fromSpot < 0)
        {
            Console.WriteLine("Invalid spot number");
            Console.Write("Press random key to continue...");
            Console.ReadKey();
            return;
        }

        if (!int.TryParse(AnsiConsole.Ask<string>("Enter the parking spot to move to: "), out int toSpot) || toSpot < 0)
        {
            Console.WriteLine("Invalid spot number");
            Console.Write("Press random key to continue...");
            Console.ReadKey();
            return;
        }

        if (garage.MoveVehicle(regNumber, fromSpot, toSpot))
        {
            Console.WriteLine("Vehicle moved successfully");
        }
        else
        {
            Console.WriteLine("Failed to move vehicle. Check if it exists and if spots are valid");
        }

        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void SearchVehicle()
    {
        Console.Clear();
        string regNumber = AnsiConsole.Ask<string>("Enter the registration plate of the vehicle you wish to search for: ");

        var spot = garage.FindVehicle(regNumber);
        if (spot != null)
        {
            AnsiConsole.Markup($"Vehicle with registration number [blue]{regNumber}[/] is parked at spot {spot.ID + 1}");
        }
        else
        {
            Console.WriteLine("Vehicle not found");
        }

        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void ShowRegisteredVehicles()
    {
        Console.Clear();
        garage.PrintRegisteredVehicles();
        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }
}