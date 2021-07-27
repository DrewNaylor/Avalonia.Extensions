using Avalonia.Controls;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace Avalonia.Extensions.Controls
{
    public static class ControlUtils
    {
        private const double Epsilon = 0.00000153; 
        public static T GetPrivateField<T>(this Control control, string fieldName)
        {
            try
            {
                var type = control.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
                FieldInfo field = type.GetField(fieldName, flag);
                return (T)field?.GetValue(control);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T GetPrivateField<T>(this NameScope scope, string fieldName)
        {
            try
            {
                var type = scope.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
                FieldInfo field = type.GetField(fieldName, flag);
                return (T)field?.GetValue(scope);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SetPrivateField(this Control control, string fieldName, object fieldValue)
        {
            try
            {
                var type = control.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var field = type.GetField(fieldName, flag);
                field?.SetValue(control, fieldValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T GetPrivateProperty<T>(this Control control, string propertyName)
        {
            try
            {
                var type = control.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic| BindingFlags.Public;
                var property = type.GetProperty(propertyName, flag);
                return (T)property?.GetValue(control);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SetPrivateProperty(this Control control, string propertyName, object propertyValue)
        {
            try
            {
                var type = control.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var property = type.GetProperty(propertyName, flag);
                property?.SetValue(control, propertyValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static bool AreClose(Size size1, Size size2)
        {
            return AreClose(size1.Width, size2.Width) && AreClose(size1.Height, size2.Height);
        }
        public static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
                return true;
            double delta = value1 - value2;
            return (delta < Epsilon) && (delta > -Epsilon);
        }
        public static SizeF MeasureString(this IWindowImpl impl, string content, Font font, float maxWidth = 0)
        {
            if (impl != null)
            {
                var graphic = Graphics.FromHwnd(impl.Handle.Handle);
                StringFormat sf = StringFormat.GenericTypographic;
                sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                if (maxWidth != 0)
                    return graphic.MeasureString(content.Trim(), font, new SizeF(maxWidth, 0), sf);
                else
                    return graphic.MeasureString(content.Trim(), font, PointF.Empty, sf);
            }
            return default;
        }
        public static SizeF MeasureString(this string text, Font font, float maxwidth)
        {
            var p = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(text, font,
                Convert.ToInt32(maxwidth * 96f / 100f));
            return new SizeF(p.Width * 100f / 96f, p.Height * 100f / 96f);
        }
        public static object InvokePrivateMethod(this Control control, string methodName, object[] parameters = null)
        {
            try
            {
                var type = control.GetType();
                MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic
                    | BindingFlags.Instance);
                if (methodInfo == null && type.BaseType != null)
                    methodInfo = type.BaseType.GetMethod(methodName, BindingFlags.NonPublic
                    | BindingFlags.Instance);
                return methodInfo?.Invoke(control, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<T> FindControls<T>(this Panel control, bool isLoop = false) where T : Control
        {
            Contract.Requires<ArgumentNullException>(control != null);
            foreach (var childControl in control.Children)
            {
                if (childControl is T obj)
                    yield return obj;
                if (childControl is Panel panel)
                {
                    var array = panel.FindControls<T>(isLoop);
                    foreach (var item in array)
                        yield return item;
                }
            }
        }
        public static void InitStyle(this IStyling styling)
        {
            try
            {
                styling.AddResource();
            }
            catch { }
        }
        internal static Window GetWindow(this IControl control, out bool hasBase)
        {
            hasBase = false;
            Window window = null;
            try
            {
                IControl parent = control.Parent;
                while (window == null)
                {
                    if (parent.Parent is Window win)
                    {
                        window = win;
                        hasBase = win is WindowBase windowBase && windowBase.Locate;
                    }
                    else
                        parent = parent.Parent;
                }
            }
            catch { }
            return window;
        }
    }
}