using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EDShyrka.UI.Models;
using EDShyrka.UI.Services;
using EDShyrka.UI.ViewModels;
using EDShyrka.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EDShyrka.UI;

public partial class App : Application
{
	#region properties
	/// <summary>
	/// Provide access to <see cref="IServiceProvider"/> instance.
	/// </summary>
	public IServiceProvider? ServiceProvider { get; private set; }
	#endregion properties

	#region methods
	public override void Initialize()
	{
		ConfigureServices();
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		var appSettings = ServiceProvider.GetRequiredService<AppSettings>();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
			// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
			DisableAvaloniaDataAnnotationValidation();
			appSettings.ServerLocation = desktop.Args.FirstOrDefault();
			desktop.MainWindow = new MainWindow();
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			appSettings.ServerLocation = JSInterop.getHostAddress();
			singleViewPlatform.MainView = new MainView();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private void ConfigureServices()
	{
		var appSettings = new AppSettings();
		var collection = new ServiceCollection()
			.AddSingleton(appSettings)
			.AddSingleton<MainViewModel>()
			.AddSingleton<CommunicationService>();
		ServiceProvider = collection.BuildServiceProvider();
	}

	private void DisableAvaloniaDataAnnotationValidation()
	{
		// Get an array of plugins to remove
		var dataValidationPluginsToRemove =
			BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

		// remove each entry found
		foreach (var plugin in dataValidationPluginsToRemove)
		{
			BindingPlugins.DataValidators.Remove(plugin);
		}
	}
	#endregion methods
}
