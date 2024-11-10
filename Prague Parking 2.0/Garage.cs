using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using PragueParking_2._0;
using System.Text.Json;

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
        public List<ParkingSpot> garageList = new List<ParkingSpot>();
        public ParkingSettings settings; // Store settings from JSON

        List<ParkedVehicle> ParkedVehicles { get; set; } = new List<ParkedVehicle>();


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
            }
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "parking_data.json");
            string json = JsonConvert.SerializeObject(parkingData, Newtonsoft.Json.Formatting.Indented);
            // File.WriteAllText("parking_data.json", json);
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine("Data saved.");
        }


        public void AddAndSaveParkedVehicle(Vehicle vehicle, int parkingSpotId)
        {
            var parkingSpot = garageList.FirstOrDefault(spot => spot.ID == parkingSpotId - 1);
            if (parkingSpot != null)
            {
                if (!parkingSpot.Spots.Any(v => v.RegNumber == vehicle.RegNumber))
                {
                    parkingSpot.Spots.Add(vehicle);
                    parkingSpot.UpdateSpot(vehicle); 

                    var parkingData = new ParkingData
                    {
                        ParkingSpots = garageList.Select(spot => new ParkingSpotData
                        {
                            ID = spot.ID,
                            Vehicles = spot.Spots.Select(v => new VehicleData
                            {
                                RegNumber = v.RegNumber,
                                Type = v.GetType().Name,
                                ParkingStartTime = v.ParkingStartime
                            }).ToList()
                        }).ToList()
                    };
                    var json = JsonConvert.SerializeObject(parkingData, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("parking_data.json", json);

                    Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at spot {parkingSpotId} and saved to parking_data.json.");
                }
                else
                {
                    // Console.WriteLine($"Vehicle {vehicle.RegNumber} is already parked at spot {parkingSpotId}.");
                }
            }
            else
            {
                Console.WriteLine("Could not find the specified parking spot.");
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


        public void LoadSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                settings = JsonConvert.DeserializeObject<ParkingSettings>(json);
                Console.WriteLine("Settings loaded successfully.");

                // Kontrollera om VehicleTypes är null och initiera vid behov
                if (settings.VehicleTypes == null)
                {
                    settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>();
                }
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
        }

        public void SaveSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented); // Indenterad format
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine("Settings saved successfully.");
        }


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
                        AddAndSaveParkedVehicle(vehicle, i + 1);
                        Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {i + 1}");
                        Console.WriteLine($"There are now {parkingSpot.Available} spaces available on the spot.");
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
                            AddAndSaveParkedVehicle(vehicle, i + 1);
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
            else if (selectSpace == true && space != 0)
            {
                space -= 1;

                for (int i = 0; i < garageList.Count; i++)
                {
                    ParkingSpot parkingSpot = garageList[i];
                    ParkingSpot secondParkingSpot = garageList[i + 1];
                    ParkingSpot thirdParkingSpot = garageList[i + 2];
                    ParkingSpot fourthParkingSpot = garageList[i + 3];

                    if (parkingBus != true)
                    {
                        if (i == space && vehicle.Space <= parkingSpot.Available)
                        {
                            garageList[space].Spots.Add(vehicle);
                            garageList[space].UpdateSpot(vehicle);
                            Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at {space + 1}");
                            Console.WriteLine($"There are now {parkingSpot.Available} spaces on this spot available.");
                            return;
                        }
                        else if (i == space && vehicle.Space > parkingSpot.Available)
                        {
                            Console.WriteLine("There isn't enough room to park the vehicle at this spot.");
                            return;
                        }
                    }
                    else if (parkingBus == true)
                    {
                        if (i == space && parkingSpot.Available == 4 && secondParkingSpot.Available == 4
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
                            Console.WriteLine($"Vehicle {vehicle.GetType().Name} parked at spot {i + 1}, {i + 2}, {i + 3}, and {i + 4}");
                            return;
                        }
                        else if (i == space && parkingSpot.Available > 4 && secondParkingSpot.Available > 4
                           && thirdParkingSpot.Available > 4 && fourthParkingSpot.Available > 4)
                        {
                            Console.WriteLine("There isn't enough room to park the bus at this spot.");
                            return;
                        }
                    }
                }
            }
        }


        public bool RemoveVehicle(string regNumber, bool isBus, int moveFrom, bool moveVehicle)
        {
            moveFrom -= 1;

            for (int i = 0; i < garageList.Count; i++)
            {
                ParkingSpot spot = garageList[i];

                foreach (Vehicle vehicle in spot.Spots.ToList())
                {
                    // Vanligt fordon
                    if (!isBus && vehicle.RegNumber == regNumber)
                    {
                        if (moveVehicle != true)
                        {
                            spot.Spots.Remove(vehicle);
                            spot.UsedCapacity -= vehicle.Space;
                            spot.Available += vehicle.Space;

                            AnsiConsole.Markup($"You have parked for [blue]{vehicle.CalculateParkingTime(DateTime.Now)}[/] ");
                            AnsiConsole.Markup($"The total cost is [blue]{vehicle.CalculateParkingCost(DateTime.Now)}[/] ");

                            Console.WriteLine($"Vehicle {vehicle.RegNumber} removed from spot {spot.ID + 1}");
                            return true;
                        }
                        else if (moveVehicle == true && i != moveFrom)
                        {
                            spot.Spots.Remove(vehicle);
                            spot.UsedCapacity -= vehicle.Space;
                            spot.Available += vehicle.Space;

                            Console.WriteLine($"Vehicle {vehicle.RegNumber} removed from spot {spot.ID + 1}");
                            return true;
                        }
                    }

                    // Buss
                    else if (isBus && vehicle.RegNumber == regNumber)
                    {
                        if (moveVehicle != true)
                        {
                            if (vehicle.RegNumber == regNumber)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    garageList[i + j].Spots.Remove(vehicle);
                                    garageList[i + j].UsedCapacity = 0;
                                    garageList[i + j].Available = 4;
                                }

                                AnsiConsole.Markup($"You have parked for [blue]{vehicle.CalculateParkingTime(DateTime.Now)}[/] ");
                                AnsiConsole.Markup($"The total cost is [blue]{vehicle.CalculateParkingCost(DateTime.Now)}[/] ");

                                Console.WriteLine($"Bus {vehicle.RegNumber} removed from spots {i + 1} to {i + 4}");
                                return true;
                            }
                        }
                        else if (moveVehicle == true && i != moveFrom)
                        {
                            if (vehicle.RegNumber == regNumber)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    garageList[i + j].Spots.Remove(vehicle);
                                    garageList[i + j].UsedCapacity = 0;
                                    garageList[i + j].Available = 4;
                                }

                            Console.WriteLine($"Bus {vehicle.RegNumber} removed from spots {i + 1} to {i + 4}");
                            return true;
                        }

                        //if (i + 3 < garageList.Count &&
                        //    garageList[i].Spots.Contains(vehicle) &&
                        //    garageList[i + 1].Spots.Contains(vehicle) &&
                        //    garageList[i + 2].Spots.Contains(vehicle) &&
                        //    garageList[i + 3].Spots.Contains(vehicle))
                        //{
                        //    for (int j = 0; j < 4; j++)
                        //    {
                        //        garageList[i + j].Spots.Remove(vehicle);
                        //        garageList[i + j].ResetSpot();
                        //    }
                        //    Console.WriteLine($"Bus {vehicle.RegNumber} removed from spots {i + 1} to {i + 4}");
                        //    return true;
                        //}
                    }
                }
            }

            Console.WriteLine($"Vehicle with registration number {regNumber} not found.");
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
                        return vehicle;
                    }
                }
            }
            return null; // Returnera null om ingen matchning hittas
        }


        public ParkingSpot? FindSpot(Vehicle vehicle)
        {
            foreach (var spot in garageList)
            {
                if (spot.Spots.Contains(vehicle))
                {
                    return spot;
                }
            }
            return null; // Returnera null om ingen matchning hittas i någon parkeringsplats
        }



        /*public bool MoveVehicle(string regNumber, bool selectSpace, int space, bool moveBus)
        {
            Vehicle? vehicle = FindVehicle(regNumber);

            if (garageList[space].Occupied != true || vehicle.Space > garageList[space].Available)
            {
                return false;
            }

            if (vehicle != null && moveBus != true)
            {
                RemoveVehicle(regNumber, false);
                ParkVehicle(vehicle, true, space - 1, false);
            }
            else if (vehicle != null && moveBus == true)
            {
                RemoveVehicle(regNumber, true);
                ParkVehicle(vehicle, true, space - 1, true);
            }
            return true;
        }*/


        public bool MoveVehicle(string regNumber, bool selectSpace, int space, bool moveBus, bool v)
        {
            // Kontrollera om platsen finns i listan (exempelvis om space är ett giltigt index)
            if (space < 0 || space >= garageList.Count)
            {
                return false; // Ogiltig parkeringsplats
            }

            Vehicle? vehicle = FindVehicle(regNumber);

            if (vehicle == null)
            {
                return false; // Fordonet finns inte
            }

            var currentSpot = garageList.FirstOrDefault(spot => spot.Spots.Any(v => v.RegNumber == regNumber));
            if (currentSpot == null)
            {
                return false; // Fordonet är inte parkerat någonstans
            }

            var targetSpot = garageList[space - 1]; // Om index är korrekt, hämta parkeringsplatsen
            if (moveBus != true && vehicle.Space > targetSpot.Available || moveBus == true && targetSpot.Available != 4)
            {
                return false; // Den nya platsen är upptagen
            }

            //var targetSpot = garageList[space - 1]; // Om index är korrekt, hämta parkeringsplatsen
            //if (targetSpot.Occupied)
            //{
            //    return false; // Den nya platsen är upptagen
            //}

            // Flytta fordonet
            ParkVehicle(vehicle, true, space, moveBus);
            RemoveVehicle(regNumber, moveBus, space, true);
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
                if (spot.Available > 0 && spot.Available < 4)
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

        public void InitializeParkingData()
        {
            // Om filen inte finns, skapa en ny med tom data
            if (!File.Exists("parking_data.json"))
            {
                var emptyParkingData = new ParkingData
                {
                    ParkingSpots = new List<ParkingSpotData>()
                };

                string json = JsonConvert.SerializeObject(emptyParkingData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("parking_data.json", json);
            }
        }

    }
}