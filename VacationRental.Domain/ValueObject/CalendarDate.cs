using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain.ValueObject
{
    public class CalendarDate : ValueObject<CalendarDate>
    {
        public DateTime Date { get; private set; }
        public List<CalendarBooking> Bookings { get; private set; }
        public List<PreparationTime> PreparationTimes { get; private set; }

        protected CalendarDate() { }


        public void AddCalendarBooking(int rentalId, ICollection<Domain.Booking> bookings)
        {
            foreach (var booking in bookings.Where(b => b.IsRentalEqual(rentalId)))
            {
                if (booking.IsStartDayEqualOrDayAfterThanNewStart(Date)
                    && booking.IsEndDayGreaterThanNewStartDay(Date))
                {
                    Bookings.Add(CalendarBooking.Create(booking.Id));
                }
            }
        }

        public void AddCalendarBookingWithPreparationDate(int rentalId, ICollection<Domain.Booking> bookings, IEnumerable<int> preparationTimes)
        {
            foreach (var booking in bookings.Where(b => b.IsRentalEqual(rentalId)))
            {
                if (booking.IsStartDayEqualOrDayAfterThanNewStart(Date)
                    && booking.IsEndDayGreaterThanNewStartDay(Date))
                {
                    PreparationTimes.AddRange(from preparationTime in preparationTimes select PreparationTime.Create(preparationTime));
                    Bookings.Add(CalendarBooking.Create(booking.Id, booking.Unit));
                }
            }
        }

        public static CalendarDate Create(DateTime start, int night)
        {
            return new CalendarDate
            {
                Date = start.Date.AddDays(night),
                Bookings = new List<CalendarBooking>(),
                PreparationTimes = new List<PreparationTime>(),
            };
        }
    }
}
