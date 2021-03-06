using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IMapper _mapper;
        private readonly IBookingServices _bookService;

        public BookingsController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings,
            IBookingServices bookingService,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _mapper = mapper;
            _bookService = bookingService;
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

            var key = _bookService.AddNewBooking(newBooking);

            return Ok(key);
        }
    }
}
