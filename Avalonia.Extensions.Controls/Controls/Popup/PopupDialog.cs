using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Timers;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// popup control with auto close when timeout
    /// </summary>
    public class PopupDialog : Popup
    {
        private Timer Timer { get; }
        public PopupDialog(PopupLength length) : base()
        {
            Timer = new Timer((int)length);
            Timer.Elapsed += Timer_Elapsed;
        }
        /// <summary>
        /// open popup
        /// </summary>
        /// <param name="message">text 4 show</param>
        public void Open(string message)
        {
            if (Child is TextBlock tb)
                tb.Text = message;
            else
                Child = new TextBlock { Text = message };
            Open();
            Timer.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Timer.Stop();
                Close();
            }
            catch { }
        }
    }
}