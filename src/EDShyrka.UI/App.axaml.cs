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
	public IServiceProvider ServiceProvider { get; init; } = ConfigureServices();
	#endregion properties

	#region methods
	public override void Initialize()
	{
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
			desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			appSettings.ServerLocation = JSInterop.getHostAddress();
			singleViewPlatform.MainView = ServiceProvider.GetRequiredService<MainView>();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private static IServiceProvider ConfigureServices()
	{
		var collection = new ServiceCollection()
			.AddSingleton<AppSettings>()
			.AddSingleton<CommunicationService>()
			.AddSingleton<MainWindow>()
			.AddTransient<MainView>()
			.AddSingleton<MainViewModel>()
			.AddTransient<ShipStatusView>()
			.AddTransient<ShipStatusViewModel>()
			;
		return collection.BuildServiceProvider();
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
