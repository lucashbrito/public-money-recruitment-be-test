using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Domain.DomainObjects
{
    public abstract class Entity : IEquatable<Entity>
    {
        public int Id { get; protected set; }

        public bool Equals(Entity obj)
        {
            var other = obj as Entity;

            return Equals(other);
        }
    }
}
