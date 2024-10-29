namespace PragueParking_2._0
{
    public enum VehicleType
    {
        Bike = 1,
        MC = 2,
        Car = 4,
        Bus = 16,
    }

    internal class Vehicle
    {
        public string RegNumber { get; set; }
        public VehicleType TypeOfVehicle { get; set; }
        public double Rate { get; set; }
        public double Space { get; set; }
        public DateTime ParkingStartTime { get; set; }

        public Vehicle(string regNumber, VehicleType typeOfVehicle)
        {
            RegNumber = regNumber;
            TypeOfVehicle = typeOfVehicle;
            SetVehicleProperties(typeOfVehicle);
            ParkingStartTime = DateTime.Now; // Sätt starttiden vid konstruktion
        }

        private void SetVehicleProperties(VehicleType typeOfVehicle)
        {
            switch (typeOfVehicle)
            {
                case VehicleType.Bike:
                    Rate = 5;
                    Space = 1;
                    break;
                case VehicleType.MC:
                    Rate = 10;
                    Space = 2;
                    break;
                case VehicleType.Car:
                    Rate = 20;
                    Space = 4;
                    break;
                case VehicleType.Bus:
                    Rate = 40;
                    Space = 16;
                    break;
            }
        }

        public double CalculateParkingCost()
        {
            TimeSpan parkedDuration = DateTime.Now - ParkingStartTime;
            return Space * Rate * parkedDuration.TotalHours; // Beräkna kostnad baserat på tid
        }
    }

    class Bike : Vehicle
    {
        public Bike(string regNumber) : base(regNumber, VehicleType.Bike) { }
    }

    class MC : Vehicle
    {
        public MC(string regNumber) : base(regNumber, VehicleType.MC) { }
    }

    class Car : Vehicle
    {
        public Car(string regNumber) : base(regNumber, VehicleType.Car) { }
    }

    class Bus : Vehicle
    {
        public Bus(string regNumber) : base(regNumber, VehicleType.Bus) { }

    }
}
