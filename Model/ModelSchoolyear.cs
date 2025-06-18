
using HqNotenverwaltung.Contracts;

namespace HqNotenverwaltung.Model
{
    public class ModelSchoolyear
    {
        public int StartYear { get; }
        public EnumSemestered Semestered { get; set; }
        public List<ModelDaySchoolSpecial> DateStart { get; private set; } = [];
        public List<ModelDaySchoolSpecial>? DateEnd { get; private set; } = [];
        public List<ModelDaySchoolSpecial>? FreeDays { get; private set; } = [];

    }
}
