using System;

namespace Avalonia.Extensions.Controls
{
    public static class CommonUtils
    {
        public static int ToInt32(this object obj)
        {
            if (obj is int result)
                return result;
            else
            {
                if (int.TryParse(obj.ToString(), out result))
                    return result;
                else
                    return Convert.ToInt32(obj.ToString());
            }
        }
        public static double ActualWidth(this Visual visual)
        {
            return visual.Bounds.Width;
        }
        public static double ActualHeight(this Visual visual)
        {
            return visual.Bounds.Height;
        }
    }
}