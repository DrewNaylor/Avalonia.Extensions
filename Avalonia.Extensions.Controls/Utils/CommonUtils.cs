using System;

namespace Avalonia.Extensions.Controls
{
    public static class CommonUtils
    {
        public static bool SmallerThan(this PixelPoint pixelPoint, PixelPoint point, bool inCludeEquals = false)
        {
            if (inCludeEquals && pixelPoint.X <= point.X && pixelPoint.Y <= point.Y)
                return true;
            else
                return pixelPoint.X < point.X && pixelPoint.Y < point.Y;
        }
        public static bool BiggerThan(this PixelPoint pixelPoint, PixelPoint point, bool inCludeEquals = false)
        {
            if (inCludeEquals && pixelPoint.X >= point.X && pixelPoint.Y >= point.Y)
                return true;
            else
                return pixelPoint.X > point.X && pixelPoint.Y > point.Y;
        }
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