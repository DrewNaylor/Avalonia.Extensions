using Avalonia.Extensions.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Avalonia.Controls.Demo
{
    public class MainWindow : Window
    {
        private int Time { get; set; } = 0;
        private Grid PopupContent { get; set; }
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.AttachDevTools();
        }
        private void InitializeComponent()
        {
            Width = 800;
            Height = 600;
            var model = new MainViewModel();
            DataContext = model;
            PopupContent = this.FindControl<Grid>("PopupContent");
        }
        private void OnPopupClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Time++;
                PopupDialog dialog = new PopupDialog(PopupContent);
                dialog.PlacementMode = PlacementMode.Top;
                dialog.Open($"PopupDialog has been show for {Time} times");
            });
        }
    }
}