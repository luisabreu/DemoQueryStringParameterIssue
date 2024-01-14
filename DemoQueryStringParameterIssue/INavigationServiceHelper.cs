using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace DemoQueryStringParameterIssue;

// added for testing view models without dependency on the blazor framework
public interface INavigationServiceHelper {
    public string GetUriWithQueryParameters(IReadOnlyDictionary<string, object?> parameters);

    void Navigate(string uri, bool forceLoad = false, bool replace = false);

    IDisposable RegisterHandlerForUrlNavigationChecking(Func<LocationChangingContext, ValueTask> handler);

}

public sealed class NavigationServiceHelper:INavigationServiceHelper {
    private readonly NavigationManager _navigationManager;

    public NavigationServiceHelper(NavigationManager navigationManager) {
        _navigationManager = navigationManager;
    }

    public string GetUriWithQueryParameters(IReadOnlyDictionary<string, object?> parameters) {
        return _navigationManager.GetUriWithQueryParameters(parameters);
    }

    public void Navigate(string uri, bool forceLoad = false, bool replace = false) {
        _navigationManager.NavigateTo(uri, forceLoad, replace);
    }

    public IDisposable RegisterHandlerForUrlNavigationChecking(Func<LocationChangingContext, ValueTask> handler) {
        return _navigationManager.RegisterLocationChangingHandler(handler);
    }

}
