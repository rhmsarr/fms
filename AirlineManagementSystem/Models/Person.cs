using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagementSystem.Models{
  
    public class Person
    {
        public int PersonID { get; set; }

        public  string Name { get; set; }

        public  string Surname { get; set; }

        public  ICollection<Passenger> Passengers { get; set; }
        public  ICollection<FlightCoordinator> FlightCoordinators { get; set; }
    }
}
