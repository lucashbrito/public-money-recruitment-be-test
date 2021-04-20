using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain
{
    public class Booking : Entity
    {
        public int RentalId { get; protected set; }
        public DateTime Start { get; protected set; }
        public int Nights { get; protected set; }
        public int Unit { get; protected set; }

        protected Booking() { }

        private Booking(int id, int rentalId, DateTime start, int nights, int unit)
        {
            Id = id;
            RentalId = rentalId;
            Start = start;
            Nights = nights;
            Unit = unit;
        }

        private Booking(int id, int rentalId, DateTime start, int nights)
        {
            Id = id;
            RentalId = rentalId;
            Start = start;
            Nights = nights;
        }

        public void SetUnit(int unit)
        {
            Unit = unit;
        }

        public static Booking Create(int id, int rentalId, DateTime start, int nights, int unit)
        {
            return new Booking(id, rentalId, start, nights, unit);
        }

        public static Booking Create(int id, int rentalId, DateTime start, int nights)
        {
            return new Booking(id, rentalId, start, nights);
        }

        public bool IsRentalEqual(int id)
        {
            return RentalId == id;
        }

        public bool IsStartDayEqualOrDayAfterThanNewStart(DateTime date)
        {
            return Start <= date;
        }

        public bool IsEndDayGreaterThanNewStartDay(DateTime newDate)
        {
            return Start.AddDays(Nights) > newDate;
        }

        public bool IsNewEndDayGreaterThanStartDay(DateTime newData, int nights)
        {
            var endDate = newData.AddDays(nights);
            return Start < endDate;
        }

        public bool IsEndDayGreatOrEqualNewEndDay(DateTime newData, int nights)
        {
            var endDate = newData.AddDays(nights);

            return Start.AddDays(Nights) >= endDate;
        }

        public bool IsStartDayGreaterThanNewStartDay(DateTime newDate)
        {
            return Start > newDate;
        }

        public bool IsNewEndDayGreaterThanEndDay(DateTime newDate, int nights)
        {
            return Start.AddDays(Nights) < newDate.AddDays(nights);
        }
    }
}
