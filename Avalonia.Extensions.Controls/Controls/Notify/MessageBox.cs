using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public sealed class MessageBox : Window
    {
        private Graphics Graphic { get; }
        public MessageBox() : base()
        {
            Width = 400;
            Height = 80;
            CreateControls();
            if (PlatformImpl != null)
                Graphic = Graphics.FromHwnd(PlatformImpl.Handle.Handle);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Graphic.Dispose();
        }
        private void CreateControls()
        {
            Grid root = new Grid
            {
                RowDefinitions = RowDefinitions.Parse("*,*"),
                ColumnDefinitions = ColumnDefinitions.Parse("*,*")
            };
            var tb = new TextBlock
            {
                Name = "Message",
                TextWrapping = TextWrapping.Wrap
            };
            root.Children.Add(tb);
            Grid.SetColumn(tb, 0);
            Grid.SetRow(tb, 0);
            Grid.SetColumnSpan(tb, 2);
            var ok = new Button
            {
                Name = "Ok",
                HorizontalAlignment = Layout.HorizontalAlignment.Center,
                HorizontalContentAlignment = Layout.HorizontalAlignment.Center
            };
            root.Children.Add(ok);
            Grid.SetColumn(ok, 0);
            Grid.SetRow(ok, 1);
            var cancel = new Button
            {
                Name = "Cancel",
                HorizontalAlignment = Layout.HorizontalAlignment.Center,
                HorizontalContentAlignment = Layout.HorizontalAlignment.Center
            };
            root.Children.Add(cancel);
            Grid.SetColumn(cancel, 1);
            Grid.SetRow(cancel, 1);
            this.Content = root;
        }
        public void SetSize(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }
        public SizeF ContentSize(string content)
        {
            return Graphic.MeasureString(content, new Font("Arial", 16));
        }
        public static Task<bool?> Show(string title, string message)
        {
            return Show(null, title, message);
        }
        public static Task<bool?> Show(Window parent, string title, string message,
            string okContent = "确定", string cancelContent = "取消")
        {
            bool? result = null;
            MessageBox messageBox = new MessageBox { Title = title };
            var logicals = messageBox.GetLogicalDescendants();
            var txtMessage = logicals.OfType<TextBlock>().FirstOrDefault(x => x.Name == "Message");
            txtMessage.Text = message;
            var btnOk = logicals.OfType<Button>().FirstOrDefault(x => x.Name == "Ok");
            btnOk.Content = okContent;
            var btnCancel = logicals.OfType<Button>().FirstOrDefault(x => x.Name == "Cancel");
            btnCancel.Content = cancelContent;
            btnOk.Click += (s, ee) =>
            {
                result = true;
                messageBox.Close();
            };
            btnCancel.Click += (s, ee) =>
            {
                result = false;
                messageBox.Close();
            };
            var tcs = new TaskCompletionSource<bool?>();
            messageBox.Closed += delegate { tcs.TrySetResult(result); };
            if (parent != null)
            {
                int x = parent.Position.X + 200, y = parent.Position.Y + 40;
                messageBox.Position = new PixelPoint(x, y);
                messageBox.ShowDialog(parent);
            }
            else
                messageBox.Show();
            AutoSize(message, messageBox, txtMessage);
            return tcs.Task;
        }
        private static void AutoSize(string message, MessageBox messageBox, TextBlock txtMessage)
        {
            var size = messageBox.ContentSize(message);
            var rows = Math.Ceiling(size.Width / txtMessage.ActualWidth());
            var height = size.Height * (rows - 1) * 1.2;
            messageBox.Height += height;
        }
    }
}