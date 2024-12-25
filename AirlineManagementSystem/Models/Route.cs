using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagementSystem.Models{

  public class Route
    {
        public int RouteID { get; set; }

        public int ArrivalPlaceID { get; set; }

        public int DeparturePlaceID { get; set; }

        public int AirportID { get; set; }

        public Airport DepartureAirport { get; set; }
        public Airport ArrivalAirport {get; set;}
    }
}