using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain;
using VacationRental.Domain.DomainObjects;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IMapper _mapper;
        public CalendarController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<CalendarDto> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                return BadRequest("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                return NotFound();

            var result = Calendar.Create(rentalId);

            for (var night = 0; night < nights; night++)
            {
                var date = CalendarDate.Create(start, night);

                date.AddCalendarBooking(rentalId, _bookings.Values);

                result.Dates.Add(date);
            }           

            return Ok(_mapper.Map<CalendarDto>(result));
        }
    }
}
