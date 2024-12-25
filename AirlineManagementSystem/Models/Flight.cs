namespace AirlineManagementSystem.Models{
    public class Flight{
        public int FlightID {get; set;}
        public int FlightCoordinatorID {get; set;}
        public int PlaneID {get; set;}
        public int RouteID {get; set;}
        public int TerminalID {get; set;}
        public int AirlineID {get; set;}    
        public bool Retard {get; set;}

        public DateTime ArrivalTime {get; set;}
        public DateTime DepartureTime {get; set;}
        public decimal Price {get; set;}
        public string DepartureAirport {get; set;}
        public string DepartureCity {get; set;}
        public string DepartureCountry {get; set;}
        public string ArrivalAirport {get; set;}
        public string ArrivalCity {get; set;}
        public string ArrivalCountry {get; set;}
        public string AirlineName {get; set;}
        
        public Route FlightRoute {get; set;}
        public Plane FlightPlane {get; set;}
    }
}