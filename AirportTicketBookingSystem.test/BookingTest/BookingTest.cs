using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.test.DataFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.test.BookingTest
{
    public class BookingTest
    {
        private readonly ITestDataFactory testDataFactory;
        public BookingTest() 
        {
            testDataFactory = new TestDataFactory();
        }

        [Fact]
        public void GetPriceForBookingClass_ShouldReturnEconomyPrice_WhenBookingClassIsEconomy()
        {
            
            var flight = testDataFactory.CreateFlightData
                (economyPrice : 100m,
                businessPrice : 200m,
                firstClassPrice : 300m);

            var booking = testDataFactory.CreateBookingData(flight: flight, bookingClass: BookingClass.Economy);

            var price = booking.Price;

            Assert.Equal(100m, price);
        }

        [Fact]
        public void GetPriceForBookingClass_ShouldReturnBusinessPrice_WhenBookingClassIsBusiness()
        {
            var flight = testDataFactory.CreateFlightData
                (economyPrice: 100m,
                businessPrice: 200m,
                firstClassPrice: 300m);

            var booking = testDataFactory.CreateBookingData(flight: flight, bookingClass: BookingClass.Business);

            var price = booking.Price;

            Assert.Equal(200m, price);
        }

        [Fact]
        public void GetPriceForBookingClass_ShouldReturnFirstClassPrice_WhenBookingClassIsFirstClass()
        {
            var flight = testDataFactory.CreateFlightData
                (economyPrice: 100m,
                businessPrice: 200m,
                firstClassPrice: 300m);

            var booking = testDataFactory.CreateBookingData(flight: flight, bookingClass: BookingClass.FirstClass);

            var price = booking.Price;

            Assert.Equal(300m, price);
        }

        [Fact]
        public void GetPriceForBookingClass_ShouldThrowArgumentException_WhenBookingClassIsInvalid()
        {
            var flight = testDataFactory.CreateFlightData
                (economyPrice: 100m,
                businessPrice: 200m,
                firstClassPrice: 300m);

            var exception = Assert.Throws<TargetInvocationException>(() =>
            {
                testDataFactory.CreateBookingData(
                    flight: flight,
                    bookingClass: (BookingClass)999,
                    bookingDate: DateTime.Now
                );
            });

            var argumentException = Assert.IsType<ArgumentException>(exception.InnerException);
            Assert.Equal("Invalid booking class.", argumentException.Message);
        }

    }
}
