using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using PragueParking_2._0;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("GarageTest")]
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
        internal List<ParkingSpot> garageList = new List<ParkingSpot>();
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
                    ParkingSpot secondParkingSpot = garageList[i+1];
                    ParkingSpot thirdParkingSpot = garageList[i+2];
                    ParkingSpot fourthParkingSpot = garageList[i+3];

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
            else if (selectSpace == true && space != 0)
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
                        if (i == space && parkingSpot.Available == 4 && secondParkingSpot.Available == 4
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
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public bool RemoveVehicle(string regNumber, bool removeBus)
        {
            foreach (ParkingSpot spot in garageList)
            {
                foreach (Vehicle vehicle in spot.Spots)
                {
                    if (vehicle.RegNumber == regNumber && vehicle.EndParking && removeBus != true)
                    {
                        AnsiConsole.Markup($"You have parked for [blue]{vehicle.CalculateParkingTime}[/] ");
                        AnsiConsole.Markup($"The total cost is [blue]{vehicle.CalculateParkingCost}[/] ");
                        spot.Spots.Remove(vehicle);
                        spot.UpdateSpot(vehicle);
                        return true;
                    }
                    else if (removeBus == true)
                    {
                        for (int j = 0; j < garageList.Count; j++)
                        {
                            ParkingSpot parkingSpot = garageList[j];
                            ParkingSpot secondParkingSpot = garageList[j + 1];
                            ParkingSpot thirdParkingSpot = garageList[j + 2];
                            ParkingSpot fourthParkingSpot = garageList[j + 3];

                            if (vehicle.RegNumber == regNumber)
                            {
                                //Need to add a function to calculate total cost
                                garageList[j].Spots.Remove(vehicle);
                                garageList[j].ResetSpot();
                                garageList[j + 1].Spots.Remove(vehicle);
                                garageList[j + 1].ResetSpot();
                                garageList[j + 2].Spots.Remove(vehicle);
                                garageList[j + 2].ResetSpot();
                                garageList[j + 3].Spots.Remove(vehicle);
                                garageList[j + 3].ResetSpot();
                                Console.WriteLine("The bus has been reclaimed");
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        spot.Spots.Remove(vehicle);
                        spot.ResetSpot();
                        return true;
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
                        return null;
                    }
                }
                break;
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
            }
            else if (vehicle != null && moveBus == true)
            {
                RemoveVehicle(regNumber, true);
                ParkVehicle(vehicle, true, space - 1, true);
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