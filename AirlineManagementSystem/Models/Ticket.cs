using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagementSystem.Models{

    public class Ticket
    {
        public int TicketID { get; set; }

        public int FlightID { get; set; }

        public int PersonID { get; set; }

        public int SeatID { get; set; }
  
        // Navigation properties
        public Flight Flight { get; set; }
        public List<Baggage> baggages{ get; set; }
        public Person Person { get; set; }
        public Seat Seat { get; set; }
    }

    
}