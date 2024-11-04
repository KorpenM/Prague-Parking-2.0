using System.Runtime.CompilerServices;


namespace Prague_Parking_2._0
{
    public interface ICalculateParkingCost
    {
        public double CalculateParkingCost(DateTime exitTime);
        //public double CalculateParkingTime(DateTime exitTime);
    }

    public abstract class Vehicle : ICalculateParkingCost
    {
        public abstract string RegNumber { get; set; }
        public abstract int Rate { get; set; }
        public abstract int Space { get; set; }
        public DateTime ParkingStartime { get; set; } = DateTime.Now;
        public abstract double CalculateParkingCost(DateTime exitTime);
        public bool EndParking { get; set; } = false;
        public string CalculateParkingTime(DateTime exitTime)
        {
            TimeSpan parkedDuration = exitTime - ParkingStartime;
            return parkedDuration.ToString(@"hh\:mm\:ss");
        }

        public bool SetEndPardking()
        {
            return EndParking = true;
        }
    }

    public class Bike(string regNumber) : Vehicle
    {

        public override string RegNumber { get; set; } = regNumber;
        public override int Rate { get; set; } = 5;
        public override int Space { get; set; } = 1;
        public override double CalculateParkingCost(DateTime exitTime)
        {
            TimeSpan parkedDuration = DateTime.Now - exitTime;
            return Space * Rate * parkedDuration.TotalHours;
        }
    }

    public class MC(string regNumber) : Vehicle
    {
        public override string RegNumber { get; set; } = regNumber;
        public override int Rate { get; set; } = 10;
        public override int Space { get; set; } = 2;
        public override double CalculateParkingCost(DateTime exitTime)
        {
            TimeSpan parkedDuration = DateTime.Now - exitTime;
            return Space * Rate * parkedDuration.TotalHours;
        }

    }

    public class Car(string regNumber) : Vehicle
    {
        public override string RegNumber { get; set; } = regNumber;
        public override int Rate { get; set; } = 20;
        public override int Space { get; set; } = 4;
        public override double CalculateParkingCost(DateTime exitTime)
        {
            TimeSpan parkedDuration = DateTime.Now - exitTime;
            return Space * Rate * parkedDuration.TotalHours;
        }

    }

    public class Bus(string regNumber) : Vehicle
    {
        public override string RegNumber { get; set; } = regNumber;
        public override int Rate { get; set; } = 40;
        public override int Space { get; set; } = 16;
        public override double CalculateParkingCost(DateTime exitTime)
        {
            TimeSpan parkedDuration = DateTime.Now - exitTime;
            return Space * Rate * parkedDuration.TotalHours;
        }
    }
}