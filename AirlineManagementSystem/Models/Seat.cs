using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagementSystem.Models
{
    public class Seat
    {
        public int SeatID { get; set; }

        public int PlaneID { get; set; }

        public int TicketTypeID { get; set; }

        public Plane Plane { get; set; }
        public required TicketType TicketType { get; set; }
    }
}