namespace AirlineManagementSystem.Models{
    public class Plane{
        public int PlaneID {get; set;}
        public string PlaneType {get; set;}
        public int AirlineID {get; set;}
        public Airline PlaneAirline {get; set;}
    }
}