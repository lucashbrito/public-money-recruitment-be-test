using VacationRental.Domain.DomainObjects;

namespace VacationRental.Domain.ValueObject
{
    public class PreparationTime : ValueObject<PreparationTime>
    {
        public int Unit { get; private set; }

        public PreparationTime(int unit)
        {
            Unit = unit;
        }

        public static PreparationTime Create(int unit)
        {
            return new PreparationTime(unit) { Unit = unit };
        }
    }
}