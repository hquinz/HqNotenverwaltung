namespace HqNotenverwaltung.Model
{
    public class ModelDaySchoolSpecial
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }
        public string Remark { get; set; } = "";
    }
}
