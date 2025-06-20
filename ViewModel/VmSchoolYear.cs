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
        //HACK make Numartik input an parse to two-digit int and tak the initvalue Might hve to adapt the initMethod
        private string yearStartNew = "NaN";
        public String YearStartNew
        { get => yearStartNew;
            set { yearStartNew = value;
                Debug.WriteLine("YearStartNew set to: " + yearStartNew);
                initializeSchoolyearNew(25);
            }
        }

        public EnumSemestered SemesteredSelectedNew { get; set; }
        private DateTime dateYearStartNew;
        public DateTime DateYearStartNew
        {
            get => dateYearStartNew;
            set { dateYearStartNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                OnPropertyChanged("DateYearStartNew");
            }
        }
        private DateTime dateAutumnBreakStartNew;
        public DateTime DateAutumnBreakStartNew
        {
            get => dateAutumnBreakStartNew;
            set { dateAutumnBreakStartNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                OnPropertyChanged("DateAutumnBreakStartNew");
            }
        }
        private DateTime dateAutumnBreakEndNew;
        public DateTime DateAutumnBreakEndNew
        {
            get => dateAutumnBreakEndNew;
            set
            {
                dateAutumnBreakEndNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged("DateAutumnBreakEndNew");
            }
        }
        private DateTime dateTechnicalSchoolStartNew;
        public DateTime DateTechnicalSchoolStartNew
        {
            get => dateTechnicalSchoolStartNew;
            set
            {
                dateTechnicalSchoolStartNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                OnPropertyChanged("DateTechnicalSchoolStartNew");
            }
        }
        private DateTime dateSemesterEndNew;
        public DateTime DateSemesterEndNew
        {
            get => dateSemesterEndNew;
            set
            {
                dateSemesterEndNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged("DateSemesterEndNew");
            }
        }
        private DateTime dateVocationalSchoolEndNew;
        public DateTime DateVocationalSchoolEndNew
        {
            get => dateVocationalSchoolEndNew;
            set
            {
                dateVocationalSchoolEndNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged("DateVocationalSchoolEndNew");
            }
        }
        private DateTime dateYearEndNew;
        public DateTime DateYearEndNew
        {
            get => dateYearEndNew;
            set
            {
                dateYearEndNew = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged("DateYearEndNew");
            }
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
        private void initializeSchoolyearNew(int year)
        {
            DateYearStartNew = new DateTime(2000+year, 9, 7);
            DateAutumnBreakStartNew = new DateTime(2000 + year, 10, 26);
            DateAutumnBreakEndNew = DateAutumnBreakStartNew.AddDays(5);
            DateTechnicalSchoolStartNew = new DateTime(2000 + year, 11, 10);
            DateSemesterEndNew = new DateTime(2001 + year, 2, 14);
            DateVocationalSchoolEndNew = new DateTime(2001 + year, 5, 2);
            DateYearEndNew = new DateTime(2001 + year, 7, 3);
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
