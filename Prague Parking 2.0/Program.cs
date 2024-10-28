using PragueParking_2._0;

internal class Program
{
    private static void Main()
    {
        //Initialize garage with parking spaces
        Garage.InitGarage();

        //Create car and park
        Car one = new Car("ABC123");
        one.ParkCar();

        //Create mc and park
        MC mc = new MC("QWE123");
        mc.ParkMC();


        //Test print to console
        Console.WriteLine(one.Space);   
        Console.WriteLine(one.TypeOfVehicle);
        Console.WriteLine(one.Rate);
        Console.WriteLine(one.RegNumber);
    }
}