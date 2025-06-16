using HqNotenverwaltung.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.ViewModel
{
    public class VmSchoolYear(ISchoolyear schoolyear) : INotifyPropertyChanged
    {
        private ISchoolyear SchoolyearModel => schoolyear;



        public List<String> Years { 
            get { return getSchoolyears(); }
//            set { RaisePropertyChanged("Years"); }
        }

        private string p_SelectedYear ="";
        public string SelectedYear
        {
            get { return p_SelectedYear; }
            set {
                p_SelectedYear = value; 
                RaisePropertyChanged("SelectedYear"); 
            }
        }


        public void RefreshSchoolyears()
        {
            RaisePropertyChanged("Years");
        }

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

        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
