using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace DemoQueryStringParameterIssue;


public interface IFluentValidationEditContext<out T> : IDisposable where T: class, new() {
    EditContext EditContext { get; }

    public bool Modificado { get; }

    T Data { get; }

    ValidationMessageStore ValidationMessageStore { get; }

}

public sealed class FluentValidationEditContext<T> : IFluentValidationEditContext<T> where T : class, new( ) {
    private readonly EditContext _editContext;
    private readonly ValidationMessageStore _messageStore;
    private readonly IValidator<T> _validator;
    private readonly T _data;

    public FluentValidationEditContext(IValidator<T> validator, T? state = null) {
        _data = state ?? new T( );
        _validator = validator;


        _editContext = new EditContext(_data);
        _editContext.OnFieldChanged += HandleFieldValidation;

        _editContext.OnValidationRequested += HandleModelValidation;

        _messageStore = new ValidationMessageStore(_editContext);
    }

    public T Data => _data;

    public EditContext EditContext => _editContext;

    public bool Modificado => _editContext.IsModified( );

    public ValidationMessageStore ValidationMessageStore => _messageStore;


    private void HandleModelValidation(object? sender, ValidationRequestedEventArgs e) {
        _messageStore.Clear( );
        ValidationResult result = _validator.Validate(_data);
        if( !result.IsValid ) {
            foreach( ValidationFailure? error in result.Errors ) {
                _messageStore.Add(_editContext.Field(error.PropertyName), error.ErrorMessage);
            }
        }
        
        _editContext.NotifyValidationStateChanged( );
    }

    private void HandleFieldValidation(object? sender, FieldChangedEventArgs e) {
        // _logger.LogDebug("Validating field {Field}", e.FieldIdentifier.FieldName);
        _messageStore.Clear(e.FieldIdentifier);

        string propertyName = e.FieldIdentifier.FieldName;
        ValidationResult result = _validator.Validate(_data, options => options.IncludeProperties(propertyName));
        // _logger.LogDebug("Property {fieldName} valid: {valid}", e.FieldIdentifier.FieldName, result.IsValid);
        if( !result.IsValid ) {
            _messageStore.Add(e.FieldIdentifier, result.Errors.Select(e => e.ErrorMessage).ToList( ));
        }

        _editContext.NotifyValidationStateChanged( );
    }

    public void Dispose() {
        if( _editContext is not null ) {
            _editContext.OnFieldChanged -= HandleFieldValidation;
            _editContext.OnValidationRequested -= HandleModelValidation;
        }
    }

    public void LimpaMensagensErro() {
        _messageStore.Clear();
    }


}
