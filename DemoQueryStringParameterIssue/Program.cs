using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DemoQueryStringParameterIssue;
using DemoQueryStringParameterIssue.Models;
using DemoQueryStringParameterIssue.ViewModels;
using FluentValidation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<INavigationServiceHelper, NavigationServiceHelper>( );

builder.Services.AddTransient(typeof( IFluentValidationEditContext<> ), typeof( FluentValidationEditContext<> ));

builder.Services.AddSingleton<IValidator<Page1Model>, Page1ModelValidator>( );
builder.Services.AddSingleton<IValidator<Page2Model>, Page2ModelValidator>( );

builder.Services.AddTransient<IPage1ViewModel, Page1ViewModel>( );
builder.Services.AddTransient<IPage2ViewModel, Page2ViewModel>( );


await builder.Build( ).RunAsync( );
