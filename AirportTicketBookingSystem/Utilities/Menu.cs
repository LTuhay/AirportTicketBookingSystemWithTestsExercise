using AirportTicketBookingSystem.Controller;
using AirportTicketBookingSystem.Controllers;
using AirportTicketBookingSystem.DTO;


namespace AirportTicketBookingSystem.Utilities
{
    public class Menu
    {
        private PassengerController pc;
        private BookingController bc;
        public Menu(PassengerController passengerControler, BookingController bookingController)
        {
            this.pc = passengerControler;
            this.bc = bookingController;
        }
        public void StartMenu()
        {
            MainMenu();


            void MainMenu()
            {
                bool exit = false;

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Welcome!");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("1. Go to passengers menu");
                    Console.WriteLine("2. Go to manager menu");
                    Console.WriteLine("0. Exit");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            PassengerMenu();
                            break;
                        case "2":
                            ManagerMenu();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }
                } while (!exit);

                Console.WriteLine("Thank you for using our Airport Ticket Booking System");

            }

            void PassengerMenu()
            {
                bool exit = false;

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("1.Search for avaiables flights");
                    Console.WriteLine("2.Book a flight");
                    Console.WriteLine("3. Manage bookings");
                    Console.WriteLine("0. Exit");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            SearchForFlights();
                            break;
                        case "2":
                            BookAFlight();
                            break;
                        case "3":
                            ManageBookings();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }

                } while (!exit);

            }



            void SearchForFlights()
            {

                var flightData = EnterFlightData();
                string? departure = flightData.departure;
                string? destination = flightData.destination;
                DateTime departureDate = flightData.departureDate;
                string? departureAirport = flightData.departureAirport;
                string? destinationAirport = flightData.destinationAirport;
                decimal minPrice = flightData.minPrice;
                decimal maxPrice = flightData.maxPrice;
                string? travelClass = flightData.travelClass;

                List<FlightDTO> searchResults = bc.GetFlightsByParams(minPrice, maxPrice, departure, destination, departureDate, departureAirport, destinationAirport, travelClass);

                Console.WriteLine("***************************************");

                if (searchResults.Count == 0)
                    Console.WriteLine("No search Results:");
                else
                {
                    Console.WriteLine("Search Results:");
                    foreach (var flight in searchResults)
                    {
                        Console.WriteLine(flight);
                    }
                }


                Console.WriteLine("***************************************");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }



            void BookAFlight()
            {
                Console.WriteLine("Enter flight number or press ENTER to cancel):");
                string? flightInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(flightInput))
                {
                    FlightDTO? dto = bc.GetFlightsByNumber(flightInput);
                    if (dto != null)
                    {
                        EnterBookingData(dto);
                    }
                    else
                    {
                        Console.WriteLine($"Flight not found with number {flightInput}.");
                    }
                }
            }

            void EnterBookingData(FlightDTO fDto)
            {

                PassengerDTO? pDto = Login();


                string bookingClass = SelectFlightClass(fDto);

                String bookingID = bc.GenerateUniqueBookingId().ToString();
                DateTime currentDateTime = DateTime.Now;
                decimal price = Price(bookingClass, fDto);
                BookingDTO bDTO = new BookingDTO(bookingID, pDto, fDto, bookingClass, currentDateTime, price);
                bc.AddBooking(bDTO);

                Console.WriteLine("***************************************");
                Console.WriteLine("Your booking has been successfully confirmed.");
                Console.WriteLine($"The booking fee is: {price}");


            }

            decimal Price(string bookingClass, FlightDTO dto)
            {
                string bookingClassForPrice = bookingClass + "Price";
                var property = typeof(FlightDTO).GetProperty(bookingClassForPrice);
                decimal price = (decimal)property.GetValue(dto);
                return price;
            }



            void ManageBookings()
            {

                PassengerDTO? pDto = Login();
                if (pDto != null)
                {
                    bool exit = false;

                    do
                    {

                        Console.WriteLine("***************************************");
                        Console.WriteLine("1. View my bookings");
                        Console.WriteLine("2. Modify a booking");
                        Console.WriteLine("3. Cancel a booking");
                        Console.WriteLine("0. Exit");

                        string? userSelection = Console.ReadLine();

                        switch (userSelection)
                        {
                            case "1":
                                ViewBookings(pDto);
                                break;
                            case "2":
                                UpdateBooking(pDto);
                                break;
                            case "3":
                                CancelBooking(pDto);
                                break;
                            case "0":
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid selection. Please try again");
                                break;
                        }
                    } while (!exit);
                }


            }

            void ViewBookings(PassengerDTO pDto)
            {

                {
                    List<BookingDTO> bookings = bc.GetBookingsByPassenger(pDto.Id);
                    if (bookings.Count > 0)
                    {
                        foreach (BookingDTO booking in bookings)
                        {
                            Console.WriteLine(booking);
                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have not bookings yet.");
                    }

                }
            }

            void CancelBooking(PassengerDTO pDto)
            {
                ViewBookings(pDto);
                string? bookingID;
                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Please enter the booking ID you wish to cancel.");
                    bookingID = Console.ReadLine();
                } while (string.IsNullOrEmpty(bookingID));

                if (bc.GetBookingById(bookingID) != null)
                {
                    bc.DeleteBookingById(bookingID);
                    Console.WriteLine("Booking deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Booking not found.");
                }

            }

            void UpdateBooking(PassengerDTO pDto)
            {
                ViewBookings(pDto);
                string? bookingID;
                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Please enter the booking ID you wish to modify.");
                    bookingID = Console.ReadLine();
                } while (string.IsNullOrEmpty(bookingID));


                if (bc.GetBookingById(bookingID) != null)
                {
                    BookingDTO newDTO = EnterUpdateData(bc.GetBookingById(bookingID));
                    bc.UpdateBooking(newDTO);
                    Console.WriteLine("Booking updated successfully.");
                }
                else
                {
                    Console.WriteLine("Booking not found.");
                }


            }

            BookingDTO EnterUpdateData(BookingDTO dto)
            {
                bool exit = false;
                PassengerDTO? passenger = null;
                FlightDTO flight = dto.Flight;
                string? flightClass = null;
                do
                {


                    Console.WriteLine("***************************************");
                    Console.WriteLine("1. Modify passenger");
                    Console.WriteLine("2. Modify flight class");
                    Console.WriteLine("0. Finish");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            passenger = Login();
                            break;
                        case "2":
                            flightClass = SelectFlightClass(flight);
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }
                } while (!exit);

                BookingDTO bookingDTO = new BookingDTO(
                    dto.BookingId,
                    passenger ?? dto.Passenger,
                    dto.Flight,
                    flightClass ?? dto.BookingClass,
                    dto.BookingDate,
                    dto.BookingPrice
                );

                return bookingDTO;

            }


            void ManagerMenu()
            {

                bool exit = false;

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("1. Check flight validation details");
                    Console.WriteLine("2. Import flights");
                    Console.WriteLine("3. Search for bookings");
                    Console.WriteLine("0. Exit");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            CheckFlightValidations();
                            break;
                        case "2":
                            ImportFlights();
                            break;
                        case "3":
                            SearchForBookings();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }
                } while (!exit);

            }

            void CheckFlightValidations()
            {
                bc.ImportConstraints();
            }

            void ImportFlights()
            {
                string? filePath;

                Console.WriteLine("***************************************");
                Console.WriteLine("Please enter the file path of the flights file you wish to upload or press ENTER to cancel.");
                filePath = Console.ReadLine();

                if (filePath != null)
                {
                    bc.BatchFlightUpload(filePath);
                }


            }

            void SearchForBookings()
            {

                bool exit = false;

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("1. Search for booking by flight number");
                    Console.WriteLine("2. Search for booking by parameters");
                    Console.WriteLine("0. Exit");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            SearchForBookingsByNumber();
                            break;
                        case "2":
                            SearchForBookingsByParams();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }
                } while (!exit);


            }

            void SearchForBookingsByNumber()
            {

                string? flightID;
                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Please enter the booking ID.");
                    flightID = Console.ReadLine();
                } while (string.IsNullOrEmpty(flightID));

                List<BookingDTO> bookings = bc.GetBookingsByFlightNumber(flightID);
                if (bookings.Count > 0)
                {
                    foreach (BookingDTO booking in bookings)
                    {
                        Console.WriteLine(booking);
                    }
                }

                else
                {
                    Console.WriteLine("No bookings found.");
                }
            }

            void SearchForBookingsByParams()
            {
                var flightData = EnterFlightData();
                string? departure = flightData.departure;
                string? destination = flightData.destination;
                DateTime departureDate = flightData.departureDate;
                string? departureAirport = flightData.departureAirport;
                string? destinationAirport = flightData.destinationAirport;
                decimal minPrice = flightData.minPrice;
                decimal maxPrice = flightData.maxPrice;
                string? travelClass = flightData.travelClass;
                int passengerId = -1;

                Console.WriteLine("Enter Passenger Id (or press Enter to skip):");
                string? passengerInput = Console.ReadLine();
                while (!int.TryParse(passengerInput, out passengerId) && passengerInput != "")
                {
                    Console.WriteLine("Invalid price format. Please enter a valid number.");
                    passengerInput = Console.ReadLine();
                }

                if (passengerInput != "")
                    passengerId = int.Parse(passengerInput);

                List<BookingDTO> searchResults = bc.GetBookingsByParams(minPrice, maxPrice, departure, destination, departureDate, departureAirport, destinationAirport, travelClass);

                Console.WriteLine("***************************************");

                if (searchResults.Count == 0)
                    Console.WriteLine("No search Results:");
                else
                {
                    Console.WriteLine("Search Results:");
                    foreach (var booking in searchResults)
                    {
                        Console.WriteLine(booking);
                        Console.WriteLine("");
                    }
                }


                Console.WriteLine("***************************************");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }


            PassengerDTO? Login()
            {
                int id;
                string? userID;
                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Please passenger's personal ID");

                    userID = Console.ReadLine();
                } while (!int.TryParse(userID, out id));

                id = int.Parse(userID);
                PassengerDTO? pDto = pc.GetPassengerById(id);
                if (pDto == null)
                {
                    Console.WriteLine("Unregistered ID. Please, register");
                    pDto = Register(id);
                }
                else
                {
                    Console.WriteLine($"Welcome to {pDto.Name}'s passenger menu ");
                }

                return pDto;
            }

            PassengerDTO Register(int id)
            {
                string? name;
                string? email;

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Enter passenger's name");
                    name = Console.ReadLine();
                } while (string.IsNullOrEmpty(name));

                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Enter passenger's email");
                    email = Console.ReadLine();
                } while (string.IsNullOrEmpty(email));

                PassengerDTO passenger = pc.AddPassenger(id, name, email);
                return passenger;
            }



            string SelectFlightClass(FlightDTO fDto)
            {
                string? bookingClass = "";
                bool selectionOk = false;
                do
                {
                    Console.WriteLine("***************************************");
                    Console.WriteLine("***************************************");
                    Console.WriteLine("Select class");
                    if (fDto.EconomyPrice != 0)
                    {
                        Console.WriteLine("1.Economy");
                    }
                    else
                    {
                        Console.WriteLine("1.Economy (not avaiable)");
                    }
                    if (fDto.BusinessPrice != 0)
                    {
                        Console.WriteLine("2. Business");
                    }
                    else
                    {
                        Console.WriteLine("2.Business (not avaiable)");
                    }
                    if (fDto.FirstClassPrice != 0)
                    {
                        Console.WriteLine("3. First Class");
                    }
                    else
                    {
                        Console.WriteLine("3.First Class (not avaiable)");
                    }
                    Console.WriteLine("0. Exit");

                    string? userSelection = Console.ReadLine();

                    switch (userSelection)
                    {
                        case "1":
                            if (fDto.EconomyPrice != 0)
                            {
                                bookingClass = "Economy";
                                selectionOk = true;

                            }
                            else
                            {
                                Console.WriteLine("Economy class is not available.");
                            }
                            break;
                        case "2":
                            if (fDto.BusinessPrice != 0)
                            {
                                bookingClass = "Business";
                                selectionOk = true;
                            }
                            else
                            {
                                Console.WriteLine("Business class is not available.");
                            }
                            break;
                        case "3":
                            if (fDto.FirstClassPrice != 0)
                            {
                                bookingClass = "FirstClass";
                                selectionOk = true;
                            }
                            else
                            {
                                Console.WriteLine("First Class is not available.");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again");
                            break;
                    }
                } while (!selectionOk);
                return bookingClass;
            }

            (string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, decimal minPrice, decimal maxPrice, string? travelClass) EnterFlightData()
            {
                string? departure;
                string? destination;
                DateTime departureDate = DateTime.MinValue;
                string? departureAirport;
                string? destinationAirport;
                decimal minPrice = -1;
                decimal maxPrice = -1;
                string? travelClass;

                Console.WriteLine("***************************************");
                Console.WriteLine("***************************************");
                Console.WriteLine("Enter Departure Country (or press Enter to skip):");
                departure = Console.ReadLine();

                Console.WriteLine("Enter Destination Country (or press Enter to skip):");
                destination = Console.ReadLine();

                Console.WriteLine("Enter Departure Date (YYYY-MM-DD) (or press Enter to skip):");
                string? dateInput = Console.ReadLine();
                DateTime dateValue;
                while (!DateTime.TryParse(dateInput, out dateValue) && dateInput != "")
                {
                    Console.WriteLine("Invalid date format. Please enter date in YYYY-MM-DD format.");
                    dateInput = Console.ReadLine();
                }

                if (dateInput != "")
                    departureDate = dateValue;

                Console.WriteLine("Enter Departure Airport (or press Enter to skip):");
                departureAirport = Console.ReadLine();

                Console.WriteLine("Enter Destination Airport (or press Enter to skip):");
                destinationAirport = Console.ReadLine();

                Console.WriteLine("Enter Minimum Price (or press Enter to skip):");
                string? priceInput = Console.ReadLine();
                decimal priceValue;
                while (!decimal.TryParse(priceInput, out priceValue) && priceInput != "")
                {
                    Console.WriteLine("Invalid price format. Please enter a valid number.");
                    priceInput = Console.ReadLine();
                }

                if (priceInput != "")
                    minPrice = priceValue;

                Console.WriteLine("Enter Maximum Price (or press Enter to skip):");
                priceInput = Console.ReadLine();
                while (!decimal.TryParse(priceInput, out priceValue) && priceInput != "")
                {
                    Console.WriteLine("Invalid price format. Please enter a valid number.");
                    priceInput = Console.ReadLine();
                }

                if (priceInput != "")
                    maxPrice = priceValue;

                Console.WriteLine("Enter Travel Class (or press Enter to skip):");
                travelClass = Console.ReadLine();

                return (departure, destination, departureDate, departureAirport, destinationAirport, minPrice, maxPrice, travelClass);
            }
        }
    }
}
