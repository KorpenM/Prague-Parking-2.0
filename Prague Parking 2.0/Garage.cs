using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.Design;
using PragueParking_2._0;
using System.Runtime.CompilerServices;

namespace Prague_Parking_2._0
{
    interface IParkVehicle //Used in Vehicle to park itself in garage.
    {
        public string RegNumber { get; set; }
        //public void ParkVehicle(Vehicle vehicle);
    }
    public class Garage
    {
        public string RegNumber { get; set; }
        private int capacity = 100;
        List<ParkingSpot> garageList = new List<ParkingSpot>();
        public ParkingSettings settings; // Store settings from JSON

        public Garage() // Constructor for Garage
        {
            LoadSettings(); // Load settings from JSON


            if (garageList.Count > 0) return;

            //Add (capacity) parkingspots to garage
            for (int i = 0; i < capacity; i++)
            {
                garageList.Add(new ParkingSpot(i));
            }
        }

        public void LoadParkingData()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "parking_data.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                var parkingData = JsonConvert.DeserializeObject<ParkingData>(json);

                foreach (var spotData in parkingData.ParkingSpots)
                {
                    var parkingSpot = garageList[spotData.ID];
                    parkingSpot.Spots.Clear();

                    foreach (var vehicleData in spotData.Vehicles)
                    {
                        Vehicle vehicle = vehicleData.Type switch
                        {
                            "Bike" => new Bike(vehicleData.RegNumber),
                            "MC" => new MC(vehicleData.RegNumber),
                            "Car" => new Car(vehicleData.RegNumber),
                            "Bus" => new Bus(vehicleData.RegNumber),
                            _ => throw new InvalidOperationException("Unknown vehicle type.")
                        };

                        vehicle.ParkingStartime = vehicleData.ParkingStartTime;
                        parkingSpot.Spots.Add(vehicle);
                        parkingSpot.UpdateSpot(vehicle);
                    }
                }

