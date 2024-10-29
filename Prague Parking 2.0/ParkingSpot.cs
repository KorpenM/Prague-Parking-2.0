namespace PragueParking_2._0
{
    public class ParkingSpot
    {
        protected string _vehicleData;
        protected string _parkToNumber;

        public ParkingSpot()
        {
            this._vehicleData = vehicleData;
            this._parkToNumber = parkToNumber;
        }

        public string vehicleData { get; set; }
        public string parkToNumber { get; set; }
    }
}