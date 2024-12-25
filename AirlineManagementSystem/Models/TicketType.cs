using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace AirlineManagementSystem.Models{
   public class TicketType
{
    public int TicketTypeID { get; set; }

    public  string TypeName { get; set; }
}

}