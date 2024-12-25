using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AirlineManagementSystem.Models
{
    public class Terminal
    {
        public int TerminalID { get; set; }

        public int AirportID { get; set; }

        public string GateNumber { get; set; }

        public Airport Airport { get; set; }
    }
}