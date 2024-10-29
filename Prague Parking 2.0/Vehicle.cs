namespace PragueParking_2._0
{
    public class Vehicle
    {
        private string _regPlate;
        private string _vehicleType;
        private int _vehicleSize;

        public Vehicle()
        {
            this._regPlate = regPlate;
            this._vehicleType = vehicleType;
            this._vehicleSize = vehicleSize;
        }

        public string regPlate { get; set; }
        public string vehicleType { get; set; }
        public int vehicleSize { get; set; }
    }

    public class MC : Vehicle
    {
        public MC(string mcPlate)
        {
            this.regPlate = mcPlate;
            this.vehicleType = "MC";
            this.vehicleSize = 2;
        }
    }

    public class Car : Vehicle
    {
        public Car(string carPlate)
        {
            this.regPlate = carPlate;
            this.vehicleType = "CAR";
            this.vehicleSize = 4;
        }
    }
}