                Console.WriteLine("Parking data loaded successfully.");
            }
            else
            {
                Console.WriteLine("Parking data file not found. No previous parking data loaded.");
            }
        }

        public void SaveParkingData()
        {
            var parkingData = new ParkingData();

            foreach (var spot in garageList)
            {
                var spotData = new ParkingSpotData
                {
                    ID = spot.ID,
                    Vehicles = spot.Spots.Select(v => new VehicleData
                    {
                        RegNumber = v.RegNumber,
                        Type = v.GetType().Name,
                        ParkingStartTime = v.ParkingStartime
                    }).ToList()
                };

                parkingData.ParkingSpots.Add(spotData);

                if (garageList.Count > 0) return;

                //Add (capacity) parkingspots to garage
                for (int i = 0; i < capacity; i++)
                {
                    garageList.Add(new ParkingSpot(i));
                }
            }
        }

        public void ShowParkingData()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "parking_data.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                var parkingData = JsonConvert.DeserializeObject<ParkingData>(json);

                Console.WriteLine("Current parking data:");

                foreach (var spot in parkingData.ParkingSpots)
                {
                    Console.WriteLine($"Spot ID: {spot.ID + 1}");
                    if (spot.Vehicles != null && spot.Vehicles.Count > 0)
                    {
                        foreach (var vehicle in spot.Vehicles)
                        {
                            Console.WriteLine($"  Vehicle: {vehicle.Type}, Reg Number: {vehicle.RegNumber}, Parking Start Time: {vehicle.ParkingStartTime}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No vehicles parked.");
                    }
                    Console.WriteLine(); // Blank line for better readability
                }
            }
            else
            {
                Console.WriteLine("Parking data file not found.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void LoadSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                settings = JsonConvert.DeserializeObject<ParkingSettings>(json);
                Console.WriteLine("Settings loaded successfully.");
            }
            else
            {
                Console.WriteLine("Config file not found. Using default settings.");

                settings = new ParkingSettings
                {
                    TotalSpots = 100,
                    FreeParkingMinutes = 10,
                    VehicleTypes = new Dictionary<string, VehicleTypeInfo>
            {
                { "Bike", new VehicleTypeInfo { SpaceRequired = 1, RatePerHour = 5, AllowedSpots = "All spots" }},
                { "MC", new VehicleTypeInfo { SpaceRequired = 2, RatePerHour = 10, AllowedSpots = "All spots" }},
                { "Car", new VehicleTypeInfo { SpaceRequired = 4, RatePerHour = 20, AllowedSpots = "All spots" }},
                { "Bus", new VehicleTypeInfo { SpaceRequired = 16, RatePerHour = 80, AllowedSpots = "Spots 1-50 only" }},
            }
                };
            }

            // Kontrollera om VehicleTypes är null och initiera vid behov
            if (settings.VehicleTypes == null)
            {
                settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>();
            }
        }

        public void SaveSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented); // Indenterad format
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine("Settings saved successfully.");
        }

        public void AddNewVehicleType()
        {
            Console.Clear();
            AnsiConsole.Markup("[bold yellow]Add New Vehicle Type:[/]\n");

            string newVehicleType = AnsiConsole.Ask<string>("Enter the name of the new vehicle type: ");

            // Kontrollera om fordonstypen redan finns
            if (settings.VehicleTypes.ContainsKey(newVehicleType))
            {
                AnsiConsole.Markup("[red]This vehicle type already exists. Please choose a different name.[/]");
                Console.Write("\nPress random key to continue...");
                Console.ReadKey();
                return;
            }

            var newVehicleInfo = new VehicleTypeInfo
            {
                SpaceRequired = AnsiConsole.Ask<int>("Enter space required: "),
                RatePerHour = AnsiConsole.Ask<int>("Enter rate per hour: "),
                AllowedSpots = AnsiConsole.Ask<string>("Enter allowed spots: "),
                NumberOfVehiclesPerSpot = AnsiConsole.Ask<int>("Enter number of vehicles per spot: ")
            };

            // Lägga till den nya fordonstypen i VehicleTypes
            settings.VehicleTypes[newVehicleType] = newVehicleInfo;
            Console.WriteLine($"Added new vehicle type: {newVehicleType}");

            SaveSettings(); // Spara de nya inställningarna
            Console.Write("\nPress random key to continue...");
            Console.ReadKey();
        }

        /*public void ParkVehicle(Vehicle vehicle, bool selectSpace, int space, bool parkBus)
        {
            if (!selectSpace && space == 0)
            {
                for (int i = 0; i < garageList.Count; i++)
                {
                    ParkingSpot parkingSpot = garageList[i];

                    if (vehicle.Space <= parkingSpot.Available)
                    {
                        garageList[i].Spots.Add(vehicle);
                        garageList[i].UpdateSpot(vehicle);
                        Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {i + 1}");
                        Console.WriteLine($"There are now {parkingSpot.Available} spaces on this spot available.");
                        break;
                    }
                }
            }
            else
            {
                ParkingSpot parkingSpot = garageList[space];

                if (vehicle.Space <= parkingSpot.Available)
                {
                    garageList[space].Spots.Add(vehicle);
                    garageList[space].UpdateSpot(vehicle);
                    Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {space + 1}");
                    Console.WriteLine($"There are now {parkingSpot.Available} spaces on this spot available.");
                }

                for (int j = 0; j < garageList[space].Spots.Count; j++)
                {
                    Console.WriteLine($"Spot {j} available");
                }
            }
        }*/

        public void ParkVehicle(Vehicle vehicle, bool selectSpace, int space, bool parkingBus)
        {
            if (!selectSpace && space == 0)
            {
                for (int i = 0; i < garageList.Count; i++)
                {
                    ParkingSpot parkingSpot = garageList[i];
                    ParkingSpot secondParkingSpot = garageList[i + 1];
                    ParkingSpot thirdParkingSpot = garageList[i + 2];
                    ParkingSpot fourthParkingSpot = garageList[i + 3];

                    if (vehicle.Space <= parkingSpot.Available && parkingBus != true)
                    {
                        garageList[i].Spots.Add(vehicle);
                        garageList[i].UpdateSpot(vehicle);
                        Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {i + 1}");
                        Console.WriteLine($"There are now {parkingSpot.Available} spaces on this spot available.");
                        return;
                    }
                    else if (parkingBus == true)
                    {
                        if (parkingSpot.Available == 4 && secondParkingSpot.Available == 4
                           && thirdParkingSpot.Available == 4 && fourthParkingSpot.Available == 4)
                        {
                            garageList[i].Spots.Add(vehicle);
                            garageList[i].UpdateSpot(vehicle);
                            garageList[i + 1].Spots.Add(vehicle);
                            garageList[i + 1].UpdateSpot(vehicle);
                            garageList[i + 2].Spots.Add(vehicle);
                            garageList[i + 2].UpdateSpot(vehicle);
                            garageList[i + 3].Spots.Add(vehicle);
                            garageList[i + 3].UpdateSpot(vehicle);
                            Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at spot {i + 1}, {i + 2}, {i + 3}, and {i + 4}");
                            return;
                        }
                        else if (i > 46)
                        {
                            Console.WriteLine("There's no available parking spots left to park this bus at");
                            Console.Write("\n\nPress any random key to continue...");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                ParkingSpot parkingSpot = garageList[space];
                ParkingSpot secondParkingSpot = garageList[space + 1];
                ParkingSpot thirdParkingSpot = garageList[space + 2];
                ParkingSpot fourthParkingSpot = garageList[space + 3];

                if (vehicle.Space <= parkingSpot.Available && parkingBus != true)
                {
                    garageList[space].Spots.Add(vehicle);
                    garageList[space].UpdateSpot(vehicle);
                    Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {space + 1}");
                    Console.WriteLine($"There are now {parkingSpot.Available} spaces on this spot available.");
                    return;
                }
                else if (parkingBus == true)
                {
                    if (parkingSpot.Available == 4 && secondParkingSpot.Available == 4
                       && thirdParkingSpot.Available == 4 && fourthParkingSpot.Available == 4)
                    {
                        garageList[space].Spots.Add(vehicle);
                        garageList[space].UpdateSpot(vehicle);
                        garageList[space + 1].Spots.Add(vehicle);
                        garageList[space + 1].UpdateSpot(vehicle);
                        garageList[space + 2].Spots.Add(vehicle);
                        garageList[space + 2].UpdateSpot(vehicle);
                        garageList[space + 3].Spots.Add(vehicle);
                        garageList[space + 3].UpdateSpot(vehicle);
                        Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at spot {space + 1}, {space + 2}, {space + 3}, and {space + 4}");
                        return;
                    }
                    else if (space > 46)
                    {
                        Console.WriteLine("There's no available parking spots left to park this bus at");
                        return;
                    }
                }
            }
        }

        public bool RemoveVehicle(string regNumber, bool removeBus)
        {
            if (removeBus != true)
                foreach (ParkingSpot spot in garageList)
                {
                    foreach (Vehicle vehicle in spot.Spots)
                    {

                        if (removeBus != true)
                        {
                            if (vehicle.RegNumber == regNumber && vehicle.EndParking)
                            {
                                AnsiConsole.Markup($"You have parked for [blue]{vehicle.CalculateParkingTime}[/] ");
                                AnsiConsole.Markup($"The total cost is [blue]{vehicle.CalculateParkingCost}[/] ");
                                spot.Spots.Remove(vehicle);
                                spot.ResetSpot();
                                return true;
                            }
                            else
                            {
                                spot.Spots.Remove(vehicle);
                                spot.ResetSpot();
                                return true;
                            }
                        }

                        else if (removeBus == true)
                        {
                            for (int i = 0; i < garageList.Count; i++)
                            {
                                ParkingSpot parkingSpot = garageList[i];
                                ParkingSpot secondParkingSpot = garageList[i];
                                ParkingSpot thirdParkingSpot = garageList[i];
                                ParkingSpot fourthParkingSpot = garageList[i];

                                if (vehicle.RegNumber == regNumber)
                                {
                                    parkingSpot.Spots.Remove(vehicle);
                                    parkingSpot.ResetSpot();
                                    secondParkingSpot.Spots.Remove(vehicle);
                                    secondParkingSpot.ResetSpot();
                                    thirdParkingSpot.Spots.Remove(vehicle);
                                    thirdParkingSpot.ResetSpot();
                                    fourthParkingSpot.Spots.Remove(vehicle);
                                    fourthParkingSpot.ResetSpot();
                                    return true;
                                }
                            }
                        }
                    }
                }
            return false;
        }

        public Vehicle? FindVehicle(string regNumber)
        {
            foreach (var spot in garageList)
            {
                foreach (var vehicle in spot.Spots)
                {
                    if (vehicle.RegNumber == regNumber)
                    {
                        int spotNumber = spot.ID;
                        return vehicle;
                    }
                    else
                    {
                        //changed from return null to continue to make move bus work
                        continue;
                    }
                }
                //changed from return null to continue to make move bus work
                continue;
            }
            return null;
        }

        internal ParkingSpot? FindSpot(Vehicle vehicle)
        {
            foreach (var spot in garageList)
            {
                if (spot.Spots.Contains(vehicle))
                {
                    return spot;
                }
                else
                {
                    return null;
                }

            }
            return null;
        }

        public bool MoveVehicle(string regNumber, bool selectSpace, int space, bool moveBus)
        {
            Vehicle? vehicle = FindVehicle(regNumber);
            if (vehicle != null && moveBus != true)
            {
                RemoveVehicle(regNumber, false);
                ParkVehicle(vehicle, true, space - 1, false);
                return true;
            }
            else if (vehicle != null && moveBus == true)
            {
                RemoveVehicle(regNumber, true);
                ParkVehicle(vehicle, true, space - 1, true);
                return true;
            }
            return true;
        }

        public void ShowColorParkingSpots()
        {
            int spotsPerRow = 10; // Number of parking spots per row
            Console.WriteLine("Parking grid - Compact view (Green = Free, Yellow = 1/2 full, Red = Busy)\n");

            for (int i = 0; i < garageList.Count; i++)
            {
                if (i % spotsPerRow == 0 && i != 0)
                {
                    Console.WriteLine();
                }

                ParkingSpot spot = garageList[i];

                if (spot.Available == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Free
                }
                else if (spot.Available > 0 && spot.Available < 4)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (spot.Available <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write($"[{spot.ID + 1:D3}] "); // Format spot ID
                Console.ResetColor();
            }
            Console.WriteLine("\n");
        }

        public void PrintRegisteredVehicles()
        {
            Console.Clear();
            Console.WriteLine("=== Registered Vehicles ===\n");

            bool hasVehicles = false;

            foreach (var spot in garageList)
            {
                if (spot.Available < 4)
                {
                    foreach (Vehicle vehicle in spot.Spots)
                    {
                        Console.WriteLine($"Spot {spot.ID + 1}: {vehicle.GetType().Name} - Reg: {vehicle.RegNumber}");
                        // Console.WriteLine(vehicle.ToString());

                        hasVehicles = true;
                    }
                }
            }

            if (!hasVehicles)
            {
                Console.WriteLine("No registered vehicles found.");
            }
            Console.ResetColor();
        }
    }
}