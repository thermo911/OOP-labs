using System;
using System.IO;
using Backups.Tools.Exceptions;
using Newtonsoft.Json;

namespace Backups.Entities
{
    public class JobObject : IEquatable<JobObject>
    {
        private readonly string _fullName;
        public JobObject(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("JobObject constructor: 'path' is invalid");

            FileInfo = new FileInfo(path);
            _fullName = FileInfo.FullName;
        }

        [JsonIgnore]
        public FileInfo FileInfo { get; }

        public string Path => FileInfo.FullName;

        public bool Equals(JobObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _fullName.Equals(other._fullName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JobObject)obj);
        }

        public override int GetHashCode() => _fullName != null ? _fullName.GetHashCode() : 0;

        public override string ToString() => _fullName;
    }
}