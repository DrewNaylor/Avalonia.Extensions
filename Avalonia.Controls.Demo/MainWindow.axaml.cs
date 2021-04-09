using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.Controls.Demo
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.AttachDevTools();
        }
        private void InitializeComponent()
        {
            Width = 480;
            Height = 300;
            var model = new MainViewModel();
            DataContext = model;
        }
    }
}