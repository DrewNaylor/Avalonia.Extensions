using Avalonia.Extensions.Controls;
using Avalonia.Extensions.Demo;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Collections.ObjectModel;

namespace Avalonia.Controls.Demo
{
    public class MainWindow : Window
    {
        private int Time { get; set; } = 0;
        private Grid PopupContent { get; set; }
        private ObservableCollection<object> Collection { get; set; }
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
            var cellListView = this.FindControl<CellListView>("cellListView");
            cellListView.ItemClick += CellListView_ItemRightClick;
            Collection = new ObservableCollection<object>
            {
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "http://s1.hdslb.com/bfs/static/passport/static/img/rl_top.35edfde.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" },
                new { Url = "https://i0.hdslb.com/bfs/live/c8e6d780a3182c37a96e79f4ed26fcb576f2520a.png" }
            };
            imgList.Items = Collection;
            var scrollView = this.FindControl<ScrollView>("scrollView");
            scrollView.ScrollEnd += ScrollView_ScrollEnd;
            scrollView.ScrollTop += ScrollView_ScrollTop;
            var a = scrollView.Content;
        }
        private void CellListView_ItemRightClick(object? sender, ViewRoutedEventArgs e)
        {
            if (e.ClickMouse == MouseButton.Right)
                MessageBox.Show("tips", "CellListView -> Right Click");
            else
                MessageBox.Show("tips", "CellListView -> Left Click");
        }
        private void ScrollView_ScrollTop(object? sender, RoutedEventArgs e)
        {
            MessageBox.Show("tips", "ScrollView -> Scroll Top");
        }
        private void ScrollView_ScrollEnd(object? sender, RoutedEventArgs e)
        {
            MessageBox.Show("tips", "ScrollView -> Scroll End");
        }
        private void OnPopupClick(object sender, RoutedEventArgs e)
        {
            Collection.Clear();
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
            var text = ((Button)sender).CommandParameter.ToInt32();
            ShowPosition position = ShowPosition.BottomLeft;
            ScollOrientation orientation = ScollOrientation.Horizontal;
            switch (text)
            {
                case 1:
                    {
                        position = ShowPosition.TopLeft;
                        orientation = ScollOrientation.Horizontal;
                        break;
                    }
                case 2:
                    {
                        position = ShowPosition.TopRight;
                        orientation = ScollOrientation.Horizontal;
                        break;
                    }
                case 3:
                    {
                        position = ShowPosition.BottomLeft;
                        orientation = ScollOrientation.Horizontal;
                        break;
                    }
                case 4:
                    {
                        position = ShowPosition.BottomRight;
                        orientation = ScollOrientation.Horizontal;
                        break;
                    }
                case 5:
                    {
                        position = ShowPosition.BottomLeft;
                        orientation = ScollOrientation.Vertical;
                        break;
                    }
                case 6:
                    {
                        position = ShowPosition.BottomRight;
                        orientation = ScollOrientation.Vertical;
                        break;
                    }
            }
            NotifyWindow window = new NotifyWindow();
            window.Content = new TextBlock { Text = "大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大大" };
            var options = new Options(position, new Size(160, 60), orientation);
            window.Show(options);
        }
    }
}