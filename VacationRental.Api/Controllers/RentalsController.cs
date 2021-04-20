using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IMapper _mapper;
        public RentalsController(IDictionary<int, Rental> rentals, IMapper mapper)
        {
            _rentals = rentals;
            _mapper = mapper; ;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public ActionResult<RentalDto> Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                return NotFound();

            return Ok(_mapper.Map<RentalDto>(_rentals[rentalId]));
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, Rental.Create(key.Id, model.Units));

            return Ok(key);
        }
    }
}
