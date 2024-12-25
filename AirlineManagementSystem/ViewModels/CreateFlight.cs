using AirlineManagementSystem.Models;
namespace AirlineManagementSystem.ViewModels{
    public class CreateFlight : Flight{
        public List<Airport> airports { get; set; } = new List<Airport>();
        public List<Airline> airlines{ get; set; } = new List<Airline>();
    }
}