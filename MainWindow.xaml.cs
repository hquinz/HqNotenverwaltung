using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Data;
using HqNotenverwaltung.Data.SqlLite;
using HqNotenverwaltung.ViewModel;
using HqNotenverwaltung.ViewPopups;
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
        private readonly VmSchoolYear vmSchoolYear;
        public MainWindow()
        {
            using ISchoolyear schoolyear = new RepositorySchoolyear(new SQLiteDbManager());

            schoolyear.ConnectAsync("Daten", "Schuljahr");
            // TODO: löschen:
            //Debug.AutoFlush = true;
            //var deleteMe = new SQLiteDbManager("Daten", "Schuljahr");
            //var seed = new Seed(deleteMe);
            //using var _ = seed.DoAsync(deleteMe.CommandsSchoolyearQuery);

            InitializeComponent();
            vmSchoolYear = new(schoolyear);
            DataContext = vmSchoolYear;

        }   
        private void ButtonNewScoolyearMousUp(object sender, RoutedEventArgs e)
        {
            var popup = new PopupNewSchoolyear { DataContext = this.DataContext };
            //HACK Provide proper startinformation for new schoolyear
            vmSchoolYear.YearStartNew = DateTime.Now.ToString("yy");
            bool? result = popup.ShowDialog();
            //HACK React on new schoolyear
            Debug.WriteLine(result.HasValue ? result.Value.ToString() : "No result returned from popup.");
        }

    }
}