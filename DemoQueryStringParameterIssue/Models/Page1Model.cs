using System.Data;
using FluentValidation;

namespace DemoQueryStringParameterIssue.Models;

public sealed class Page1Model {
    public string? Info { get; set; }
}

public sealed partial class Page1ModelValidator : AbstractValidator<Page1Model> {
    public Page1ModelValidator() {
        RuleFor(m => m.Info)
            .NotEmpty( );
    }
}
