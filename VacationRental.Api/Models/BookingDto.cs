using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Api.Models
{
    public class BookingDto
    {
        public int RentalId { get;  set; }
        public DateTime Start { get;  set; }
        public int Nights { get;  set; }
        public int Unit { get;  set; }
    }
}
