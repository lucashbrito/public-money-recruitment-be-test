using System;
using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain
{
    public class Rental : Entity
    {
        public int Units { get; protected set; }
        public int PreprationTimeInDays { get; set; }

        protected Rental() { }

        private Rental(int id, int units)
        {
            Id = id;
            Units = units;
        }

        private Rental(int id, int units, int preprationTimeInDays)
        {
            Id = id;
            Units = units;
            PreprationTimeInDays = preprationTimeInDays;
        }


        public static Rental Create(int id, int units)
        {
            return new Rental(id, units);
        }

        public static Rental Create(int id, int units, int preparationTimeInDays)
        {
            return new Rental(id, units, preparationTimeInDays);
        }

        public void IsRentalAvailable(int count)
        {
            if (count >= Units)
            {
                throw new DomainException("Rental is not available");
            }
        }

        public void SetPreprationTimeInDays(int preprationTimeInDays)
        {
            if (PreprationTimeInDays > preprationTimeInDays)
                throw new DomainException("you can't increasing  the number of units");
        }

        public void SetUnit(int units)
        {
            if (Units < units)
                throw new DomainException("you can't decreasing the number of units");
        }
    }
}
