using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;


namespace PragueParking_2._0
{
    internal class Garage
    {
        private int capacity = 100;
        public List<ParkingSpot> garageList = new List<ParkingSpot>();
        public ParkingSettings settings; // Store settings from JSON

        public Garage() // Constructor for Garage
        {
            LoadSettings(); // Load settings from JSON

            InitializeGarage();

            // Debug: check for duplicates or incorrect count
            // Console.WriteLine("Total parking spots: " + garageList.Count);
            // Console.WriteLine("Unique IDs count: " + new HashSet<int>(garageList.Select(s => s.ID)).Count);
        }

        public void InitGarage()
        {
            // PrintGarage();
        }

        /*public void LoadSettings()
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
        }*/


        public void LoadSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);

                // Deserialisera endast "parkingSettings" sektionen
                JObject jsonObj = JObject.Parse(json);
                var parkingSettingsJson = jsonObj["parkingSettings"];

                settings = parkingSettingsJson?.ToObject<ParkingSettings>();

                Console.WriteLine("Settings loaded successfully.");

                if (settings.VehicleTypes == null)
                {
                    settings.VehicleTypes = new Dictionary<string, VehicleTypeInfo>();
                }
            }
            else
            {
                Console.WriteLine("Config file not found. Using default settings.");

                // Om config.json saknas, använd standardinställningar
                settings = new ParkingSettings
                {
                    TotalSpots = 100,
                    FreeParkingMinutes = 10,
                    VehicleTypes = new Dictionary<string, VehicleTypeInfo>
            {
                { "Bike", new VehicleTypeInfo { SpaceRequired = 1, RatePerHour = 5, AllowedSpots = "All spots", NumberOfVehiclesPerSpot = 4 }},
                { "MC", new VehicleTypeInfo { SpaceRequired = 2, RatePerHour = 10, AllowedSpots = "All spots", NumberOfVehiclesPerSpot = 2 }},
                { "Car", new VehicleTypeInfo { SpaceRequired = 4, RatePerHour = 20, AllowedSpots = "All spots", NumberOfVehiclesPerSpot = 1 }},
                { "Bus", new VehicleTypeInfo { SpaceRequired = 16, RatePerHour = 80, AllowedSpots = "Spots 1-50 only", NumberOfVehiclesPerSpot = 1 }},
            }
                };
            }
        }


        public void SaveSettings()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented); // Indenterad format
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine("Settings saved successfully.");
        }

        public void InitializeGarage() // Add parking spots to the garage
        {
            if (garageList.Count > 0) return;

            /*if (garageList.Count > 0)
            {
                Console.WriteLine("Garage already initialized.");
                return;
            }*/

            for (int i = 0; i < capacity; i++)
            {
                garageList.Add(new ParkingSpot { ID = i, Occupied = false });
                // garageList.Add(new ParkingSpot());
                // garageList[i].ID = i;  // ID for the spot
                // garageList[i].Occupied = false; // Initially set as not occupied
            }
            // Debug
            Console.WriteLine($"Garage initialized with {garageList.Count} parking spots.");
        }


        public bool ParkVehicle(Vehicle vehicle)
        {
            if (vehicle.TypeOfVehicle == VehicleType.Bus)
            {
                // Try park bus (4 spots)
                for (int i = 0; i < garageList.Count - 3; i++) //  Check up to 3 spots left
                {
                    if (!garageList[i].Occupied &&
                        !garageList[i + 1].Occupied &&
                        !garageList[i + 2].Occupied &&
                        !garageList[i + 3].Occupied &&
                        garageList[i].CanAcceptVehicle(vehicle) &&
                        garageList[i + 1].CanAcceptVehicle(vehicle) &&
                        garageList[i + 2].CanAcceptVehicle(vehicle) &&
                        garageList[i + 3].CanAcceptVehicle(vehicle))
                    {
                        // Park bus on these 4 spots
                        for (int j = 0; j < 4; j++)
                        {
                            garageList[i + j].Occupied = true;
                            garageList[i + j].ParkedVehicle = vehicle;
                        }
                        vehicle.ParkingStartTime = DateTime.Now; // Parking Start time

                        Console.WriteLine($"Bus with reg {vehicle.RegNumber} has been parked on spots {i + 1}, {i + 2}, {i + 3}, and {i + 4}.");
                        return true; // Bus parked success
                    }
                }
                Console.WriteLine("No available parking spots for the bus.");
                return false;
            }
            else // For other vehicle types
            {
                foreach (var spot in garageList)
                {
                    if (!spot.Occupied && spot.CanAcceptVehicle(vehicle)) // Find free spot
                    {
                        spot.Occupied = true;
                        spot.ParkedVehicle = vehicle; // Add vehicle to spot
                        vehicle.ParkingStartTime = DateTime.Now; // Start park time

                        Console.WriteLine($"{vehicle.TypeOfVehicle} with reg {vehicle.RegNumber} has been parked on spot {spot.ID + 1}.");
                        Console.WriteLine($"Check in: {vehicle.ParkingStartTime:yyyy-MM-dd HH:mm:ss}");
                        return true; // Vehicle parked
                    }
                }
                return false; // No free spot
            }
        }

        public bool RemoveVehicle(string regNumber)
        {
            foreach (var spot in garageList)
            {
                if (spot.Occupied && spot.ParkedVehicle?.RegNumber == regNumber)
                {
                    var parkedDuration = DateTime.Now - spot.ParkedVehicle.ParkingStartTime; // Calculate park time

                    int days = parkedDuration.Days;
                    int hours = parkedDuration.Hours;
                    int minutes = parkedDuration.Minutes;

                    // Create a string to show parkingtime
                    string durationString = "";
                    if (days > 0)
                    {
                        durationString += $"{days} day(s) ";
                    }

                    if (hours > 0)
                    {
                        durationString += $"{hours} hour(s) ";
                    }

                    durationString += $"{minutes} minute(s)";

                    Console.WriteLine($"{regNumber} has been parked for: {durationString}.");
                    // Remove the vehicle
                    spot.Occupied = false;
                    spot.ParkedVehicle = null;
                    return true;
                }
            }
            return false;
        }

        public ParkingSpot? FindVehicle(string regNumber)
        {
            foreach (var spot in garageList)
            {
                if (spot.Occupied && spot.ParkedVehicle?.RegNumber == regNumber)
                {
                    return spot;
                }
            }
            return null;
        }

        public bool MoveVehicle(string regNumber, int fromSpot, int toSpot)
        {
            fromSpot--;
            toSpot--;

            if (fromSpot < 0 || fromSpot >= garageList.Count || toSpot < 0 || toSpot >= garageList.Count)
            {
                Console.WriteLine("Invalid parking spots.");
                return false;
            }


            var vehicle = garageList[fromSpot].ParkedVehicle;
            if (vehicle == null || vehicle.RegNumber != regNumber)
            {
                Console.WriteLine("No vehicle registered on the from-spot.");
                return false;
            }

            if (vehicle.TypeOfVehicle == VehicleType.Bus)
            {
                if (fromSpot + 3 < garageList.Count && // Make sure all spots for bus are available
                    garageList[fromSpot + 1].ParkedVehicle?.RegNumber == regNumber &&
                    garageList[fromSpot + 2].ParkedVehicle?.RegNumber == regNumber &&
                    garageList[fromSpot + 3].ParkedVehicle?.RegNumber == regNumber)
                {
                    // Make sure new spots are free and don't go above 50
                    if (toSpot + 3 < garageList.Count && toSpot + 3 < 50 && // New spot = not above 50
                        !garageList[toSpot].Occupied &&
                        !garageList[toSpot + 1].Occupied &&
                        !garageList[toSpot + 2].Occupied &&
                        !garageList[toSpot + 3].Occupied)
                    {
                        // Move bus to new spots
                        for (int i = 0; i < 4; i++)
                        {
                            garageList[toSpot + i].Occupied = true;
                            garageList[toSpot + i].ParkedVehicle = vehicle;
                        }

                        // Make old spots free
                        for (int i = 0; i < 4; i++)
                        {
                            garageList[fromSpot + i].Occupied = false;
                            garageList[fromSpot + i].ParkedVehicle = null;
                        }

                        Console.WriteLine($"Bus moved from spots {fromSpot + 1}, {fromSpot + 2}, {fromSpot + 3}, {fromSpot + 4} to spots {toSpot + 1}, {toSpot + 2}, {toSpot + 3}, {toSpot + 4}.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Cannot move bus to these spots. They are occupied or out of bounds.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("No vehicle registered on the from-spot.");
                    return false; // No bus on spots
                }
            }

            // If not bus, move other vehicles
            if (garageList[toSpot].Occupied)
            {
                Console.WriteLine("Cannot move vehicle to this spot. It is already occupied.");
                return false;
            }

            // Move vehicle from a normal spot
            garageList[toSpot].Occupied = true; // Mark new spot as busy
            garageList[toSpot].ParkedVehicle = vehicle; // Move the vehicle

            garageList[fromSpot].Occupied = false; // Mark the old spot as free
            garageList[fromSpot].ParkedVehicle = null; // Remove vehicle from the old spot

            Console.WriteLine($"Vehicle moved from spot {fromSpot + 1} to spot {toSpot + 1}.");
            return true;
        }

        public void PrintGarage()
        {
            Console.WriteLine("=== Parking Spots ===\n");

            foreach (ParkingSpot spot in garageList)
            {
                if (spot.Occupied)
                {
                    string spotString = spot.ParkedVehicle.TypeOfVehicle.ToString();
                    int bikeIndex = spotString.IndexOf("Bike");
                    int mcIndex = spotString.IndexOf("MC");
                    int carIndex = spotString.IndexOf("Car");
                    int busIndex = spotString.IndexOf("Bus");

                    if (bikeIndex != -1 && bikeIndex < 3 || mcIndex != -1 && mcIndex < 1 || bikeIndex == 0 && mcIndex == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Parking Spot {spot.ID + 1}: Occupied by {spot.ParkedVehicle.RegNumber}"); // Partly occupied
                    }
                    else if (carIndex != -1 || bikeIndex == 3 || mcIndex == 1 || busIndex != -1 || bikeIndex == 1 && mcIndex == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Parking Spot {spot.ID + 1}: Occupied by {spot.ParkedVehicle.RegNumber}"); // Occupied/busy
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Parking Spot {spot.ID + 1}: Available");
                }
                Console.ResetColor();
            }
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

                if (!spot.Occupied)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Free
                }
                else // Spot occupied
                {
                    string spotString = spot.ParkedVehicle.TypeOfVehicle.ToString();
                    int bikeIndex = spotString.IndexOf("Bike");
                    int mcIndex = spotString.IndexOf("MC");
                    int carIndex = spotString.IndexOf("Car");
                    int busIndex = spotString.IndexOf("Bus");

                    if (bikeIndex != -1 && bikeIndex < 3 || mcIndex != -1 && mcIndex < 1 || bikeIndex == 0 && mcIndex == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; //Partly occupied
                    }
                    else if (carIndex != -1 || bikeIndex == 3 || mcIndex == 1 || busIndex != -1 || bikeIndex == 1 && mcIndex == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Occupied
                    }
                }

                Console.Write($"[{spot.ID + 1:D3}] "); // Format spot ID
                Console.ResetColor();
            }

            Console.WriteLine("\n");
        }

        //public void PrintColorGarage()
        //{

        //}


        public void PrintRegisteredVehicles()
        {
            Console.Clear();
            Console.WriteLine("=== Registered Vehicles ===\n");

            bool hasVehicles = false;

            foreach (var spot in garageList)
            {
                if (spot.Occupied)
                {
                    Console.WriteLine($"Spot {spot.ID + 1}: {spot.ParkedVehicle.TypeOfVehicle} - Reg: {spot.ParkedVehicle.RegNumber}");
                    hasVehicles = true;
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