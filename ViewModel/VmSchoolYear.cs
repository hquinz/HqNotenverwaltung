using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
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
        private ISchoolyear SchoolyearModel => schoolyear;

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
                if (!string.IsNullOrEmpty(value)) { SchoolyearModel.GetSchoolyearAsync(int.Parse(value.Split('/')[0])); }
                UpdateViewSchoolyear();
            }
        }
        public EnumSemestered SemesteredSelected
        {
            get { return SchoolyearModel.ActiveSchoolYear.Semestered; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.Semestered = value;
            }
        }
        public DateTime DateYearStart
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateStart[0].Date.ToDateTime(new TimeOnly(0)); }
            set
            {
                int index = 0;
                SchoolyearModel.ActiveSchoolYear.DateStart[index]= GetSchooldayFromDtPicker(index, value, "Start Schuljahr");
            }
        }
        public DateTime DateTechnicalSchoolStart
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateStart[1].Date.ToDateTime(new TimeOnly(0)); }
            set
            {
                int index = 1;
                SchoolyearModel.ActiveSchoolYear.DateStart[index] = GetSchooldayFromDtPicker(index, value, "Start Fachschule");
            }
        }
        public DateTime DateSemesterEnd
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[0].Date.ToDateTime(new TimeOnly(0)); }
            set
            {
                int index = 0;
                SchoolyearModel.ActiveSchoolYear.DateEnd[index] = GetSchooldayFromDtPicker(index, value, "Ende Semester");
            }
        }
        public DateTime DateVocationalSchoolEnd
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[1].Date.ToDateTime(new TimeOnly(0)); }
            set
            {
                int index = 1;
                SchoolyearModel.ActiveSchoolYear.DateEnd[index] = GetSchooldayFromDtPicker(index, value, "Ende Matruaklassen");
            }
        }
        public DateTime DateYearEnd
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[2].Date.ToDateTime(new TimeOnly(0)); }
            set
            {
                int index = 2;
                SchoolyearModel.ActiveSchoolYear.DateEnd[index] = GetSchooldayFromDtPicker(index, value, "Ende Schuljahr");
            }
        }

        public void RefreshSchoolyears() { OnPropertyChanged("Years"); }
        private List<string> getSchoolyears()
        {
            List<int> yearsStart = SchoolyearModel.Schoolyears;
            List<string> years = [];
            foreach (var year in yearsStart)
            {
                var nextYear = year+1;
                years.Add(year.ToString() + "/" + nextYear.ToString());
            }
            if (string.IsNullOrEmpty(YearSelected)) { YearSelected = years.FirstOrDefault(""); }
            return years;
        }
        private ModelSchooldaySpecial GetSchooldayFromDtPicker (int index, DateTime value, string remark) 
        {
            return new ModelSchooldaySpecial
            {
                Id = index,
                Date = DateOnly.FromDateTime(value),
                Remark = remark 
            };
        }

        private void UpdateViewSchoolyear()
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
