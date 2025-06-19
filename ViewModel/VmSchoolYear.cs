using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using HqNotenverwaltung.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HqNotenverwaltung.ViewModel
{
    public class VmSchoolYear(ISchoolyear schoolyear) : INotifyPropertyChanged
    {
        private ISchoolyear schoolyearModel => schoolyear;
        private VmSchoolyearMethods schoolyearMethods => new();
        public Array SemesteredOptions => Enum.GetValues(typeof(EnumSemestered));
        public List<String> Years { 
            get {
                var years = schoolyearMethods.GetSchoolyears(schoolyearModel.Schoolyears);
                if (string.IsNullOrEmpty(YearSelected)) { YearSelected = years.FirstOrDefault(""); }
                return years; 
            }
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
            set { if (registerSpecialDay(EnumDateTabels.Start, 0, value)) { OnPropertyChanged("DateYearStart"); } }
        }
        public DateTime DateTechnicalSchoolStart
        {
            get { return schoolyearModel.ActiveSchoolYear.DateStart[1].Date.ToDateTime(new TimeOnly(0)); }
            set { if (registerSpecialDay(EnumDateTabels.Start, 1, value)) { OnPropertyChanged("DateTechnicalSchoolStart"); } } 
        }
        public DateTime DateSemesterEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[0].Date.ToDateTime(new TimeOnly(0)); }
            set
            { if (registerSpecialDay(EnumDateTabels.End, 0, value)) { OnPropertyChanged("DateSemesterEnd"); } }
        }
        public DateTime DateVocationalSchoolEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[1].Date.ToDateTime(new TimeOnly(0)); }
            set { if (registerSpecialDay(EnumDateTabels.End, 1, value)) { OnPropertyChanged("DateVocationalSchoolEnd"); }; }
        }
        public DateTime DateYearEnd
        {
            get { return schoolyearModel.ActiveSchoolYear.DateEnd[2].Date.ToDateTime(new TimeOnly(0)); }
            set { if (registerSpecialDay(EnumDateTabels.End, 2, value)) { OnPropertyChanged("DateYearEnd"); }; }
        }

        public void RefreshSchoolyears() { OnPropertyChanged("Years"); }

        /// <summary>
        /// Checks if the Date fits to its purpose (Starts on Monda, Ends on Friday), 
        /// changes it if necessary and writes it to the model.
        /// </summary>
        /// <param name="table">Table to be used</param>
        /// <param name="index">Index in Table to be usef</param>
        /// <param name="value">Value to be written</param>
        /// <returns>Date-value had to be changed</returns>
        private bool registerSpecialDay (EnumDateTabels table, int index, DateTime value)
        {
            DateTime _day = value;
            switch (table)
            {
                case EnumDateTabels.Start:
                    schoolyearModel.ActiveSchoolYear.DateStart[index]  = 
                        schoolyearMethods.GetRegisterDataSpecialDay(table,index, ref _day, value);
                    break;

                case EnumDateTabels.End:
                    schoolyearModel.ActiveSchoolYear.DateEnd[index] = 
                        schoolyearMethods.GetRegisterDataSpecialDay(table, index, ref _day, value);
                    break;
            }
            return _day != value;
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
