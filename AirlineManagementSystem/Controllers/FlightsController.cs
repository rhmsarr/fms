using Microsoft.AspNetCore.Mvc;
using AirlineManagementSystem.Models;
using Npgsql;
using AirlineManagementSystem.ViewModels;
using System.Collections.ObjectModel;

namespace AirlineManagementSystem.Controllers{
    public class FlightsController : Controller{
        private readonly string _connectionString;

        public FlightsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Database");
        }

        [HttpPost]
        public IActionResult Index(SelectFlight flight) {
        
            if(!ModelState.IsValid) {
                return Redirect("/Home/Index");
            }
            List<Flight> flights = new List<Flight>();
            
            string query = @$"
            SELECT 
                ""DepartureAirport"".""CountryName"" AS ""DepartureCountryName"",
                ""DepartureAirport"".""CityName"" AS ""DepartureCityName"",
                ""DepartureAirport"".""AirportName""AS ""DepartureAirportName"",
                ""ArrivalAirport"".""CountryName"" AS ""ArrivalCountryName"",
                ""ArrivalAirport"".""CityName"" AS ""ArrivalCityName"",
                ""ArrivalAirport"".""AirportName"" AS ""ArrivalAirportName"",
                ""Flight"".""DepartureTime"",
                ""Flight"".""ArrivalTime"",
                ""Flight"".""Price"",
                ""Airline"".""AirlineName"",
                ""Flight"".""FlightID""
            FROM ""Flight""
            INNER JOIN ""Plane"" ON ""Flight"".""PlaneID"" = ""Plane"".""PlaneID""
            INNER JOIN ""Airline"" ON ""Airline"".""AirlineID"" = ""Plane"".""AirlineID""
            INNER JOIN ""Route"" ON ""Flight"".""RouteID"" = ""Route"".""RouteID""
            INNER JOIN ""Airport"" AS ""DepartureAirport"" ON ""Route"".""DeparturePlace"" = ""DepartureAirport"".""AirportID""
            INNER JOIN ""Airport"" AS ""ArrivalAirport"" ON ""Route"".""ArrivalPlace"" = ""ArrivalAirport"".""AirportID""
            WHERE ""Flight"".""DepartureTime""::date = '{flight.Date.ToString("yyyy-MM-dd")}'
            and  ""DepartureAirport"".""CountryName"" = '{flight.DepartureCountry}'
            and ""ArrivalAirport"".""CountryName"" = '{flight.ArrivalCountry}';
            ";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flights.Add(new Flight
                        {
                            DepartureCountry = reader.GetString(reader.GetOrdinal("DepartureCountryName")),
                            DepartureCity = reader.GetString(reader.GetOrdinal("DepartureCityName")),
                            DepartureAirport = reader.GetString(reader.GetOrdinal("DepartureAirportName")),
                            ArrivalCountry = reader.GetString(reader.GetOrdinal("ArrivalCountryName")),
                            ArrivalCity = reader.GetString(reader.GetOrdinal("ArrivalCityName")),
                            ArrivalAirport = reader.GetString(reader.GetOrdinal("ArrivalAirportName")),
                            DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                            ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            AirlineName = reader.GetString(reader.GetOrdinal("AirlineName")),
                            FlightID = reader.GetInt32(reader.GetOrdinal("FlightID"))
                        });
                    }
                }
            }
        }

        

            return View(flights);
        }   
        
        [HttpGet]
        public IActionResult Checkout(int FlightID) {

            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Ticket model) {

            return View();
        }
        public IActionResult LastCheckout(){
            return View();
        }
    
        public IActionResult CreateFlight(){
            CreateFlight flight = new CreateFlight();
            string query = $@"SELECT * FROM ""Airport""";
            using (var connection = new NpgsqlConnection(_connectionString))
            {

                using (var command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flight.airports.Add(new Airport{
                            AirportName = reader.GetString(reader.GetOrdinal("AirportName")),
                            CityName = reader.GetString(reader.GetOrdinal("CountryName")),
                            CountryName = reader.GetString(reader.GetOrdinal("CityName")),
                            AirportId = reader.GetInt32(reader.GetOrdinal("AirportID"))
                        });   
                        }
                    }
                }
            
            query = $@"SELECT * FROM ""Airline""";
            
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flight.airlines.Add(new Airline{
                                AirlineName = reader.GetString(reader.GetOrdinal("AirlineName")),
                                AirlineId = reader.GetInt32(reader.GetOrdinal("AirlineID"))
                            });   
                        }
                    }
                }
            }
            return View(flight);
        }

        [HttpPost]
        public IActionResult CreateFlight(CreateFlight flight){
            
            string query = $@"SELECT ""RouteID"" FROM ""Route"" 
                WHERE ""Route"".""DeparturePlace"" = '{flight.DepartureAirport}' 
                AND ""Route"".""ArrivalPlace"" = '{flight.ArrivalAirport}'";
            int RouteID = 0;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RouteID = reader.GetInt32(reader.GetOrdinal("RouteID"));   
                        }
                    }
                }
                
            
            if (RouteID == 0)
            {
                query = @"
                    INSERT INTO ""Route"" (""DeparturePlace"", ""ArrivalPlace"") 
                    VALUES (@DeparturePlace, @ArrivalPlace) 
                    RETURNING ""RouteID""";
                
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Add parameters for string values to avoid SQL injection and formatting issues
                    command.Parameters.AddWithValue("@DeparturePlace", Convert.ToInt32(flight.DepartureAirport));
                    command.Parameters.AddWithValue("@ArrivalPlace", Convert.ToInt32(flight.ArrivalAirport));

                    // Execute the query and retrieve the returned RouteID
                    var output = command.ExecuteScalar();
                    RouteID = Convert.ToInt32(output);
                }
            }
            
            
            
            
            query = @"SELECT ""PlaneID"" FROM ""Plane"" WHERE ""AirlineID"" = @AirlineID;";
            using(var command = new NpgsqlCommand(query, connection)){
                command.Parameters.AddWithValue("@AirlineID",flight.AirlineID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flight.PlaneID = reader.GetInt32(reader.GetOrdinal("PlaneID"));   
                    }
                }
            }
                
            
            query = @"INSERT INTO ""Flight"" (""AirlineID"", ""ArrivalTime"", ""DepartureTime"", ""Price"", ""RouteID"",""PlaneID"")
                 VALUES (@AirlineID, @ArrivalTime, @DepartureTime, @Price, @RouteID, @PlaneID)";

            using (var command = new NpgsqlCommand(query, connection))
            {
                // Add parameters with appropriate types
                command.Parameters.AddWithValue("@AirlineID", flight.AirlineID);
                command.Parameters.AddWithValue("@ArrivalTime", flight.ArrivalTime); 
                command.Parameters.AddWithValue("@DepartureTime", flight.DepartureTime); 
                command.Parameters.AddWithValue("@Price", flight.Price);
                command.Parameters.AddWithValue("@RouteID", RouteID);
                command.Parameters.AddWithValue("@PlaneID", flight.PlaneID);

                command.ExecuteNonQuery();
            }
            }
            return View();
        }
        public IActionResult AllFlights() {
        
            
            List<Flight> flights = new List<Flight>();
            
            string query = @$"
            SELECT 
                ""DepartureAirport"".""CountryName"" AS ""DepartureCountryName"",
                ""DepartureAirport"".""CityName"" AS ""DepartureCityName"",
                ""DepartureAirport"".""AirportName""AS ""DepartureAirportName"",
                ""ArrivalAirport"".""CountryName"" AS ""ArrivalCountryName"",
                ""ArrivalAirport"".""CityName"" AS ""ArrivalCityName"",
                ""ArrivalAirport"".""AirportName"" AS ""ArrivalAirportName"",
                ""Flight"".""DepartureTime"",
                ""Flight"".""ArrivalTime"",
                ""Flight"".""Price"",
                ""Airline"".""AirlineName"",
                ""Flight"".""FlightID""
            FROM ""Flight""
            INNER JOIN ""Plane"" ON ""Flight"".""PlaneID"" = ""Plane"".""PlaneID""
            INNER JOIN ""Airline"" ON ""Airline"".""AirlineID"" = ""Plane"".""AirlineID""
            INNER JOIN ""Route"" ON ""Flight"".""RouteID"" = ""Route"".""RouteID""
            INNER JOIN ""Airport"" AS ""DepartureAirport"" ON ""Route"".""DeparturePlace"" = ""DepartureAirport"".""AirportID""
            INNER JOIN ""Airport"" AS ""ArrivalAirport"" ON ""Route"".""ArrivalPlace"" = ""ArrivalAirport"".""AirportID""
            order by ""FlightID"";";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flights.Add(new Flight
                        {
                            DepartureCountry = reader.GetString(reader.GetOrdinal("DepartureCountryName")),
                            DepartureCity = reader.GetString(reader.GetOrdinal("DepartureCityName")),
                            DepartureAirport = reader.GetString(reader.GetOrdinal("DepartureAirportName")),
                            ArrivalCountry = reader.GetString(reader.GetOrdinal("ArrivalCountryName")),
                            ArrivalCity = reader.GetString(reader.GetOrdinal("ArrivalCityName")),
                            ArrivalAirport = reader.GetString(reader.GetOrdinal("ArrivalAirportName")),
                            DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                            ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            AirlineName = reader.GetString(reader.GetOrdinal("AirlineName")),
                            FlightID = reader.GetInt32(reader.GetOrdinal("FlightID"))
                        });
                    }
                }
            }
        }

        

            return View(flights);
         }
        [HttpGet]
        public IActionResult UpdateFlight(int id){
            string query = @"SELECT
                    ""DepartureAirport"".""AirportName"" AS ""DepartureAirportName"",
                    ""ArrivalAirport"".""AirportName"" AS ""ArrivalAirportName"", 
                    ""Flight"".""DepartureTime"",
                    ""Flight"".""ArrivalTime"",
                    ""Flight"".""Price"",
                    ""Airline"".""AirlineID"" from ""Flight"" INNER JOIN 
                    ""Plane"" ON ""Flight"".""PlaneID"" = ""Plane"".""PlaneID""
                    INNER JOIN ""Airline"" ON ""Airline"".""AirlineID"" = ""Plane"".""AirlineID""
                    INNER JOIN ""Route"" ON ""Flight"".""RouteID"" = ""Route"".""RouteID""
                    INNER JOIN  ""Airport"" AS ""DepartureAirport"" ON ""Route"".""DeparturePlace"" = ""DepartureAirport"".""AirportID""
                    INNER JOIN  ""Airport"" AS ""ArrivalAirport"" ON ""Route"".""ArrivalPlace"" = ""ArrivalAirport"".""AirportID""
                    WHERE ""Flight"".""FlightID"" = @FlightID
                    ";

            CreateFlight flight = new CreateFlight();
            flight.FlightID = id;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                        
                                flight.DepartureAirport = reader.GetString(reader.GetOrdinal("DepartureAirportName"));
                                flight.ArrivalAirport = reader.GetString(reader.GetOrdinal("ArrivalAirportName"));
                                flight.DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime"));
                                flight.ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime"));
                                flight.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                                flight.AirlineID = reader.GetInt32(reader.GetOrdinal("AirlineID"));
                                
                            
                        }

                    }

                    
                }
                query = $@"SELECT * FROM ""Airport""";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flight.airports.Add(new Airport{
                            AirportName = reader.GetString(reader.GetOrdinal("AirportName")),
                            CityName = reader.GetString(reader.GetOrdinal("CountryName")),
                            CountryName = reader.GetString(reader.GetOrdinal("CityName")),
                            AirportId = reader.GetInt32(reader.GetOrdinal("AirportID"))
                        });   
                        }
                    }
                }
            
            query = $@"SELECT * FROM ""Airline""";
            
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flight.airlines.Add(new Airline{
                                AirlineName = reader.GetString(reader.GetOrdinal("AirlineName")),
                                AirlineId = reader.GetInt32(reader.GetOrdinal("AirlineID"))
                            });   
                        }
                    }
                }
                
            }

                            
            return View(flight);
        }
        [HttpPost]
        public IActionResult UpdateFlight(CreateFlight flight){
             
            string query = @"SELECT ""PlaneID"" FROM ""Plane"" WHERE ""AirlineID"" = @AirlineID;";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using(var command = new NpgsqlCommand(query, connection)){
                    command.Parameters.AddWithValue("@AirlineID",flight.AirlineID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flight.PlaneID = reader.GetInt32(reader.GetOrdinal("PlaneID"));   
                        }
                    }
                }
            

            query = @"UPDATE ""Flight"" SET ""AirlineID"" = @AirlineID, 
                ""ArrivalTime"" = @ArrivalTime, 
                ""DepartureTime"" = @DepartureTime, 
                ""PlaneID"" = @PlaneID,
                ""Price"" = @Price
                WHERE ""FlightID"" = @flightID";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirlineID", flight.AirlineID);
                    command.Parameters.AddWithValue("@ArrivalTime", flight.ArrivalTime); 
                    command.Parameters.AddWithValue("@DepartureTime", flight.DepartureTime); 
                    command.Parameters.AddWithValue("@Price", flight.Price);
                    command.Parameters.AddWithValue("@PlaneID", flight.PlaneID);
                    command.Parameters.AddWithValue("@flightID", flight.FlightID);

                    command.ExecuteNonQuery();
                }
            }  
            return RedirectToAction("AllFlights");
        }
        
        public IActionResult Delete(int id){
            string query = @"DELETE FROM ""Flight"" WHERE ""FlightID"" = @flightID";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@flightID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("AllFlights");
       
        }



        
    }
}