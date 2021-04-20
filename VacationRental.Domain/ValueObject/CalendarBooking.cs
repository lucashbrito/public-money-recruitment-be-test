using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain.ValueObject
{
    public class CalendarBooking : ValueObject<CalendarBooking>
    {
        public int Id { get; private set; }

        public int Units { get; private set; }


        protected CalendarBooking(int id)
        {
            Id = id;
        }

        protected CalendarBooking(int id, int units)
        {
            Id = id;
            Units = units;
        }

        public static CalendarBooking Create(int id)
        {
            return new CalendarBooking(id);
        }
        public static CalendarBooking Create(int id, int units)
        {
            return new CalendarBooking(id, units);
        }
    }
}
