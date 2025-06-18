using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Data;
using HqNotenverwaltung.Data.SqlLite;
using HqNotenverwaltung.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HqNotenverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ISchoolyear schoolyear = new RepositorySchoolyear(new SQLiteDbManager());

            schoolyear.ConnectAsync("Daten", "Schuljahr");
            // TODO: löschen:
            //Debug.AutoFlush = true;
            //var deleteMe = new SQLiteDbManager("Daten", "Schuljahr");
            //var seed = new Seed(deleteMe);
            //using var _ = seed.DoAsync(deleteMe.CommandsSchoolyearQuery);

            InitializeComponent();
            VmSchoolYear vmSchoolYear = new(schoolyear);
            DataContext = vmSchoolYear;
            // https://stackoverflow.com/questions/561166/binding-a-wpf-combobox-to-a-custom-list

        }
    }
}