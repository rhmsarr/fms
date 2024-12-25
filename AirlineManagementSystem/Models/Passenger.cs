using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagementSystem.Models
{
    public class Passenger : Person
    {
        public int PersonID { get; set; }

        public  string Email { get; set; }

        public  string PhoneNumber { get; set; }

        public int CountryCodeID { get; set; }

        public CountryCode CountryCode { get; set; }
    }
}