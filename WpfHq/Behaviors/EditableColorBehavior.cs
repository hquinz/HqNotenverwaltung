using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace HqNotenverwaltung.WpfHq.Behaviors
{
    public class EditableColorBehavior : Behavior<Control>
    {
        public Brush EditableForeground
        {
            get => (Brush)GetValue(EditableForegroundProperty);
            set => SetValue(EditableForegroundProperty, value);
        }
        public static readonly DependencyProperty EditableForegroundProperty =
            DependencyProperty.Register(nameof(EditableForeground), typeof(Brush), typeof(EditableColorBehavior), new PropertyMetadata(Brushes.Black, OnAppearanceChanged));

        public Brush EditableBackground
        {
            get => (Brush)GetValue(EditableBackgroundProperty);
            set => SetValue(EditableBackgroundProperty, value);
        }
        public static readonly DependencyProperty EditableBackgroundProperty =
            DependencyProperty.Register(nameof(EditableBackground), typeof(Brush), typeof(EditableColorBehavior), new PropertyMetadata(Brushes.White, OnAppearanceChanged));

        public Brush ReadonlyForeground
        {
            get => (Brush)GetValue(ReadonlyForegroundProperty);
            set => SetValue(ReadonlyForegroundProperty, value);
        }
        public static readonly DependencyProperty ReadonlyForegroundProperty =
            DependencyProperty.Register(nameof(ReadonlyForeground), typeof(Brush), typeof(EditableColorBehavior), new PropertyMetadata(Brushes.Gray, OnAppearanceChanged));

        public Brush ReadonlyBackground
        {
            get => (Brush)GetValue(ReadonlyBackgroundProperty);
            set => SetValue(ReadonlyBackgroundProperty, value);
        }
        public static readonly DependencyProperty ReadonlyBackgroundProperty =
            DependencyProperty.Register(nameof(ReadonlyBackground), typeof(Brush), typeof(EditableColorBehavior), new PropertyMetadata(Brushes.LightGray, OnAppearanceChanged));

        private DependencyPropertyDescriptor? _isReadOnlyDescriptor;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.IsEnabledChanged += OnIsEnabledChanged;

            // Dynamisch IsReadOnly-Änderungen abonnieren, falls vorhanden
            _isReadOnlyDescriptor = DependencyPropertyDescriptor.FromName(
                "IsReadOnly",
                AssociatedObject.GetType(),
                AssociatedObject.GetType()
            );
            _isReadOnlyDescriptor?.AddValueChanged(AssociatedObject, OnIsReadOnlyChanged);

            UpdateColors();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsEnabledChanged -= OnIsEnabledChanged;

            _isReadOnlyDescriptor?.RemoveValueChanged(AssociatedObject, OnIsReadOnlyChanged);
            _isReadOnlyDescriptor = null;

            base.OnDetaching();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateColors();
        }

        private void OnIsReadOnlyChanged(object? sender, EventArgs e)
        {
            UpdateColors();
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableColorBehavior behavior)
                behavior.UpdateColors();
        }

        private void UpdateColors()
        {
            bool isEditable = AssociatedObject.IsEnabled;

            // Prüfen, ob IsReadOnly existiert und berücksichtigen
            var prop = AssociatedObject.GetType().GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.Public);
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                bool isReadOnly = (bool)prop.GetValue(AssociatedObject)!;
                isEditable &= !isReadOnly;
            }

            AssociatedObject.Foreground = isEditable ? EditableForeground : ReadonlyForeground;
            AssociatedObject.Background = isEditable ? EditableBackground : ReadonlyBackground;
        }
    }
}