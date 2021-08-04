using Avalonia.Controls;
using Avalonia.Extensions.Controls;
using Avalonia.Styling;
using System;
using System.Diagnostics;

namespace Avalonia.Extensions.Styles
{
    public interface IStyling : IStyleable
    {
        void AddResource()
        {
            try
            {
                if (this is Control control)
                {
                    var typeName = GetType().Name;
                    if (!control.Resources.ContainsKey(typeName))
                        control.Resources.Add(typeName, $"avares://Avalonia.Extensions.Theme/{typeName}.xaml".AsResource());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}