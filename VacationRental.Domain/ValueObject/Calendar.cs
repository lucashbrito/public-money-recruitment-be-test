using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain.ValueObject
{
    public class Calendar : ValueObject<Calendar>
    {
        public int RentalId { get; private set; }
        public List<CalendarDate> Dates { get; private set; }        

        protected Calendar() { }

        public static Calendar Create(int rentalId)
        {
            return new Calendar
            {
                RentalId = rentalId,
                Dates = new List<CalendarDate>()
            };
        }
    }
}
