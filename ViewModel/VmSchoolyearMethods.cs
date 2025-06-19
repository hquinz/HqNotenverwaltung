using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using HqNotenverwaltung.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.ViewModel
{
    class VmSchoolyearMethods
    {
        public List<string> GetSchoolyears(List<int> schoolyears)
        {
            List<int> yearsStart = schoolyears;
            List<string> years = [];
            foreach (var year in yearsStart)
            {
                var nextYear = year + 1;
                years.Add(year.ToString() + "/" + nextYear.ToString());
            }
            return years;
        }
        public ModelSchooldaySpecial GetRegisterDataSpecialDay(EnumDateTabels table, int index, ref DateTime day, DateTime value)
        {
            string _remark = "";
            switch (table)
            {
                //TODO: Delete comments
                case EnumDateTabels.Start:
                    day = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                    _remark = ToolsEnum.GetDescription<EnumSchoolyearStartRemarks>(index);
                    return GetSchooldayFromDtPicker(index, day, _remark);

                case EnumDateTabels.End:
                    day = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                    _remark = ToolsEnum.GetDescription<EnumSchoolyearEndRemarks>(index);
                    return GetSchooldayFromDtPicker(index, day, _remark);
            }
            return GetSchooldayFromDtPicker(666, DateTime.Now, "");
        }
        public ModelSchooldaySpecial GetSchooldayFromDtPicker(int index, DateTime value, string remark)
        {
            return new ModelSchooldaySpecial
            {
                Id = index,
                Date = DateOnly.FromDateTime(value),
                Remark = remark
            };
        }

    }
}
