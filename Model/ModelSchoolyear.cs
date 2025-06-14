
using HqNotenverwaltung.Contracts;

namespace HqNotenverwaltung.Model
{
    public class ModelSchoolyear
    {
        public int Id { get; }
        public string Title { get; set; } = "";
        public EnumSemestered Semestered { get; set; }
        public required List<DateOnly> DateStart { get; set; }
        public List<DateOnly>? DateEnd { get; set; }
        public List<FreeDay>? FreeDays { get; set; }

    }
}
