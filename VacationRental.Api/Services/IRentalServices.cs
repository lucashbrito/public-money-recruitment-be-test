using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalServices
    {
        void Update(int id, VacationRentalBindingModel vacationRental);
    }
}
