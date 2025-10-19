using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDShyrka.UI.DataTemplates;

public class ViewDescriptionTemplate : IDataTemplate
{
	private const string viewsNamespace = "EDShyrka.UI.Views";

	private readonly IServiceProvider _serviceProvider;

	public ViewDescriptionTemplate(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public Control? Build(object? param)
	{
		if (param is ViewDescription viewDesc)
		{
			var viewTypeName = $"{viewsNamespace}.{viewDesc.Name}View";
			var types = GetViewTypes();
			var viewType = types.FirstOrDefault(o => o.FullName == viewTypeName);
			if (viewType is not null)
			{
				return _serviceProvider.GetService(viewType)! as Control;
			}
		}
		return new TextBlock { Text = $"{GetType().Name} could not resolve the template" };
	}

	public bool Match(object? data)
	{
		return data is ViewDescription;
	}

	private IEnumerable<Type> GetViewTypes()
	{
		return AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(o => o.ExportedTypes)
			.Where(o => o.Namespace == viewsNamespace);
	}
}

public record ViewDescription(string Name);
