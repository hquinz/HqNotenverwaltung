using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Data.SqlLite;
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
            ISchoolyear schoolyear = new SqLiteProviderSchoolyear();

            schoolyear.ConnectAsync("Daten", "Schuljahr");

            var schuljahre = schoolyear.Schoolyears;

            foreach (var y in schuljahre) {
                Debug.WriteLine(y);

            }

            InitializeComponent();



        }
    }
}