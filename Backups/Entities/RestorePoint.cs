using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private List<JobObject> _storedJobObjects;

        public RestorePoint(string id, List<JobObject> jobObjects, string path, DateTime creationDateTime)
        {
            _storedJobObjects = jobObjects ??
                                throw new ArgumentNullException(nameof(jobObjects));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            CreationDateTime = creationDateTime;
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
        public IReadOnlyCollection<JobObject> StoredJobObjects => _storedJobObjects;
        public string Path { get; }
        public DateTime CreationDateTime { get; }
    }
}