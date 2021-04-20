using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Domain;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/vacationrental")]
    [ApiController]
    public class VacationRentalController : ControllerBase
    {

        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IMapper _mapper;
        public VacationRentalController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings,
            IMapper mapper)
        {
            _rentals = rentals;
            _bookings = bookings;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Rentals(VacationRentalBindingModel rentalBinding)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, Rental.Create(key.Id, rentalBinding.Units, rentalBinding.PreparationTimeInDays));

            return Ok(key);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Rentals(int id, [FromBody] VacationRentalBindingModel vacationRental)
        {
            if (!_rentals.ContainsKey(id))
                return NotFound();

            foreach (var rental in _rentals.Where(r => r.Value.Id == id).Select(r => r.Value))
            {
                rental.SetUnit(vacationRental.Units);
                rental.SetPreprationTimeInDays(vacationRental.PreparationTimeInDays);
            }

            return NoContent();
        }


        [HttpGet]
        [Route("calendar")]
        public ActionResult<CalendarDto> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                return BadRequest("Nights must be positive");
            if (!_rentals.ContainsKey(rentalId))
                return NotFound();

            var result = Calendar.Create(rentalId);
            var prepartionTimes = _rentals.Where(r => r.Value.Id == rentalId).Select(r => r.Value.PreprationTimeInDays);
            for (var night = 0; night < nights; night++)
            {
                var date = CalendarDate.Create(start, night);

                date.AddCalendarBookingWithPreparationDate(rentalId, _bookings.Values, prepartionTimes);

                result.Dates.Add(date);
            }

            return Ok(_mapper.Map<CalendarDto>(result));
        }
    }
}
