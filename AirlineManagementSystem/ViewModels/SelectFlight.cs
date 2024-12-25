using System.ComponentModel.DataAnnotations;

namespace AirlineManagementSystem.ViewModels{
    public class SelectFlight{
        [Required]
        public string DepartureCountry { get; set; } = "";
        [Required]
        public string ArrivalCountry { get; set; } = "";
        [Required]
        public DateTime Date { get; set; }
    }
}