using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IMapper _mapper;

        public BookingsController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public ActionResult<BookingDto> Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                return NotFound();

            return Ok(_mapper.Map<Booking, BookingDto>(_bookings[bookingId]));
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel newBooking)
        {
            if (newBooking.Nights <= 0)
                return BadRequest("Nigts must be positive");
            if (!_rentals.ContainsKey(newBooking.RentalId))
                return NotFound();            

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

            return Ok(key);
        }
    }
}
