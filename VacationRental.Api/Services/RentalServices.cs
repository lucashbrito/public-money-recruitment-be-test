using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Domain;

namespace VacationRental.Api.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IDictionary<int, Rental> _rentals;
        public RentalServices(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        public void Update(int id, VacationRentalBindingModel vacationRental)
        {
            foreach (var rental in _rentals.Where(r => r.Value.Id == id).Select(r => r.Value))
            {
                rental.SetUnit(vacationRental.Units);
                rental.SetPreprationTimeInDays(vacationRental.PreparationTimeInDays);
            }
        }
    }
}
