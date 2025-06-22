
namespace HqNotenverwaltung.Feiertagsberechnung
{
    internal class DayNamed : IEquatable<DayNamed>, IComparable<DayNamed>
    {
        public DateOnly Date { get; }
        public string Name { get; }

        public DayNamed(DateOnly date, string name)
        {
            Date = date;
            Name = name;
        }

        public int CompareTo(DayNamed? date)
        {
            if (date is null) return 1;
            return this.Date.CompareTo(date.Date);
        }

        public bool Equals(DayNamed? date)
        {
            if (date is null) return false;
            return this.Date == date.Date;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(DayNamed)) return false;
            return Equals((DayNamed)obj);
        }
        public static bool operator ==(DayNamed? left, DayNamed? right)
            {
                if (left is null && right is null) return true;
                if (left is null || right is null) return false;
                return left.Equals(right);
            }
        public static bool operator !=(DayNamed? left, DayNamed? right) => !(left == right);
        public static bool operator <(DayNamed? left, DayNamed? right)
        {
            if (left is null && right is null) return false;
            if (left is null) return true;
            if (right is null) return false;
            return left.Date < right.Date;
        }
        public static bool operator >(DayNamed? left, DayNamed? right)
        {
            if (left is null && right is null) return false;
            if (left is null) return false;
            if (right is null) return true;
            return left.Date > right.Date;
        }
        public override int GetHashCode() => HashCode.Combine(Date, Name);
        public override string ToString() => Date.ToString()+ ": " + Name;
    }
}
