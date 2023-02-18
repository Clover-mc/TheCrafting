using System;

namespace Minecraft.Tools
{
    /// <summary>
    /// Like TimeSpan but for minecraft where 1 second = 20 ticks
    /// </summary>
    // TODO: Implement other methods and functions to structure
    public struct TickSpan : IComparable, IComparable<TickSpan>, IEquatable<TickSpan>, IComparable<TimeSpan>, IEquatable<TimeSpan>, ISpanFormattable
    {

        /// <summary>
        /// Inteval between ticks in seconds
        /// </summary>
        public const double TicksInterval = 10000000 / TicksPerSecond;
        /// <summary>
        /// TickSpan representation as TimeSpan
        /// </summary>
        public TimeSpan TimeSpan;

        /// <summary>
        /// Maximum possible value of TickSpan
        /// </summary>
        public static readonly TickSpan MaxValue = new TickSpan(long.MaxValue);
        /// <summary>
        /// Minimum possible value of TickSpan
        /// </summary>
        public static readonly TickSpan MinValue = new TickSpan(long.MinValue);
        /// <summary>
        /// Represents TickSpan with zero ticks done
        /// </summary>
        public static readonly TickSpan Zero = new TickSpan();
        /// <summary>
        /// Number of ticks per day
        /// </summary>
        public const long TicksPerDay = TicksPerHour * 24;
        /// <summary>
        /// Number of ticks per hour
        /// </summary>
        public const long TicksPerHour = TicksPerMinute * 60;
        /// <summary>
        /// Number of ticks per minute
        /// </summary>
        public const long TicksPerMinute = TicksPerSecond * 60;
        /// <summary>
        /// Number of ticks per second
        /// </summary>
        public const long TicksPerSecond = 20;
        /// <summary>
        /// Number of ticks per millisecond
        /// </summary>
        public const double TicksPerMillisecond = TicksPerSecond / 1000;

        /// <summary>
        /// Current number of ticks
        /// </summary>
        public long Ticks => (long)TotalTicks % 20;
        /// <summary>
        /// Current number of millisecond
        /// </summary>
        public int Milliseconds => (int)(TotalTicks * (TicksInterval / 10000));
        /// <summary>
        /// Current number of seconds
        /// </summary>
        public int Seconds => (int)TotalMilliseconds % 60;
        /// <summary>
        /// Current number of minutes
        /// </summary>
        public int Minutes => (int)TotalSeconds % 60;
        /// <summary>
        /// Current number of hours
        /// </summary>
        public int Hours => (int)TotalMinutes % 24;
        /// <summary>
        /// Current number of days
        /// </summary>
        public int Days => (int)TotalHours / 24;

        /// <summary>
        /// Total number of ticks
        /// </summary>
        public double TotalTicks => TimeSpan.Ticks / TicksInterval;
        /// <summary>
        /// Total number of milliseconds
        /// </summary>
        public double TotalMilliseconds => Ticks / TicksInterval;
        /// <summary>
        /// Total number of seconds
        /// </summary>
        public double TotalSeconds => TotalMilliseconds / 1000;
        /// <summary>
        /// Total number of minutes
        /// </summary>
        public double TotalMinutes => TotalSeconds / 60;
        /// <summary>
        /// Total number of hours
        /// </summary>
        public double TotalHours => TotalMinutes / 60;
        /// <summary>
        /// Total number of days
        /// </summary>
        public double TotalDays => TotalHours / 24;

        public TickSpan(int hours, int minutes, int seconds) => TimeSpan = new TimeSpan(hours, minutes, seconds);
        public TickSpan(int days, int hours, int minutes, int seconds) => TimeSpan = new TimeSpan(days, hours, minutes, seconds);
        public TickSpan(int days, int hours, int minutes, int seconds, int milliseconds) => TimeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
        /// <summary>
        /// TickSpan constructor
        /// </summary>
        /// <param name="ticks">Ticks in minecraft's format (20 Tick per Second)</param>
        public TickSpan(long ticks) => TimeSpan = new TimeSpan(ticks * (10000000 / TicksPerSecond));
        public TickSpan(TimeSpan time) => TimeSpan = time;
        /// <summary>
        /// Creates TickSpan with zero ticks done
        /// </summary>
        public TickSpan() => TimeSpan = TimeSpan.Zero;

        public int CompareTo(object? obj)
        {
            TimeSpan span;
            if (obj is TickSpan tickSpan) span = tickSpan.TimeSpan;
            else if (obj is TimeSpan timeSpan) span = timeSpan;
            else throw new ArgumentException("Given object is neither " + nameof(TickSpan) + " neither " + nameof(TimeSpan) + '!');

            return TimeSpan.CompareTo(span);
        }

        public int CompareTo(TickSpan other) => TimeSpan.CompareTo(other.TimeSpan);
        public int CompareTo(TimeSpan other) => TimeSpan.CompareTo(other);

        public bool Equals(TickSpan other) => TimeSpan.Equals(other.TimeSpan);
        public bool Equals(TimeSpan other) => TimeSpan.Equals(other);
        public override bool Equals(object? other) => other is TickSpan tickSpan ? TimeSpan.Equals(tickSpan.TimeSpan) : TimeSpan.Equals(other);

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode() => TimeSpan.GetHashCode();

        public static bool operator ==(TickSpan left, TickSpan right) => left.Equals(right);

        public static bool operator !=(TickSpan left, TickSpan right) => !left.Equals(right);

        public static bool operator <(TickSpan left, TickSpan right) => left.CompareTo(right) < 0;

        public static bool operator <=(TickSpan left, TickSpan right) => left.CompareTo(right) <= 0;

        public static bool operator >(TickSpan left, TickSpan right) => left.CompareTo(right) > 0;

        public static bool operator >=(TickSpan left, TickSpan right) => left.CompareTo(right) >= 0;
    }
}
