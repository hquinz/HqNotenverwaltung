using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HqNotenverwaltung.WpfHq.Controls

{
    public class NumericTextBox : TextBox
    {
        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(nameof(AllowNegative), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty AllowPositiveProperty =
            DependencyProperty.Register(nameof(AllowPositive), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumericTextBox), new PropertyMetadata(0));

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(nameof(Unit), typeof(string), typeof(NumericTextBox), new PropertyMetadata(string.Empty));

        public bool AllowNegative
        {
            get => (bool)GetValue(AllowNegativeProperty);
            set => SetValue(AllowNegativeProperty, value);
        }

        public bool AllowPositive
        {
            get => (bool)GetValue(AllowPositiveProperty);
            set => SetValue(AllowPositiveProperty, value);
        }

        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public NumericTextBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(this, OnPaste);
            LostFocus += OnLostFocus;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string fullText = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, e.Text);
            e.Handled = !IsValidInput(fullText);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                string fullText = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, pasteText);
                if (!IsValidInput(fullText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Text, out double value)) { Text = value.ToString($"F{DecimalPlaces}", CultureInfo.CurrentCulture); }
        }

        private bool IsValidInput(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return true;
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
            if (text.Length - text.IndexOf(seperator) -1 > DecimalPlaces) return false;
            return true;
        }
        private bool checkValue(double value)
        {
            if (!AllowNegative && value < 0) return false;
            if (!AllowPositive && value > 0) return false;
            return true;
        }
    }
}