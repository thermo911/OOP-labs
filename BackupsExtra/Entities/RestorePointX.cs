using System;
using System.Collections.Generic;
using Backups.Entities;
using Newtonsoft.Json;

namespace BackupsExtra.Entities
{
    public class RestorePointX : IEquatable<RestorePointX>, IComparable<RestorePointX>
    {
        private readonly HashSet<JobObject> _storedJobObjects;

        public RestorePointX(DateTime creationDateTime, HashSet<JobObject> storedJobObjects)
        {
            Id = Guid.NewGuid();
            CreationDateTime = creationDateTime;
            _storedJobObjects = storedJobObjects ??
                                throw new ArgumentNullException(nameof(storedJobObjects));
        }

        [JsonConstructor]
        private RestorePointX(Guid id, DateTime creationDateTime, HashSet<JobObject> storedJobObjects)
        {
            Id = id;
            CreationDateTime = creationDateTime;
            _storedJobObjects = storedJobObjects ??
                                throw new ArgumentNullException(nameof(storedJobObjects));
        }

        public Guid Id { get; }
        public DateTime CreationDateTime { get; }
        public IReadOnlyCollection<JobObject> StoredJobObjects => _storedJobObjects;

        public void Merge(RestorePointX point)
            => _storedJobObjects.UnionWith(point._storedJobObjects);

        public bool Equals(RestorePointX other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RestorePointX)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public int CompareTo(RestorePointX other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return CreationDateTime.CompareTo(other.CreationDateTime);
        }

        public override string ToString() => $"{GetType().Name}(id = {Id})";
    }
}