namespace Prague_Parking_2._0;

public interface ICalculateParkingCost
{
    public double CalculateParkingCost(DateTime exitTime);
}
public abstract class Vehicle : ICalculateParkingCost
{
    public  string? RegNumber { get; set; }
    //public abstract string Type { get; set; }
    public abstract int Rate { get; set; }
    public abstract int Space { get; set; }
    public DateTime ParkingStartime { get; set; } = DateTime.Now;
    public abstract double CalculateParkingCost(DateTime exitTime);

}


class Bike : Vehicle
{

    public string? RegNumber { get; set; }
    //public string Type { get; set; } = "Bike";
    public override int Rate { get; set; } = 5;
    public override int Space { get; set; } = 1;

    public Bike(string? regNumber)
    {
        RegNumber = regNumber;
    }


    public override double CalculateParkingCost(DateTime exitTime )
    {
        TimeSpan parkedDuration = DateTime.Now - exitTime;
        return Space * Rate * parkedDuration.TotalHours;
    }

}



class MC : Vehicle
{
    public string? RegNumber { get; set; }
    //public override string Type { get; set; } = "MC";
    public override int Rate { get; set; } = 10;
    public override int Space { get; set; } = 2;

    public MC(string? regNumber)
    {
        RegNumber = regNumber;
    }


    public override double CalculateParkingCost(DateTime exitTime)
    {
        TimeSpan parkedDuration = DateTime.Now - exitTime;
        return Space * Rate * parkedDuration.TotalHours;
    }

}

class Car : Vehicle
{

    public string? RegNumber { get; set; }
    //public override string Type { get; set; } = "Car";
    public override int Rate { get; set; } = 20;
    public override int Space { get; set; } = 4;

    public Car(string? regNumber)
    {
        RegNumber = regNumber;
    }
    public override double CalculateParkingCost(DateTime exitTime)
    {
        TimeSpan parkedDuration = DateTime.Now - exitTime;
        return Space * Rate * parkedDuration.TotalHours;
    }

}

class Bus : Vehicle
{

    public  string? RegNumber { get; set; }
    //public override string Type { get; set; } = "Bus";
    public override int Rate { get; set; } = 40;
    public override int Space { get; set; } = 16;

    public Bus(string? regNumber)
    {
        RegNumber = regNumber;
    }
    public override double CalculateParkingCost(DateTime exitTime)
    {
        TimeSpan parkedDuration = DateTime.Now - exitTime;
        return Space * Rate * parkedDuration.TotalHours;
    }
}

