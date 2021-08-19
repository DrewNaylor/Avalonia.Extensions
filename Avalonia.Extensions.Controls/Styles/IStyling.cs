using Avalonia.Controls;
using Avalonia.Extensions.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace Avalonia.Extensions.Styles
{
    public interface IStyling : IStyleable
    {
        void AddResource()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                try
                {
                    if (this is Control control)
                    {
                        string typeName = GetType().Name,
                        sourceUrl = $"avares://Avalonia.Extensions.Controls/Styles/Xaml/{typeName}.xml";
                        var sourceUri = new Uri(sourceUrl);
                        if (!control.Resources.ContainsKey(typeName) && Core.Instance.InnerClasses.Contains(sourceUri))
                        {
                            control.Resources.Add(typeName, sourceUrl.AsResource());
                            control.ApplyTheme(sourceUri);
                        }
                    }
                    ApplyGridViewStyle();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }
        /// <summary>
        /// GridView style will sometimes fail
        /// </summary>
        void ApplyGridViewStyle()
        {
            if (this is GridView)
            {
                var target = AvaloniaRuntimeXamlLoader.Parse<ItemsPanelTemplate>(
                    "<ItemsPanelTemplate xmlns='https://github.com/avaloniaui'><WrapPanel Orientation=\"Horizontal\"/></ItemsPanelTemplate>");
                SetValue(ItemsControl.ItemsPanelProperty, target);
            }
        }
    }
}