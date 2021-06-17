using System;

namespace GameServer.Utilities
{
    public class Range<T> where T : IComparable<T>
    {
        public T Min { get; set; }

        public T Max { get; set; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool IsValid()
        {
            return Min.CompareTo(Max) <= 0;
        }

        public bool ContainsValue(T value)
        {
            return (Min.CompareTo(value) <= 0) && (value.CompareTo(Max) <= 0);
        }

        public bool IsInRange(Range<T> range)
        {
            return IsValid() && range.IsValid() && range.ContainsValue(Min) && range.ContainsValue(Max);
        }

        public bool ContainsRange(Range<T> range)
        {
            return IsValid() && range.IsValid() && this.ContainsValue(range.Min) && this.ContainsValue(range.Max);
        }

        public bool Overlap(Range<T> range)
        {
            return this.ContainsRange(range);
        }

        public override string ToString()
        {
            return $"[{Min} - {Max}]";
        }
    }
}
