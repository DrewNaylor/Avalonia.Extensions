using Avalonia.Controls.Templates;
using Avalonia.Styling;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Avalonia.Extensions.Controls
{
    public class GridViewColumn : AvaloniaObject, INotifyPropertyChanged
    {
        public GridViewColumn()
        {
            ResetPrivateData();
            _state = double.IsNaN(Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth;
            HeaderProperty.Changed.AddClassHandler<GridViewColumn>(OnHeaderChanged);
            HeaderContainerStyleProperty.Changed.AddClassHandler<GridViewColumn>(OnHeaderContainerStyleChanged);
            HeaderTemplateProperty.Changed.AddClassHandler<GridViewColumn>(OnHeaderTemplateChanged);
            HeaderStringFormatProperty.Changed.AddClassHandler<GridViewColumn>(OnHeaderStringFormatChanged);
            CellTemplateProperty.Changed.AddClassHandler<GridViewColumn>(OnCellTemplateChanged);
            WidthProperty.Changed.AddClassHandler<GridViewColumn>(OnWidthChanged);
        }
        public override string ToString()
        {
            throw new NotImplementedException(); 
        }
        public static readonly AvaloniaProperty HeaderProperty = AvaloniaProperty.Register<GridViewColumn, object>(nameof(Header));
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        private static void OnHeaderChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn c = (GridViewColumn)d;
            c.OnPropertyChanged(HeaderProperty.Name);
        }
        public static readonly AvaloniaProperty HeaderContainerStyleProperty = AvaloniaProperty.Register<GridViewColumn, Style>(nameof(HeaderContainerStyle));
        public Style HeaderContainerStyle
        {
            get { return (Style)GetValue(HeaderContainerStyleProperty); }
            set { SetValue(HeaderContainerStyleProperty, value); }
        }
        private static void OnHeaderContainerStyleChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn c = (GridViewColumn)d;
            c.OnPropertyChanged(HeaderContainerStyleProperty.Name);
        }
        public static readonly AvaloniaProperty HeaderTemplateProperty = AvaloniaProperty.Register<GridViewColumn, IDataTemplate>(nameof(HeaderTemplate));
        public IDataTemplate HeaderTemplate
        {
            get { return (IDataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        private static void OnHeaderTemplateChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn c = (GridViewColumn)d;
            c.OnPropertyChanged(HeaderTemplateProperty.Name);
        }
        public static readonly AvaloniaProperty HeaderStringFormatProperty = AvaloniaProperty.Register<GridViewColumn, string>(nameof(HeaderStringFormat), null);
        public string HeaderStringFormat
        {
            get { return (string)GetValue(HeaderStringFormatProperty); }
            set { SetValue(HeaderStringFormatProperty, value); }
        }
        private static void OnHeaderStringFormatChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn ctrl = (GridViewColumn)d;
            ctrl.OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
        }
        protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat) { }
        public static readonly AvaloniaProperty CellTemplateProperty = AvaloniaProperty.Register<GridViewColumn, IDataTemplate>(nameof(CellTemplate));
        public IDataTemplate CellTemplate
        {
            get { return (IDataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }
        private static void OnCellTemplateChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn c = (GridViewColumn)d;
            c.OnPropertyChanged(CellTemplateProperty.Name);
        }
        public static readonly AvaloniaProperty WidthProperty = AvaloniaProperty.Register<GridViewColumn, double>(nameof(WidthProperty), double.NaN);
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        private static void OnWidthChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumn c = (GridViewColumn)d;
            double newWidth = (double)e.NewValue;
            c.State = double.IsNaN(newWidth) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth;
            c.OnPropertyChanged(WidthProperty.Name);
        }
        public double ActualWidth
        {
            get => _actualWidth;
            private set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0)
                    Debug.Assert(false, "Invalid value for ActualWidth.");
                else if (_actualWidth != value)
                {
                    _actualWidth = value;
                    OnPropertyChanged(c_ActualWidthName);
                }
            }
        }
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
        private event PropertyChangedEventHandler propertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            propertyChanged?.Invoke(this, e);
        }
        internal double EnsureWidth(double width)
        {
            if (width > DesiredWidth)
                DesiredWidth = width;
            return DesiredWidth;
        }
        internal void ResetPrivateData()
        {
            _actualIndex = -1;
            _desiredWidth = 0.0;
            _state = double.IsNaN(Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth;
        }
        internal ColumnMeasureState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    if (value != ColumnMeasureState.Init)
                        UpdateActualWidth();
                    else
                        DesiredWidth = 0.0;
                }
                else if (value == ColumnMeasureState.SpecificWidth)
                    UpdateActualWidth();

            }
        }
        internal int ActualIndex
        {
            get { return _actualIndex; }
            set { _actualIndex = value; }
        }
        internal double DesiredWidth
        {
            get { return _desiredWidth; }
            private set { _desiredWidth = value; }
        }
        internal const string c_ActualWidthName = "ActualWidth";
        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateActualWidth()
        {
            ActualWidth = (State == ColumnMeasureState.SpecificWidth) ? Width : DesiredWidth;
        }
        private double _desiredWidth;
        private int _actualIndex;
        private double _actualWidth;
        private ColumnMeasureState _state;
    }
    internal enum ColumnMeasureState
    {
        Init = 0,
        Headered = 1,
        Data = 2,
        SpecificWidth = 3
    }
}