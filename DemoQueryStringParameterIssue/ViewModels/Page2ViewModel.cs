using DemoQueryStringParameterIssue.Models;
using Microsoft.JSInterop;

namespace DemoQueryStringParameterIssue.ViewModels;

public interface IPage2ViewModel : IViewModelComContextoValidacao<Page2Model> {
    Task UpdateFromParametersAsync(string? info);

    Task SearchAndUpdateUriAsync();
}

public sealed class Page2ViewModel: ViewModelWithValidationContext<Page2Model>, IPage2ViewModel {
    public Page2ViewModel(IJSRuntime jsRuntime, 
                          IFluentValidationEditContext<Page2Model> contexto,
                          INavigationServiceHelper navigationService) : base(jsRuntime, contexto) {
        _navigationService = navigationService;
    }

    private bool _first = true;
    private readonly INavigationServiceHelper _navigationService;

    public Task UpdateFromParametersAsync(string? info) {
        if( !_first ) {
            return Task.CompletedTask;
        }

        _first = false;

        Model.Info = info;
        
        return Task.CompletedTask;
    }

    public Task SearchAndUpdateUriAsync() {
        // this would run a search by calling a rest web service
        // and then we'd update the uri so that this search can be shared
        // with other users if required


        UpdateUri( );
        return Task.CompletedTask;
    }

    private void UpdateUri() {
        Dictionary<string, object?> qs = new( ) {
                                                    ["id"] = string.IsNullOrEmpty(Model.Info) ? null : Model.Info
                                                };
        string uri = _navigationService.GetUriWithQueryParameters(qs);
        _navigationService.Navigate(uri);
    }
}
