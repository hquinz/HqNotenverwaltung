using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HqNotenverwaltung.WpfHq.Controls

{
    public class NumericTextBox : TextBox
    {

        private bool _gotTextFromValue = false;

        #region Register Dependency Properties
        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(nameof(AllowNegative), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty AllowPositiveProperty =
            DependencyProperty.Register(nameof(AllowPositive), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty PreDecimalPlacesProperty =
            DependencyProperty.Register(nameof(PreDecimalPlaces), typeof(int), typeof(NumericTextBox), new PropertyMetadata(0));
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumericTextBox), new PropertyMetadata(0));
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(nameof(Unit), typeof(string), typeof(NumericTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double?), typeof(NumericTextBox),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValuePropertyChanged));
        #endregion

        public bool AllowNegative { get => (bool)GetValue(AllowNegativeProperty); set => SetValue(AllowNegativeProperty, value); }
        public bool AllowPositive { get => (bool)GetValue(AllowPositiveProperty); set => SetValue(AllowPositiveProperty, value); }
        public int PreDecimalPlaces { get => (int)GetValue(PreDecimalPlacesProperty); set => SetValue(PreDecimalPlacesProperty, value); }
        public int DecimalPlaces { get => (int)GetValue(DecimalPlacesProperty); set => SetValue(DecimalPlacesProperty, value); }
        public string Unit { get => (string)GetValue(UnitProperty); set => SetValue(UnitProperty, value); }
        public double? Value { get => (double?)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        public NumericTextBox()
        {
            PreviewKeyDown += OnPreviewKeyDown;
            PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(this, OnPaste);
            LostFocus += OnLostFocus;
            GotFocus += OnGotFocus;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetValue(ValueProperty, textToValue(Text));
                valueToText(Value);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back || e.Key == Key.Delete) 
            { 
                _gotTextFromValue = false;
                return;
            }
            if (e.Key == Key.Escape)
            {
                valueToText (Value);
                e.Handled = true;
                return;
            }
        }
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_gotTextFromValue)
            {
                prepairEditText();
                if (SelectionStart == 0) Text = string.Empty; 
            }

            string fullText = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, e.Text);
            if (!IsValidInput(fullText)) e.Handled = true;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(typeof(string))) { e.CancelCommand(); return; }

            string _fullText = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, (string)e.DataObject.GetData(typeof(string)));
            if (!IsValidInput(_fullText)) e.CancelCommand();
        }
        private void OnGotFocus(object sender, RoutedEventArgs e) { Text = removeUnitIfPresent(Text); }

        private void OnLostFocus(object sender, RoutedEventArgs e) { SetValue(ValueProperty, textToValue(Text)); }

        private bool IsValidInput(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return true;
            if (string.Equals(text, "-") && AllowNegative) return true;
            if (!DecimalDigitsOK(text)) return false;
            if (double.TryParse(text, out double value)) return checkValue(value);
            return false;
        }
        private bool DecimalDigitsOK(string text)
        {
            var gotDecimalPoint = text.Contains('.');
            var gotDecimalComma = text.Contains(',');

            if (gotDecimalPoint && gotDecimalComma) return false;

            var seperator = gotDecimalPoint ? '.' : ',';

            if (!text.Contains(seperator)) return true;
            if (text.IndexOf(seperator) != text.LastIndexOf(seperator)) return false; // More than one decimal separator
            if (text.Length - text.IndexOf(seperator) - 1 > DecimalPlaces) return false;
            return true;
        }
        private bool checkValue(double value)
        {
            var valueInt = (int)(value*Math.Pow(10,DecimalPlaces));

            var _valueZero = valueInt == 0;
            var _valueNegative = valueInt < 0;
            int strippedValue = (int)(valueInt / Math.Pow(10, DecimalPlaces + PreDecimalPlaces));
            var _predecimalExceed = strippedValue != 0 && PreDecimalPlaces != 0;
            return ((_valueNegative && AllowNegative) || (!_valueNegative && AllowPositive) || _valueZero) && !_predecimalExceed;
        }
        private double? textToValue(string text)
        {
            var _text = removeUnitIfPresent(text);
            return double.TryParse(_text, NumberStyles.Float, CultureInfo.CurrentCulture, out var value) ? value : (float?)null;
        }
        private void valueToText(double? value)
        {
            Text = (value.HasValue) ? value.Value.ToString($"F{DecimalPlaces}", CultureInfo.CurrentCulture) + Unit : string.Empty;
            SelectionStart = 0;
            _gotTextFromValue = true;
        }
        private void prepairEditText()
        {
            Text = removeUnitIfPresent(Text);
            _gotTextFromValue = false;
        }
        private string removeUnitIfPresent(string text)
        {
            if (string.IsNullOrEmpty(Unit)) return text.Trim();
            return (text.Contains(Unit)) ? text.Replace(Unit, string.Empty).Trim() : text.Trim();
        }  

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericTextBox)d;
            control.valueToText((double?)e.NewValue);
        }
    }
}