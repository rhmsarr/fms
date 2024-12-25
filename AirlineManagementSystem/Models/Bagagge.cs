namespace AirlineManagementSystem.Models{
    public class Baggage{
        public int BaggageId { get; set;}
        public int BaggageTypeID { get; set;}
        public int BaggageWeight { get; set;}
        public bool Extra { get; set;}
        public int TicketID { get; set;}
    }
}