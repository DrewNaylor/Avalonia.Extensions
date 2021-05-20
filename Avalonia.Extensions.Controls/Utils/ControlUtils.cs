using Avalonia.Controls;
using System;
using System.Reflection;

namespace Avalonia.Extensions.Controls
{
    public static class ControlUtils
    {
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
    }
}