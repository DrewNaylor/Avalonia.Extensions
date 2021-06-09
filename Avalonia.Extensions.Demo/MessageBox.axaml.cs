using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Demo
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
            Width = 400;
            Height = 80;
            ShowInTaskbar = false;
        }
        public static Task<bool?> Show(string title, string message)
        {
            return Show(null, title, message);
        }
        public static Task<bool?> Show(Window parent, string title, string message)
        {
            bool? result = null;
            MessageBox messageBox = new MessageBox { Title = title };
            messageBox.FindControl<TextBlock>("Message").Text = message;
            var btnOk = messageBox.FindControl<Button>("Ok");
            var btnCancel = messageBox.FindControl<Button>("Cancel");
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
            return tcs.Task;
        }
    }
}