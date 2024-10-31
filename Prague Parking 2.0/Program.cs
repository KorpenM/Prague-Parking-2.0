using System;
using System.Collections.Generic;
using Spectre.Console;
using PragueParking_2._0;

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
                "Search Vehicle",
                //"Show Parking Spots",
                "Show all registered vehicles",
                "Optimize parking",
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
                case "Search Vehicle":
                    SearchVehicle();
                    break;
                //case "Show Parking Spots":
                //    ShowParking();
                //    break;
                case "Show all registered vehicles":
                    ShowRegisteredVehicles();
                    break;
                case "Optimize parking":
                    OptimizeParking();
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
            AnsiConsole.Markup($"{vehicle.TypeOfVehicle} with registration number [blue]{regNumber}[/] has been parked");
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

    //private static void ShowParking()
    //{
    //    Console.Clear();
    //    AnsiConsole.Markup("[bold yellow]Showing parking spots...[/]\n");
    //    garage.PrintGarage();
    //    Console.Write("\nPress random key to continue...");
    //    Console.ReadKey();
    //    Console.Clear();
    //}

    private static void ShowRegisteredVehicles()
    {
        Console.Clear();
        garage.PrintRegisteredVehicles();
        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }

    private static void OptimizeParking()
    {
        Console.Clear();
        garage.OptimizeParking();
        Console.Write("\nPress random key to continue...");
        Console.ReadKey();
    }
}