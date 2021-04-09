using Avalonia.Styling;

namespace Avalonia.Controls.Extensions.Presenters
{
    public interface IColumnsPresenterHost : ITemplatedControl, IAvaloniaObject
    {
        void RegisterItemsPresenter(IColumnsPresenter presenter);
    }
}