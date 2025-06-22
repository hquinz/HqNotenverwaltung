namespace HqNotenverwaltung.Model
{
    public class ModelSchooldaySpecial : IEquatable<ModelSchooldaySpecial>, IComparable<ModelSchooldaySpecial>
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Remark { get; set; } = "";

        public int SortByIdAscending(int id1, int id2) => id1.CompareTo(id2);
        public int CompareTo(ModelSchooldaySpecial? other)
        {
            if (other is null) return 1;
            return this.Date.CompareTo(other.Date);
        }
        public bool Equals(ModelSchooldaySpecial? other)
        {
            if (other is null) return false;
            return this.Id == other.Id && this.Date == other.Date;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(ModelSchooldaySpecial)) return false;
            return Equals((ModelSchooldaySpecial)obj);
        }
        public static bool operator ==(ModelSchooldaySpecial? left, ModelSchooldaySpecial? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }
        public static bool operator !=(ModelSchooldaySpecial? left, ModelSchooldaySpecial? right) => !(left == right);
        public static bool operator <(ModelSchooldaySpecial? left, ModelSchooldaySpecial? right)
        {
            if (left is null && right is null) return false;
            if (left is null) return true;
            if (right is null) return false;
            return left.Date < right.Date;
        }
        public static bool operator >(ModelSchooldaySpecial? left, ModelSchooldaySpecial? right)
        {
            if (left is null && right is null) return false;
            if (left is null) return false;
            if (right is null) return true;
            return left.Date > right.Date;
        }
        public override int GetHashCode() => HashCode.Combine(Id, Date, Remark);
        public override string ToString() => $"{Date}: {Remark} (ID: {Id})";
    }
}
