﻿namespace Bookify.Domain.Abstractions
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        protected Entity(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; init; }

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }
        public IReadOnlyList<IDomainEvent> GetDomainEvent()
        {
            return _domainEvents.ToList();
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
