using FluentValidation;

namespace DemoQueryStringParameterIssue.Models;

public sealed class Page2Model {
    public Page2Model() {
        var stop = "";
    }

    private string? _info;

    public string? Info {
        get => _info;
        set {
            _info = value; 
            
        }
    }
}

public sealed partial class Page2ModelValidator : AbstractValidator<Page2Model> {
    public Page2ModelValidator() {
        RuleFor(m => m.Info)
            .NotEmpty( );
    }
}
