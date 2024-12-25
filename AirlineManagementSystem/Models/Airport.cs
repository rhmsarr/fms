namespace AirlineManagementSystem.Models{
    public class Airport{
        public int AirportId { get; set; }
        public string AirportName { get; set;} = string.Empty;
        public string CityName { get; set;} = string.Empty;
        public string CountryName { get; set;} = string.Empty;
    }
}