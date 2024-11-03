using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prague_Parking_2._0;

namespace PragueParking.UnitTests
{
    [TestClass]
    public class GarageTest
    {
        [TestMethod]
        public void RemoveVehicle_NoVehicleParked_ReturnFalse()
        {
            //Arrange
            var garage = new Garage();
            string registrationPlate = "FTD123";

            //Act
            var result = garage.RemoveVehicle(registrationPlate, false);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveVehicle_ParkedVehicleRemoved_ReturnTrue()
        {
            //Arrange
            var garage = new Garage();
            string registrationPlate = "FTD321";
            var vehicle = new Car(registrationPlate);
            garage.ParkVehicle(vehicle, true, 4, false);

            //Act
            var result = garage.RemoveVehicle(registrationPlate, false);

            //Assert
            Assert.IsTrue(result);
        }
    }
}