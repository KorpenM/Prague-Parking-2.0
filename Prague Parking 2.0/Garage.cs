using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;

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

            //InitializeGarage();
            if (garageList.Count > 0) return;

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

        public bool ParkVehicle(Vehicle vehicle)
        {
            if (vehicle.Type != "Bus")
            {
                //Parks vehicle at first possible space according to vehicle size (vehicle.Space).
                for (int i = 0; i < garageList.Count; i++)
                {
                    var parkingSpot = garageList[i];
                    int capacity = parkingSpot.SpotCapacity;
                    int used = parkingSpot.UsedCapacity;
                    int available = parkingSpot.Available;
                    var spots = parkingSpot.Spots;

                    if (vehicle.Space <= capacity && vehicle.Space <= available)
                    {
                        parkingSpot.UsedCapacity += vehicle.Space;
                        parkingSpot.Available = capacity - vehicle.Space;
                        vehicle.ParkingStartTime = DateTime.Now;
                        spots.Add(vehicle);
                        if (parkingSpot.Available == 0) { parkingSpot.Occupied = true; }
                        Console.WriteLine($"{vehicle.Type} with reg {vehicle.RegNumber} has been parked on spot {parkingSpot.ID + 1}.");
                        Console.WriteLine($"This spot now has used {parkingSpot.UsedCapacity} parking spaces. There are {parkingSpot.Available} spaces available.");
                        break;
                    }
                    else if (vehicle.Space > capacity || vehicle.Space > available || parkingSpot.Occupied) //Implementation for buss?
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("No space left");
                    }
                }
            }
            return true;
        }

        public bool RemoveVehicle(string regNumber)
        {
            foreach (var spot in garageList)
            {
                if (spot.Occupied && spot.ParkedVehicle?.RegNumber == regNumber)
                {
                    var parkedDuration = DateTime.Now - spot.ParkedVehicle.ParkingStartTime; // Beräkna parkeringstid

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

        public bool MoveVehicle(string regNumber, int fromSpot, int toSpot) //*2 - Needs to be possible to move bike to a space with a bike
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

            if (vehicle.Type == "Bus")
            {
                if (fromSpot + 3 < garageList.Count && // Kontrollera att bussens alla platser finns
                    garageList[fromSpot + 1].ParkedVehicle?.RegNumber == regNumber &&
                    garageList[fromSpot + 2].ParkedVehicle?.RegNumber == regNumber &&
                    garageList[fromSpot + 3].ParkedVehicle?.RegNumber == regNumber)
                {
                    // Kontrollera att de nya platserna är lediga och att de inte överstiger plats 50
                    if (toSpot + 3 < garageList.Count && toSpot + 3 < 50 && // Ny plats får inte vara > 50
                        !garageList[toSpot].Occupied &&
                        !garageList[toSpot + 1].Occupied &&
                        !garageList[toSpot + 2].Occupied &&
                        !garageList[toSpot + 3].Occupied)
                    {
                        // Flytta bussen till de nya platserna
                        for (int i = 0; i < 4; i++)
                        {
                            garageList[toSpot + i].Occupied = true;
                            garageList[toSpot + i].ParkedVehicle = vehicle;
                        }

                        // Frigör de gamla platserna
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
                    return false; // Ingen buss på de angivna platserna
                }
            }

            // Om det inte är en buss, flytta fordonet som vanligt
            if (garageList[toSpot].Occupied)
            {
                Console.WriteLine("Cannot move vehicle to this spot. It is already occupied.");
                return false; // Kan inte flytta till en upptagen plats
            }

            // Flytta fordonet från en vanlig plats
            garageList[toSpot].Occupied = true; // Markera den nya platsen som upptagen
            garageList[toSpot].ParkedVehicle = vehicle; // Flytta fordonet

            garageList[fromSpot].Occupied = false; // Markera den gamla platsen som ledig
            garageList[fromSpot].ParkedVehicle = null; // Ta bort fordonet från den gamla platsen

            Console.WriteLine($"Vehicle moved from spot {fromSpot + 1} to spot {toSpot + 1}.");
            return true; // Fordonet flyttat
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
                else if (spot.Available == 0)
                {
                    Console.ForegroundColor= ConsoleColor.Red;
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
                    Console.WriteLine($"Spot {spot.ID + 1}: {spot.ParkedVehicle.Type} - Reg: {spot.ParkedVehicle.RegNumber}");
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