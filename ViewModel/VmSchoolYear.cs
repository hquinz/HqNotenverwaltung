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

        public List<String> Years { 
            get { return getSchoolyears(); }
//            set { OnPropertyChanged("SelectedYear"); }
        }

        private string p_SelectedYear ="";
        public string SelectedYear
        {
            get { return p_SelectedYear; }
            set {
                p_SelectedYear = value; 
                if (!string.IsNullOrEmpty(value))
                {
                    Debug.WriteLine("Selected Year: " + value);
                    SchoolyearModel.GetSchoolyear(int.Parse(value.Split('/')[0]));

                }
                OnPropertyChanged("SelectedYear");
            }
        }


        public void RefreshSchoolyears()
        {
            OnPropertyChanged("Years");
        }

        private List<string> getSchoolyears()
        {
            List<int> yearsStart = SchoolyearModel.Schoolyears;
            List<string> years = [];
            foreach (var year in yearsStart)
            {
                
                Debug.WriteLine("Year: " + year);
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
