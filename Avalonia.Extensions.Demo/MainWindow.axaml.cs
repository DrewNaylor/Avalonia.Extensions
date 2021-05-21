using Avalonia.Extensions.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Collections.Generic;

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
            var imgList = this.FindControl<ListBox>("imgList");
            imgList.Items = new List<object>
            {
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" }
            };
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
        private void OnNotifyClick(object sender, RoutedEventArgs e)
        {
            NotifyWindow window = new NotifyWindow();
            window.Content = new TextBlock { Text = "大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大" };
            var options = new Options(ShowPosition.BottomRight, new Size(160, 60), ScollOrientation.Horizontal);
            window.Show(options);
        }
    }
}