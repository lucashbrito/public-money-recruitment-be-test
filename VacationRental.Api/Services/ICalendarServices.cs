using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api.Services
{
    public interface ICalendarServices
    {
        Calendar Get(int rentalId, DateTime start, int nights);
        Calendar GetWithPreparationTime(int rentalId, DateTime start, int nights);
    }
}
