
namespace HqNotenverwaltung.Contracts
{
    public class FreeDay
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }
        public string Remark { get; set; } = "";
    }
}
