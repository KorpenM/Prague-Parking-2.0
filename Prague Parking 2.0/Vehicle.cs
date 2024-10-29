namespace PragueParking_2._0
{
    public enum VehicleType
    {
        Bike = 1,
        MC = 2,
        Car = 4,
        Bus = 16,
    }

    internal class Vehicle : ParkingSpot
    {
        private string? regNumber;
        private DateTime parkingStartTime; // Tidpunkt för parkering
        private double rate;
        private double space;

        public string? RegNumber { get; set; }
        public VehicleType TypeOfVehicle { get; set; }
        public double Rate { get; set; }
        public double Space { get; set; }
        public double ParkingCost { get; private set; } // Kostnaden ska beräknas

        // Konstruktor som tar emot regNumber och VehicleType
        public Vehicle(string regNumber, VehicleType typeOfVehicle)
        {
            this.RegNumber = regNumber;
            this.TypeOfVehicle = typeOfVehicle;
            SetVehicleProperties(typeOfVehicle);
            this.parkingStartTime = DateTime.Now; // Sätter starttiden för parkeringen
        }

        private void SetVehicleProperties(VehicleType typeOfVehicle)
        {
            switch (typeOfVehicle)
            {
                case VehicleType.Bike:
                    this.Rate = 5;
                    this.Space = 1;
                    break;
                case VehicleType.MC:
                    this.Rate = 10;
                    this.Space = 2;
                    break;
                case VehicleType.Car:
                    this.Rate = 20;
                    this.Space = 4;
                    break;
                case VehicleType.Bus:
                    this.Rate = 40;
                    this.Space = 16;
                    break;
            }
        }

        public double CalculateParkingCost()
        {
            TimeSpan parkedDuration = DateTime.Now - parkingStartTime; // Beräkna hur länge fordonet har varit parkerat

            // Kolla om parkeringsdurationen är mindre än eller lika med 10 minuter
            if (parkedDuration.TotalMinutes <= 10)
            {
                return 0; // Gratis de första 10 minuterna
            }

            // Beräkna kostnaden baserat på den tid som överstiger 10 minuter
            double chargeableMinutes = parkedDuration.TotalMinutes - 10; // Tid som debiteras
            double cost = (chargeableMinutes / 60) * Rate; // Kostnaden för tiden i timmar
            ParkingCost = Math.Round(cost, 2); // Avrunda kostnaden till två decimaler
            return ParkingCost;
        }

        public void ParkVehicle()
        {
            Console.WriteLine($"Vehicle with reg: {RegNumber} is parked.");
        }
    }

    class Bike : Vehicle
    {
        public Bike(string regNumber) : base(regNumber, VehicleType.Bike) { }

        public void ParkBike()
        {
            ParkVehicle();
            Console.WriteLine($"Bike with reg: {RegNumber} is parked.");
        }
    }

    class MC : Vehicle
    {
        public MC(string regNumber) : base(regNumber, VehicleType.MC) { }

        public void ParkMC()
        {
            ParkVehicle();
            Console.WriteLine($"MC with reg: {RegNumber} is parked.");
        }
    }

    class Car : Vehicle
    {
        public Car(string regNumber) : base(regNumber, VehicleType.Car) { }

        public void ParkCar()
        {
            ParkVehicle();
            Console.WriteLine($"Car with reg: {RegNumber} is parked.");
        }
    }

    class Bus : Vehicle
    {
        public Bus(string regNumber) : base(regNumber, VehicleType.Bus) { }

        public void ParkBus()
        {
            ParkVehicle();
            Console.WriteLine($"Bus with reg: {RegNumber} is parked.");
        }
    }
}
