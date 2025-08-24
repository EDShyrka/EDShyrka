using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EDShyrka.Logging;
using EDShyrka.UI.ViewModels;
using EDShyrka.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EDShyrka.UI;

public partial class App : Application
{
	private ServiceProvider _services;

	public override void Initialize()
	{
		ConfigureServices();
		AvaloniaXamlLoader.Load(this);
	}

	private void ConfigureServices()
	{
		var collection = new ServiceCollection();
		collection.AddLogging(o => o.ConfigureLogging());
		collection.AddSingleton<MainViewModel>();
		_services = collection.BuildServiceProvider();
	}

	public override void OnFrameworkInitializationCompleted()
	{
		var mainViewModel = _services.GetRequiredService<MainViewModel>();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
			// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
			DisableAvaloniaDataAnnotationValidation();

			desktop.MainWindow = new MainWindow
			{
				DataContext = mainViewModel
			};
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			singleViewPlatform.MainView = new MainView
			{
				DataContext = mainViewModel
			};
		}

		base.OnFrameworkInitializationCompleted();
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
}