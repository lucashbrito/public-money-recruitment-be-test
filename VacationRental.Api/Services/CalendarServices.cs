using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api.Services
{
    public class CalendarServices : ICalendarServices
    {
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IDictionary<int, Rental> _rentals;

        public CalendarServices(IDictionary<int, Booking> bookings, IDictionary<int, Rental> rentals)
        {
            _bookings = bookings;
            _rentals = rentals;
        }
        public Calendar Get(int rentalId, DateTime start, int nights)
        {
            var result = Calendar.Create(rentalId);

            for (var night = 0; night < nights; night++)
            {
                var date = CalendarDate.Create(start, night);

                date.AddCalendarBooking(rentalId, _bookings.Values);

                result.Dates.Add(date);
            }

            return result;
        }

        public Calendar GetWithPreparationTime(int rentalId, DateTime start, int nights)
        {
            var result = Calendar.Create(rentalId);
            var prepartionTimes = _rentals.Where(r => r.Value.Id == rentalId).Select(r => r.Value.PreprationTimeInDays);
            for (var night = 0; night < nights; night++)
            {
                var date = CalendarDate.Create(start, night);

                date.AddCalendarBookingWithPreparationDate(rentalId, _bookings.Values, prepartionTimes);

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
