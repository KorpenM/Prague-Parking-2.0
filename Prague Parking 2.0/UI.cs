using Newtonsoft.Json;
using Prague_Parking_2._0;
using PragueParking_2._0;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._0
{
    internal class UI
    {
        string menuChoice;

        public UI(Garage garage)
        {
            garage.LoadParkingData();

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
                "Retrieve Vehicle",
                "Move Vehicle",
                "Search Vehicle",
                "Show All Registered Vehicles",
                "Edit Parking Settings",
                "Show Current Settings",
                "Show Parking Data",
                "Add New Vehicle Type",
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
                        AddVehicle(garage);
                        break;
                    case "Retrieve Vehicle":
                        RemoveVehicle(garage);
                        break;
                    case "Move Vehicle":
                        MoveVehicle(garage);
                        break;
                    case "Search Vehicle":
                        SearchVehicle(garage);
                        break;
                    case "Show All Registered Vehicles":
                        ShowRegisteredVehicles(garage);
                        break;
                    case "Edit Parking Settings":
                        EditSettings(garage);
                        garage.SaveParkingData();
                        break;
                    case "Show Current Settings":
                        ShowSettings(garage);
                        garage.SaveParkingData();
                        break;
                    case "Show Parking Data":
                        garage.ShowParkingData();
                        break;
                    case "Add New Vehicle Type":
                        garage.AddNewVehicleType();
                        garage.SaveParkingData();
                        break;
                    case "Exit":
                        Console.Write("\nExiting program...");
                        break;
                    default:
                        Console.Write("\nInvalid choice. Please try again...");
                        break;
                }
            } while (menuChoice != "Exit"); // Program closes only at case "Exit".
        }

        private static void EditSettings(Garage garage)
        {
            {
                Console.Clear();
                AnsiConsole.Markup("[bold yellow]Editing Parking Settings:[/]\n");

                garage.settings.TotalSpots = AnsiConsole.Ask<int>("Enter total parking spots: ");
                garage.settings.FreeParkingMinutes = AnsiConsole.Ask<int>("Enter free parking minutes: ");

                // Kontrollera om det redan finns fordonstyper och initiera dem om de inte finns
                if (garage.settings.VehicleTypes == null || !garage.settings.VehicleTypes.Any())
                {
                    garage.settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>
        {
            { "Bike", new VehicleTypeInfo() },
            { "Motorcycle", new VehicleTypeInfo() },
            { "Car", new VehicleTypeInfo() },
            { "Bus", new VehicleTypeInfo() }
        };
                }

                // Ge möjlighet att lägga till en ny fordonstyp
                if (AnsiConsole.Confirm("Would you like to add a new vehicle type?"))
                {
                    string newVehicleType = AnsiConsole.Ask<string>("Enter the new vehicle type name: ");
                    if (!garage.settings.VehicleTypes.ContainsKey(newVehicleType))
                    {
                        garage.settings.VehicleTypes[newVehicleType] = new VehicleTypeInfo();
                        garage.settings.VehicleTypes[newVehicleType].SpaceRequired = AnsiConsole.Ask<int>("Enter space required: ");
                        garage.settings.VehicleTypes[newVehicleType].RatePerHour = AnsiConsole.Ask<int>("Enter rate per hour: ");
                        garage.settings.VehicleTypes[newVehicleType].AllowedSpots = AnsiConsole.Ask<string>("Enter allowed spots: ");
                        garage.settings.VehicleTypes[newVehicleType].NumberOfVehiclesPerSpot = AnsiConsole.Ask<int>("Enter number of vehicles per spot: ");
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]This vehicle type already exists.[/]");
                    }
                }

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
        }

        private static void ShowSettings(Garage garage)
        {
            Console.Clear();
            AnsiConsole.Markup("[bold yellow]Current Parking Settings:[/]\n");

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            if (File.Exists(jsonFilePath))
            {
                // Read and show JSON file
                string json = File.ReadAllText(jsonFilePath);
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Newtonsoft.Json.Formatting.Indented);

                AnsiConsole.Markup($"[bold cyan]{formattedJson}[/]");

                AnsiConsole.Markup(formattedJson);

            }
            else
            {
                AnsiConsole.Markup("[red]Config file not found. Default settings may be in use.[/]");
            }
            AnsiConsole.Markup("\n[green]Press Enter to return to the menu...[/]");
            Console.ReadLine();
        }

        private static void AddVehicle(Garage garage)
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
            Vehicle? vehicle = null;
            bool parkingBus = false;

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
                    parkingBus = true;

                    break;
                default:
                    Console.WriteLine("Invalid vehicle type selected");
                    Console.Write("\nPress random key to continue...");
                    Console.ReadKey();
                    return;
            }

            if (regNumber != null)
            {
                //AnsiConsole.Markup($"{vehicle.TypeOfVehicle} with registration number [blue]{regNumber}[/] has been parked");
                garage.ParkVehicle(vehicle, false, 0, parkingBus);
                Console.WriteLine("UI Parked");
            }
            else
            {
                Console.WriteLine("There is no available parking spots");
            }
            Console.Write("\nPress random key to continue...");
            Console.ReadKey();
        }

        private static void RemoveVehicle(Garage garage)
        {
            Console.Clear();
            string regNumber = AnsiConsole.Ask<string>("Enter the registration plate of the vehicle you wish to remove: ");

            Console.Clear();
            // Ask the user to confirm
            bool removeBus = AnsiConsole.Prompt(
                new TextPrompt<bool>("Is the vehicle you're reclaiming a bus?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "y" : "n"));
            // Echo the confirmation back to the terminal
            Console.WriteLine(removeBus ? "Confirmed" : "Declined");

            if (regNumber != null)
            {
                Vehicle? vehicle = garage.FindVehicle(regNumber);
                Console.WriteLine($"Total cost for parking: {vehicle?.CalculateParkingCost(DateTime.Now)}");
                Console.WriteLine($"TIme parked: {vehicle? .CalculateParkingTime(DateTime.Now)}");
                garage.RemoveVehicle(regNumber, removeBus);
                //AnsiConsole.Markup($"Vehicle with registration number [blue]{regNumber}[/] has been removed");
            }
            else
            {
                Console.WriteLine("Vehicle not found");
            }
            
            Console.Write("Press random key to continue...");
            Console.ReadKey();
        }

        private static void MoveVehicle(Garage garage)
        {
            Console.Clear();
            string regNumber = AnsiConsole.Ask<string>("Enter the registration number of the vehicle you want to move: ");

            bool moveBus = AnsiConsole.Prompt(
                new TextPrompt<bool>("Is the vehicle you're reclaiming a bus?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "y" : "n"));
            Console.WriteLine(moveBus ? "Confirmed" : "Declined");

            if (!int.TryParse(AnsiConsole.Ask<string>("Enter the parking spot to move to: "), out int toSpot) || toSpot < 0 || toSpot > garage.garageList.Count)
            {
                Console.WriteLine("Invalid spot number");
                Console.Write("Press random key to continue...");
                Console.ReadKey();
                return;
            }

            if (garage.MoveVehicle(regNumber, true, toSpot - 1, moveBus))
            {
                //Console.WriteLine("Vehicle moved successfully");
            }
            else
            {
                Console.WriteLine("Failed to move vehicle. Check if it exists and if spots are valid");
            }

            Console.Write("\nPress random key to continue...");
            Console.ReadKey();
        }

        //private static void MoveVehicle(Garage garage)
        //{
        //    Console.Clear();
        //    string regNumber = AnsiConsole.Ask<string>("Enter the registration number of the vehicle you want to move: ");
        //    if (!int.TryParse(AnsiConsole.Ask<string>("Enter the parking spot to move from: "), out int fromSpot) || fromSpot < 0)
        //    {
        //        Console.WriteLine("Invalid spot number");
        //        Console.Write("Press random key to continue...");
        //        Console.ReadKey();
        //        return;
        //    }

        //    if (!int.TryParse(AnsiConsole.Ask<string>("Enter the parking spot to move to: "), out int toSpot) || toSpot < 0)
        //    {
        //        Console.WriteLine("Invalid spot number");
        //        Console.Write("Press random key to continue...");
        //        Console.ReadKey();
        //        return;
        //    }

        //    if (garage.MoveVehicle(regNumber, fromSpot, toSpot))
        //    {
        //        Console.WriteLine("Vehicle moved successfully");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Failed to move vehicle. Check if it exists and if spots are valid");
        //    }

        //    Console.Write("\nPress random key to continue...");
        //    Console.ReadKey();
        //}

        private static void SearchVehicle(Garage garage)
        {
            Console.Clear();
            string regNumber = AnsiConsole.Ask<string>("Enter the registration plate of the vehicle you wish to search for: ");
            Vehicle? vehicle = garage.FindVehicle(regNumber);
            ParkingSpot? spot = garage.FindSpot(vehicle);
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

        private static void ShowRegisteredVehicles(Garage garage)
        {
            Console.Clear();
            garage.PrintRegisteredVehicles();
            Console.Write("\nPress random key to continue...");
            Console.ReadKey();
        }
    }
}

