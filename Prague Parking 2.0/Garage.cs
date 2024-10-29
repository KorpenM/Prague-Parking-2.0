using System;
using System.Collections.Generic;

namespace PragueParking_2._0
{
    internal class Garage
    {
        private int capacity = 100;
        public List<ParkingSpot> garageList = new List<ParkingSpot>();

        public Garage() // Constructor for Garage
        {
            InitializeGarage();
        }

        public void InitGarage() // Create the garage (can be called to initialize)
        {
            PrintGarage();
        }

        public void InitializeGarage() // Add parking spots to the garage
        {
            for (int i = 0; i < capacity; i++)
            {
                garageList.Add(new ParkingSpot());
                garageList[i].ID = i;  // ID for the spot
                garageList[i].Occupied = false; // Initially set as not occupied
            }
        }

        public bool ParkVehicle(Vehicle vehicle) // Park vehicle
        {
            foreach (var spot in garageList)
            {
                if (!spot.Occupied) // Find an available spot
                {
                    spot.Occupied = true;
                    spot.ParkedVehicle = vehicle; // Add vehicle to the spot
                    vehicle.ParkingStartTime = DateTime.Now; // Set parking start time

                    Console.WriteLine($"{vehicle.TypeOfVehicle} with registration number {vehicle.RegNumber} has been parked on spot {spot.ID + 1}.");
                    return true; // Vehicle parked
                }
            }
            return false;
        }

        public bool RemoveVehicle(string regNumber)
        {
            foreach (var spot in garageList)
            {
                if (spot.Occupied && spot.ParkedVehicle?.RegNumber == regNumber)
                {
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

            // Debugging
            Console.WriteLine($"Attempting to move vehicle from spot {fromSpot + 1} to spot {toSpot + 1}.");
            Console.WriteLine($"From Spot Occupied: {garageList[fromSpot].Occupied}");
            Console.WriteLine($"From Spot Vehicle RegNumber: {garageList[fromSpot].ParkedVehicle?.RegNumber ?? "None"}");

            if (!garageList[fromSpot].Occupied || garageList[fromSpot].ParkedVehicle?.RegNumber != regNumber)
            {
                Console.WriteLine("No vehicle registered on the from-spot.");
                return false;
            }

            if (garageList[toSpot].Occupied)
            {
                Console.WriteLine("Cannot move vehicle to this spot. It is already occupied.");
                return false;
            }

            var vehicle = garageList[fromSpot].ParkedVehicle; // Hämta fordonet från 'fromSpot'
            garageList[toSpot].Occupied = true; // Markera den nya platsen som ockuperad
            garageList[toSpot].ParkedVehicle = vehicle; // Tilldela fordonet till den nya platsen

            garageList[fromSpot].Occupied = false; // Markera den gamla platsen som ledig
            garageList[fromSpot].ParkedVehicle = null; // Ta bort fordonet från den gamla platsen

            Console.WriteLine($"Vehicle moved from spot {fromSpot + 1} to spot {toSpot + 1}.");
            return true;
        }


        public void PrintGarage()
        {
            Console.Clear();
            Console.WriteLine("=== Parking Spots ===\n");

            foreach (ParkingSpot spot in garageList)
            {
                if (spot.Occupied)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Parking Spot {spot.ID + 1}: Occupied by {spot.ParkedVehicle.RegNumber}");
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
                    Console.ForegroundColor = ConsoleColor.Red; // Occupied
                }

                Console.Write($"[{spot.ID + 1:D3}] "); // Format spot ID
                Console.ResetColor();
            }

            Console.WriteLine("\n");
        }

        // Additional methods for colored parking spots and registered vehicles
        public void PrintColorGarage()
        {
            
        }

        public void ShowRegisteredVehicles()
        {

        }

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

        public void OptimizeParking()
        {
            
        }
    }
}
