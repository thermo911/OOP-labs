using System;
using BackupsExtra.Logging;
using Newtonsoft.Json;

namespace BackupsExtra.Entities
{
    public class PointsCountSettings
    {
        [JsonConstructor]
        private PointsCountSettings(
            TimeSpan maxPointExistenceTime,
            int maxPointsCount,
            LimitMatchingMode matchingMode,
            bool preferMerge,
            ILogger logger)
        {
            MaxPointExistenceTime = maxPointExistenceTime;
            MaxPointsCount = maxPointsCount;
            MatchingMode = matchingMode;
            PreferMerge = preferMerge;
            Logger = logger;
        }

        private PointsCountSettings() { }

        public enum LimitMatchingMode : byte
        {
            MatchAll,
            MatchAny,
        }

        public ILogger Logger { get; set; }

        public TimeSpan MaxPointExistenceTime { get; private set; }
        public int MaxPointsCount { get; private set; }
        public LimitMatchingMode MatchingMode { get; private set; }
        public bool PreferMerge { get; private set; }

        public static Builder BuildNew() => new Builder();
        public static PointsCountSettings NoLimitsSettings() => new Builder().Build();

        public bool IsMatchingLimits(RestorePointX point, int index, int count)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));

            if (index >= count)
                throw new ArgumentException($"'{nameof(index)}' >= '{nameof(count)}'");

            switch (MatchingMode)
            {
                case PointsCountSettings.LimitMatchingMode.MatchAll:
                    return
                        (DateTime.Now - point.CreationDateTime) <= MaxPointExistenceTime
                        && index >= count - MaxPointsCount;
                default:
                    return
                        (DateTime.Now - point.CreationDateTime) <= MaxPointExistenceTime
                        || index >= count - MaxPointsCount;
            }
        }

        public class Builder
        {
            private TimeSpan _maxPointExistenceTime = TimeSpan.MaxValue;
            private int _maxPointsCount = int.MaxValue;
            private LimitMatchingMode _matchingMode = PointsCountSettings.LimitMatchingMode.MatchAny;
            private bool _preferMerge = true;

            public Builder MaxPointExistenceTime(TimeSpan timeSpan)
            {
                _maxPointExistenceTime = timeSpan;
                return this;
            }

            public Builder MaxPointsCount(int count)
            {
                if (count <= 0)
                    throw new ArgumentException($"'{nameof(count)}' is negative or zero");
                _maxPointsCount = count;
                return this;
            }

            public Builder LimitMatchingMode(LimitMatchingMode mode)
            {
                _matchingMode = mode;
                return this;
            }

            public Builder PreferMerge(bool isPreferred)
            {
                _preferMerge = isPreferred;
                return this;
            }

            public PointsCountSettings Build()
            {
                return new PointsCountSettings
                {
                    MaxPointExistenceTime = _maxPointExistenceTime,
                    MaxPointsCount = _maxPointsCount,
                    MatchingMode = _matchingMode,
                    PreferMerge = _preferMerge,
                };
            }
        }
    }
}