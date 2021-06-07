using Avalonia.Controls;
using System;
using System.Collections.Generic;
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
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
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
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
                var property = type.GetProperty(propertyName, flag);
                return (T)property?.GetValue(control);
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
        public static void SetPrivateProperty(this Control control, string propertyName, object propertyValue)
        {
            try
            {
                var type = control.GetType();
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
                var property = type.GetProperty(propertyName, flag);
                property?.SetValue(control, propertyValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static object InvokePrivateMethod(this Control control, string methodName, object[] parameters = null)
        {
            try
            {
                var type = control.GetType();
                MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic
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
    }
}