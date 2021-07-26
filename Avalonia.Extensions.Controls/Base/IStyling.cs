using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Diagnostics;

namespace Avalonia.Extensions.Controls
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
                    {
                        var sourceUri = new Uri($"avares://Avalonia.Extensions.Theme/{typeName}.xaml");
                        control.Resources.Add(typeName, sourceUri.AsResource());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}