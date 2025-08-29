using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EDShyrka.Logging;
using EDShyrka.UI.ViewModels;
using EDShyrka.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
		var mainViewModel = ServiceProvider!.GetRequiredService<MainViewModel>();
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

	private void ConfigureServices()
	{
		var collection = new ServiceCollection();
		collection.AddLogging(o => o.ConfigureLogging());
		collection.AddSingleton<MainViewModel>();
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
