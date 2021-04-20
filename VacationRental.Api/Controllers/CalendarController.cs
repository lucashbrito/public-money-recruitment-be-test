using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
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
        private readonly ICalendarServices _calendarService;
        private readonly IMapper _mapper;

        public CalendarController(IDictionary<int, Rental> rentals, IMapper mapper, ICalendarServices calendarService)
        {
            _rentals = rentals;
            _calendarService = calendarService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<CalendarDto> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                return BadRequest("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                return NotFound();

            var result = _calendarService.Get(rentalId, start, nights);

            return Ok(_mapper.Map<CalendarDto>(result));
        }
    }
}
