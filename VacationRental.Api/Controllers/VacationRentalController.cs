using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Domain;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/vacationrental")]
    [ApiController]
    public class VacationRentalController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IMapper _mapper;
        private readonly IRentalServices _rentalServices;
        private readonly ICalendarServices _calendarService;

        public VacationRentalController(IDictionary<int, Rental> rentals,
            IMapper mapper,
            IRentalServices rentalServices,
            ICalendarServices calendarService)
        {
            _rentalServices = rentalServices;
            _rentals = rentals;
            _mapper = mapper;
            _calendarService = calendarService;
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

            _rentalServices.Update(id, vacationRental);

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
            
            var result = _calendarService.GetWithPreparationTime(rentalId, start, nights);

            return Ok(_mapper.Map<CalendarDto>(result));
        }    
    }
}
