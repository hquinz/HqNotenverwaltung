using System.ComponentModel;

namespace HqNotenverwaltung.ViewModel
{
    internal enum EnumSchoolyearEndRemarks
    {
        [Description("Ende Semester")]      EndSemester = 0,
        [Description("Ende Matruaklassen")] EndVocationalSchool = 1,
        [Description("Ende Schuljahr")]     EndSchoolyear = 2
    }
}
