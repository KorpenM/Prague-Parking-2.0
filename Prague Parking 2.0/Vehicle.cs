namespace PragueParking_2._0;

public interface ICalculateParkingCost
{
    public double CalculateParkingCost(double Space, int Rate, DateTime ParkingStartTime);
}
public abstract class Vehicle : ICalculateParkingCost
{
    public abstract string? RegNumber { get; set; }
    public abstract string Type { get; set; }
    public abstract double Rate { get; set; }
    public abstract double Space { get; set; }
    public DateTime ParkingStartTime { get; set; }

    public Vehicle()
    {

    }
    public double CalculateParkingCost(double Space, int Rate, DateTime ParkingStartTime)
    {
        TimeSpan parkedDuration = DateTime.Now - ParkingStartTime;
        return Space * Rate * parkedDuration.TotalHours;
    }


}


class Bike : Vehicle
{
    public override string? RegNumber { get; set; }
    public override string Type { get; set; } = "Bike";
    public override double Rate { get; set; } = 5;
    public override double Space { get; set; } = 1;

    public Bike(string regnumber)
    {
        this.RegNumber = regnumber;
    }

}



class MC : Vehicle
{
    public override string? RegNumber { get; set; }
    public override string Type { get; set; } = "MC";
    public override double Rate { get; set; } = 10;
    public override double Space { get; set; } = 2;

    public MC(string regnumber)
    {
        this.RegNumber = regnumber;
    }

}

class Car : Vehicle
{
    public override string? RegNumber { get; set; }
    public override string Type { get; set; } = "Car";
    public override double Rate { get; set; } = 20;
    public override double Space { get; set; } = 4;

    public Car(string regnumber)
    {
        this.RegNumber = regnumber;
    }

}

class Bus : Vehicle
{
    public override string? RegNumber { get; set; }
    public override string Type { get; set; } = "Bus";
    public override double Rate { get; set; } = 40;
    public override double Space { get; set; } = 16;

    public Bus(string regnumber)
    {
        this.RegNumber = regnumber;
    }

}


