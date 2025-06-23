using System.Windows;

namespace HqNotenverwaltung.ViewPopups
{
    /// <summary>
    /// Interaktionslogik für PopupNewSchoolyear.xaml
    /// </summary>
    public partial class PopupNewSchoolyear : Window
    {
        public PopupNewSchoolyear()
        {
            InitializeComponent();
        }

        private void BtnSchoolyearNewCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnSchoolyearNewSaveClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
