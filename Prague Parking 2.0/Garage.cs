using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using PragueParking_2._0;

namespace Prague_Parking_2._0
{

    interface IParkVehicle //Used in Vehicle to park itself in garage.
    {
        public string RegNumber { get; set; }
        //public void ParkVehicle(Vehicle vehicle);

    }
    class Garage
    {
        public string RegNumber { get; set; }
        private int capacity = 100;
        public List<ParkingSpot> garageList = new List<ParkingSpot>();
        public ParkingSettings settings; // Store settings from JSON

        public List<ParkedVehicle> ParkedVehicles { get; set; } = new List<ParkedVehicle>();


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
                        parkingSpot.Add(vehicle);
                        //parkingSpot.UpdateSpot(vehicle);
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

            string json = JsonConvert.SerializeObject(parkingData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("parking_data.json", json);
        }



        private void AddAndSaveParkedVehicle(Vehicle vehicle, params int[] spots)
        {
            foreach (int spot in spots)
            {
                ParkedVehicles.Add(new ParkedVehicle
                {
                    RegistrationNumber = vehicle.RegNumber,
                    VehicleType = vehicle.GetType().Name,
                    ParkingSpot = spot,
                    EntryTime = DateTime.Now
                });
            }

            SaveParkingData(); // Spara parkeringstillståndet i JSON-fil
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
            for (int i = 0; i < garageList.Count; i++)
            {
                //Park car or smaller.
                if (!selectSpace && !parkingBus && space == 0)
                {
                    if (vehicle.Space <= garageList[i].Available)
                    {
                        Console.WriteLine("Added in Garage");
                        garageList[i].Add(vehicle);
                        break;
                    }
                    else
                    {
                        //Print string of currently parked vehicles
                        garageList[i].ToString();
                    }
                }
                else if (selectSpace && !parkingBus) //For move method
                {
                    if (vehicle.Space <= garageList[i].Available)
                    {
                        Console.WriteLine("Added in Garage");
                        garageList[space].Add(vehicle);
                        break;
                    }
                    else
                    {
                        //Print string of currently parked vehicles
                        garageList[space].ToString();
                    }
                }

                //Park buss
            }
        }

        public bool RemoveVehicle(string regNumber, bool removeBus)
        {

            removeBus = false;

            //foreach (ParkingSpot parkingSpot in garageList)
            foreach (ParkingSpot parkingSpot in garageList)
            {
                if (parkingSpot.Spots.Count > 0)
                {
                    foreach (Vehicle vehicle in parkingSpot.Spots)
                    {
                        if (vehicle.RegNumber == regNumber)
                        {
                            Console.WriteLine(vehicle.RegNumber);
                            parkingSpot.Remove(vehicle);
                            //Console.WriteLine("Removed from garage");
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }

            }

            return true;
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

        public ParkingSpot? FindSpot(Vehicle vehicle)
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

            if (vehicle != null && !moveBus)
            {
                garageList[space].CheckAvailability(vehicle);
                if (garageList[space].CanPark)
                {
                    RemoveVehicle(regNumber, false);
                    ParkVehicle(vehicle, true, space, false);
                }
            }
            else if (vehicle != null && moveBus)
            {
                RemoveVehicle(regNumber, true);
                ParkVehicle(vehicle, true, space, true);
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
    }
}