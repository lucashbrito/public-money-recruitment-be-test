using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Domain;

namespace VacationRental.Api.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;

        public BookingServices(IDictionary<int, Rental> rentals, IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public ResourceIdViewModel AddNewBooking(BookingBindingModel newBooking)
        {
            var reservationForThatTime = 0;
            foreach (var booking in _bookings.Values)
            {
                if (booking.IsRentalEqual(newBooking.RentalId)
                    && (booking.IsNewStartDayAfterPrepartionTime(newBooking.Start.Date, _rentals[booking.RentalId].PreprationTimeInDays)
                        && booking.IsEndDayGreaterThanNewStartDay(newBooking.Start.Date))

                    || (booking.IsNewEndDayGreaterThanStartDay(newBooking.Start, newBooking.Nights, _rentals[booking.RentalId].PreprationTimeInDays)
                        && booking.IsEndDayGreatOrEqualNewEndDay(newBooking.Start, newBooking.Nights))

                    || (booking.IsStartDayGreaterThanNewStartDay(newBooking.Start, _rentals[booking.RentalId].PreprationTimeInDays)
                        && booking.IsNewEndDayGreaterThanEndDay(newBooking.Start, newBooking.Nights)))
                {
                    reservationForThatTime++;
                }
            }
            _rentals[newBooking.RentalId].IsRentalAvailable(reservationForThatTime);

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, Booking.Create(key.Id, newBooking.RentalId, newBooking.Start.Date, newBooking.Nights));
            return key;
        }

    }
}
