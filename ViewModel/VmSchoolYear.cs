using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using HqNotenverwaltung.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HqNotenverwaltung.ViewModel
{
    public class VmSchoolYear(ISchoolyear schoolyear) : INotifyPropertyChanged
    {
        private ISchoolyear schoolyearModel => schoolyear;
        private ToolsDateTime toolsDateTime => new();
        private enum enumDateTabels { Start, End, Free};

        public Array SemesteredOptions => Enum.GetValues(typeof(EnumSemestered));
        public List<String> Years { 
            get { return getSchoolyears(); }
        }

        private string yearSelected ="";
        public string YearSelected
        {
            get { return yearSelected; }
            set {
                yearSelected = value; 
                if (!string.IsNullOrEmpty(value)) { schoolyearModel.GetSchoolyearAsync(int.Parse(value.Split('/')[0])); }
                updateViewSchoolyear();
            }
        }
        public EnumSemestered SemesteredSelected
        {
            get { return schoolyearModel.ActiveSchoolYear.Semestered; }
            set { schoolyearModel.ActiveSchoolYear.Semestered = value; }
        }
        public DateTime DateYearStart
        {
            get { return schoolyearModel.ActiveSchoolYear.DateStart[0].Date.ToDateTime(new TimeOnly(0)); }
            set { if (registerSpecialDay(enumDateTabels.Start, 0, value)) { OnPropertyChanged("DateYearStart"); } }
        }
        public DateTime DateTechnicalSchoolStart
        {
            get { return schoolyearModel.ActiveSchoolYear.DateStart[1].Date.ToDateTime(new TimeOnly(0)); }
            set { if (registerSpecialDay(enumDateTabels.Start, 1, value)) { OnPropertyChanged("DateTechnicalSchoolStart"); } } 
        }
        public DateTime DateSemesterEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[0].Date.ToDateTime(new TimeOnly(0)); }
            set
            { if (registerSpecialDay(enumDateTabels.End, 0, value)) { OnPropertyChanged("DateSemesterEnd"); } }
        }
        public DateTime DateVocationalSchoolEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[1].Date.ToDateTime(new TimeOnly(0)); }
            set
            { if (registerSpecialDay(enumDateTabels.End, 1, value)) { OnPropertyChanged("DateVocationalSchoolEnd"); }; }
        }
        public DateTime DateYearEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[2].Date.ToDateTime(new TimeOnly(0)); }
            set
            { if (registerSpecialDay(enumDateTabels.End, 2, value)) { OnPropertyChanged("DateYearEnd"); }; }
        }

        public void RefreshSchoolyears() { OnPropertyChanged("Years"); }
        private List<string> getSchoolyears()
        {
            List<int> yearsStart = schoolyearModel.Schoolyears;
            List<string> years = [];
            foreach (var year in yearsStart)
            {
                var nextYear = year+1;
                years.Add(year.ToString() + "/" + nextYear.ToString());
            }
            if (string.IsNullOrEmpty(YearSelected)) { YearSelected = years.FirstOrDefault(""); }
            return years;
        }
        /// <summary>
        /// Checks if the Date fits to its purpose (Starts on Monda, Ends on Friday), 
        /// changes it if necessary and writes it to the model.
        /// </summary>
        /// <param name="table">Table to be used</param>
        /// <param name="index">Index in Table to be usef</param>
        /// <param name="value">Value to be written</param>
        /// <returns>Date-value had to be changed</returns>
        private bool registerSpecialDay (enumDateTabels table, int index, DateTime value)
        {
            bool _dateChanged = false;
            DateTime _day;
            string _remark = "";
            switch (table)
            {
                case enumDateTabels.Start:
                    _day = toolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                    _dateChanged = _day != value;
                    switch (index)
                    {
                        case 0: _remark = "Start Schuljahr"; break;
                        case 1: _remark = "Start Fachschule"; break;
                    }
                    schoolyearModel.ActiveSchoolYear.DateStart[index] = getSchooldayFromDtPicker(index, _day, _remark);
                    break;

                case enumDateTabels.End:
                    _day = toolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                    _dateChanged = _day != value;
                    switch (index)
                    {
                        case 0: _remark = "Ende Semester"; break;
                        case 1: _remark = "Ende Matruaklassen"; break;
                        case 2: _remark = "Ende Schuljahr"; break;
                    }
                    schoolyearModel.ActiveSchoolYear.DateEnd[index] = getSchooldayFromDtPicker(index, _day, _remark);
                    break;
            }
            return _dateChanged;
        }
        private ModelSchooldaySpecial getSchooldayFromDtPicker (int index, DateTime value, string remark) 
        {
            return new ModelSchooldaySpecial
            {
                Id = index,
                Date = DateOnly.FromDateTime(value),
                Remark = remark 
            };
        }

        private void updateViewSchoolyear()
        {
            OnPropertyChanged("SemesteredSelected");
            OnPropertyChanged("DateYearStart");
            OnPropertyChanged("DateTechnicalSchoolStart");
            OnPropertyChanged("DateSemesterEnd");
            OnPropertyChanged("DateVocationalSchoolEnd");
            OnPropertyChanged("DateYearEnd");
        }

        internal void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
