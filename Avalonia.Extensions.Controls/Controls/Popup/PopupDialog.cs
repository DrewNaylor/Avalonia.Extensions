using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// popup control with auto close when timeout
    /// </summary>
    public class PopupDialog : Popup
    {
        private Panel Panel { get; }
        private int Timeout { get; set; }
        /// <summary>
        /// create a PopupDialog instance
        /// </summary>
        /// <param name="panel">showin that layout</param>
        /// <param name="timeout">1k as 1 sencond</param>
        public PopupDialog(Panel panel, int timeout) : base()
        {
            Contract.Requires<ArgumentNullException>(panel != null);
            Panel = panel;
            Timeout = timeout;
            Panel.Children.Add(this);
        }
        /// <summary>
        /// create a PopupDialog instance
        /// </summary>
        /// <param name="panel">showin that layout</param>
        public PopupDialog(Panel panel, PopupLength length = PopupLength.Default) : base()
        {
            Contract.Requires<ArgumentNullException>(panel != null);
            Panel = panel;
            Timeout = (int)length;
            Panel.Children.Add(this);
        }
        private async void Hidden()
        {
            try
            {
                await Task.Delay(Timeout);
                Close();
                Panel.Children.Remove(this);
            }
            catch { }
        }
        /// <summary>
        /// show popup
        /// </summary>
        /// <param name="message">text 2 show</param>
        public void Open(string message)
        {
            message ??= string.Empty;
            if (Child is TextBlock tb)
                tb.Text = message;
            else
                Child = new TextBlock { Text = message };
            Open();
        }
        public new void Open()
        {
            base.Open();
            Hidden();
        }
    }
}