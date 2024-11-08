using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prague_Parking_2._0;

namespace PragueParking.UnitTests
{
    [TestClass]
    public class GarageTest
    {
        [TestMethod]
        public void FindVehicle_CarFound_ReturnTrue()
        {
            //Arrange
            var garage = new Garage();

            string registrationPlate = "FTD123";
            Car myCar = new Car(registrationPlate);

            bool foundParkedCar = false;

            //Act
            garage.ParkVehicle(myCar, false, 0, false);

            var result = garage.FindVehicle(registrationPlate);

            if (result != null)
            {
                foundParkedCar = true;
            }

            //Assert
            Assert.IsTrue(foundParkedCar);
        }

        [TestMethod]
        public void FindVehicle_CarNotFound_ReturnFalse()
        {
            //Arrange
            var garage = new Garage();

            string registrationPlate = "FTD123";
            Car myCar = new Car(registrationPlate);

            string wrongRegistrationPlate = "FTD124";

            bool foundParkedCar = false;

            //Act
            garage.ParkVehicle(myCar, false, 0, false);

            var result = garage.FindVehicle(wrongRegistrationPlate);

            if (result != null)
            {
                foundParkedCar = true;
            }

            //Assert
            Assert.IsFalse(foundParkedCar);
        }
    }
}