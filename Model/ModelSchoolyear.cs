
using HqNotenverwaltung.Contracts;

namespace HqNotenverwaltung.Model
{
    public class ModelSchoolyear
    {
        public int StartYear { get; set; }
        public EnumSemestered Semestered { get; set; }
        public List<ModelSchooldaySpecial> DateStart { get; set; } = [];
        public List<ModelSchooldaySpecial> DateEnd { get; set; } = [];
        public List<ModelSchooldaySpecial> DaysFree { get; set; } = [];

    }
}
