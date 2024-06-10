using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        private string _filePath;
        private List<Passenger> passengers;

        public PassengerRepository(string? filePath = null)
        {
            passengers = new List<Passenger>();
            _filePath = filePath ?? @"..\..\..\Data\PeopleData.txt";

        }

        public void BatchPassengerUpload() // Uploads file with passengers info when system is initiated
        {
            PassengerUpload(_filePath);
        }

        private void PassengerUpload(string fp)  
        {

            try
            {
                using (StreamReader reader = new StreamReader(fp))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        string[] parts = line.Split(',');
                        if (parts.Length >= 3)
                        {

                            Passenger passenger = new Passenger
                            {
                                Id = int.Parse(parts[0]),
                                Name = parts[1],
                                Email = parts[2]
                            };


                            passengers.Add(passenger);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

        }

        public Passenger? GetPassengerById(int passengerId)
        {
            return passengers.FirstOrDefault(passenger => passenger.Id == passengerId);
        }
        public void AddPassenger(Passenger passenger)
        {
            passengers.Add(passenger);
            string line = $"{passenger.Id},{passenger.Name},{passenger.Email}\n";
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(line);
            }
        }

        public List<Passenger> GetAllPassengers() 
        {
            return passengers;
        }



    }
}