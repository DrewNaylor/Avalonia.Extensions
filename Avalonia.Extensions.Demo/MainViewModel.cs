using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Avalonia.Controls.Demo
{
    public sealed class MainViewModel : ReactiveObject, IDisposable
    {
        private string message;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }
        private ObservableCollection<object> items;
        public ObservableCollection<object> Items
        {
            get => items;
            private set => this.RaiseAndSetIfChanged(ref items, value);
        }
        public ReactiveCommand<object, Unit> OnButtonClick { get; }
        public ReactiveCommand<object, Unit> OnItemClick { get; }
        public MainViewModel()
        {
            OnItemClick= ReactiveCommand.Create<object>(ItemClick);
            OnButtonClick = ReactiveCommand.Create<object>(ButtonClick);
            items = new ObservableCollection<object>{
                new { content="111111"},
                new { content="222222"},
                new { content="333333"},
                new { content="444444"},
                new { content="555555"},
                new { content="666666"},
                new { content="777777"},
                new { content="888888"},
                new { content="999999"},
                new { content="000000"},
                new { content="111111"},
                new { content="222222"},
                new { content="333333"},
                new { content="444444"},
                new { content="555555"},
                new { content="666666"},
                new { content="777777"},
                new { content="888888"},
                new { content="999999"},
                new { content="000000"}};
        }
        private void ItemClick(object obj)
        {
            Message = "你点击了CellListView , CommandParameter :" + obj;
        }
        private void ButtonClick(object obj)
        {
            Message = "你点击了ItemsRepeaterContent , CommandParameter :" + obj;
        }
        public void Dispose()
        {

        }
    }
}