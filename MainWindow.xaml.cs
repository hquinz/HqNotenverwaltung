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
            //HACK List
            // - Calculate days off
            // - Store (new) Schoolyear in DB
            //   - Make sure not to have already a schoolyear with the new year
            //   - Make sure not to overeide an existing indexes for special days if storing new schoolyear
            // - Gui for Days off
            // - Gui to Maniplulate Days off
            // - Schoolyearstatistics
            //   - 3 Parts: common, final clases vocational and technical school
            // ***** Export actual schooldays to Excel *****
            // - Recomendet and actual Rotation changes (store in DB) (Turnuswechsel)


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
        private void ButtonNewScoolyearClick(object sender, RoutedEventArgs e)
        {
            var newSchoolyearData = new VmSchoolyearNew(DateTime.Now.Year % 100);
            var popup = new PopupNewSchoolyear { DataContext = newSchoolyearData };
            bool? result = popup.ShowDialog();
            //HACK React on new schoolyear result = true 
            //HACK: Collect days off in the future

            Debug.WriteLine(result.HasValue ? result.Value.ToString() : "No result returned from popup.");

            newSchoolyearData = null;
        }

     }
}