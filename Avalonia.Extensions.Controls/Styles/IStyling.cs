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
                    string typeName = GetType().Name, url =
                        $"avares://Avalonia.Extensions.Theme/{typeName}.xaml";
                    if (!control.Resources.ContainsKey(typeName))
                        control.Resources.Add(typeName, url.AsResource());
                    control.ApplyTheme(typeName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}