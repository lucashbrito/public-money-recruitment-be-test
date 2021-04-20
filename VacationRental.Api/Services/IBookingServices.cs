using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingServices
    {
        ResourceIdViewModel AddNewBooking(BookingBindingModel newBooking);
    }
}
