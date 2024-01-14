using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace DemoQueryStringParameterIssue;


public class Tecnico{}



public interface IViewModel {
    
    IJSRuntime JsRuntime { get; } 
    
    ValueTask<bool> ShowJsDialogAsync(string msg);
}

public abstract class ViewModel: IViewModel {
    protected ViewModel(IJSRuntime jsRuntime) {
        JsRuntime = jsRuntime;
    }

    public IJSRuntime JsRuntime { get; private set; }
    
    public ValueTask<bool> ShowJsDialogAsync(string msg) {
        return JsRuntime.InvokeAsync<bool>("confirm", msg);
    }

}

public interface IViewModelComContextoValidacao<T> : IViewModel, IDisposable where T : class, new( ) {
    
    IFluentValidationEditContext<T> EditContext { get; }

    T Model { get; }
   
}


public abstract class ViewModelWithValidationContext<T> : ViewModel, IViewModelComContextoValidacao<T> where T : class, new( ) {
    private IFluentValidationEditContext<T> _contexto;
    private bool _disposed = false;

    protected ViewModelWithValidationContext(IJSRuntime jsRuntime,
                                            IFluentValidationEditContext<T> contexto) : base(jsRuntime) {
        _contexto = contexto;
    }

    public IFluentValidationEditContext<T> EditContext => _contexto;

    public T Model => EditContext.Data;

    public void Dispose() {
        Dispose(true);
    }


    protected virtual void Dispose(bool disposing) {
        if( _disposed ) {
            return;
        }

        if( disposing ) {
            _contexto.Dispose( );
        }

        _disposed = true;
    }

}
