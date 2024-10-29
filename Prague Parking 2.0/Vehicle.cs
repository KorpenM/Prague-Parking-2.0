namespace PragueParking_2._0
{
    internal class Vehicle : ParkingSpot
    {
        private string? regNumber;
        private string? typeOfVehicle;
        private double rate;
        private double space;
        private double parkingCost = 0;

        public string? RegNumber { get; set; }
        public string? TypeOfVehicle { get; set; }
        public double Rate { get; set; }
        public double Space { get; set; }
        public double ParkingCost { get; set; }

        public Vehicle()
        {
            this.regNumber = RegNumber;
            this.typeOfVehicle = TypeOfVehicle;
            this.rate = Rate;
            this.space = Space;
            //this.parkingCost = ParkingCost;
        }

        public override void ParkVehicle()
        {
            base.ParkVehicle();
            Console.WriteLine("ParkVehicle in class Vehicle");
        }
        public static DateTime ParkingStart() //Should this be in ParkingSpot/Garage instead?
        {
            DateTime start = DateTime.Now;
            return start;
        }

        public void ParkingEnd() //Should this be in ParkingSpot/Garage instead?
        {

        }

        public virtual void CalculateSpace(double space)
        {

        }

        public double CalculateParkingCost(double space, double rate) //Should this be in ParkingSpot/Garage instead?
        {
            ParkingCost = space * rate;
            return ParkingCost;
        }
    }

    class Car : Vehicle
    {
        public Car(string regNumber)
        {
            base.RegNumber = regNumber;
            base.TypeOfVehicle = "car";
            base.Rate = 20;
            base.Space = 1;
            //base.ParkingCost = base.CalculateParkingCost(base.space, rate);
            ParkingStart();
        }

        public void ParkCar()
        {
            //Console.WriteLine($"Car with reg: {RegNumber} is parked");
            base.ParkVehicle();
            Console.WriteLine($"Car with reg: {RegNumber} is parked, ParkVehicle in Car : Vehicle");
        }
    }

    class MC : Vehicle
    {
        public MC(string regNumber)
        {
            base.RegNumber = regNumber;
            base.TypeOfVehicle = "mc";
            base.Rate = 20;
            base.Space = 1;
            //base.ParkingCost = base.CalculateParkingCost(base.space, rate);
            ParkingStart();
        }

        public void ParkMC()
        {
            base.ParkVehicle();
        }
    }
}
