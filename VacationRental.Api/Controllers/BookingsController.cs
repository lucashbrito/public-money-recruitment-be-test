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
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                return BadRequest("Nigts must be positive");
            if (!_rentals.ContainsKey(model.RentalId))
                return NotFound();

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.IsRentalEqual(model.RentalId)
                        && (booking.IsStartDayEqualOrDayAfterThanNewStart(model.Start.Date) && booking.IsEndDayGreaterThanNewStartDay(model.Start.Date))
                        || (booking.IsNewEndDayGreaterThanStartDay(model.Start, model.Nights) && booking.IsEndDayGreatOrEqualNewEndDay(model.Start, model.Nights))
                        || (booking.IsStartDayGreaterThanNewStartDay(model.Start) && booking.IsNewEndDayGreaterThanEndDay(model.Start, model.Nights)))
                    {
                        count++;
                    }
                }

                _rentals[model.RentalId].IsRentalAvailable(count);
            }

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, Booking.Create(key.Id, model.RentalId, model.Start.Date, model.Nights));

            return Ok(key);
        }
    }
}
