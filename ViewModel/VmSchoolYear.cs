using HqNotenverwaltung.Contracts;
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

        private string _SelectedYear ="";
        public string SelectedYear
        {
            get { return _SelectedYear; }
            set {
                _SelectedYear = value; 
                if (!string.IsNullOrEmpty(value))
                {
                    Debug.WriteLine("Selected Year: " + value);
                    SchoolyearModel.GetSchoolyearAsync(int.Parse(value.Split('/')[0]));
                }
                OnPropertyChanged("SelectedYear");
                OnPropertyChanged("SelectedSemestered");
            }
        }
        public EnumSemestered SelectedSemestered
        {
            get { return SchoolyearModel.ActiveSchoolYear.Semestered; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.Semestered = value;
                OnPropertyChanged("SelectedSemestered");
            }
        }
        public DateOnly StartDateYear
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateStart[0].Date; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Id = 0;
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Date = value;
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Remark = "Start Schuljahr";
                OnPropertyChanged("StartDateYear");
            }
        }
        public DateOnly StartDateTechnicalSchool
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateStart[1].Date; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Id = 1;
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Date = value;
                SchoolyearModel.ActiveSchoolYear.DateStart[0].Remark = "Start Fachschule";
                OnPropertyChanged("StartDateTechnicalSchool");
            }
        }
        public DateOnly EndDateSemester
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[0].Date; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Id = 0;
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Date = value;
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Remark = "Ende Semester";
                OnPropertyChanged("EndDateSemester");
            }
        }
        public DateOnly EndDateVocationalSchool
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[1].Date; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Id = 1;
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Date = value;
                SchoolyearModel.ActiveSchoolYear.DateEnd[0].Remark = "Ende Matruaklassen";
                OnPropertyChanged("EndDateVocationalSchool");
            }
        }
        public DateOnly EndDateYear
        {
            get { return SchoolyearModel.ActiveSchoolYear.DateEnd[3].Date; }
            set
            {
                SchoolyearModel.ActiveSchoolYear.DateStart[3].Id = 0;
                SchoolyearModel.ActiveSchoolYear.DateStart[3].Date = value;
                SchoolyearModel.ActiveSchoolYear.DateStart[3].Remark = "Ende Schuljahr";
                OnPropertyChanged("EndDateYear");
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
            if (string.IsNullOrEmpty(SelectedYear)) { SelectedYear = years.FirstOrDefault(""); }
            return years;
        }

        internal void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
