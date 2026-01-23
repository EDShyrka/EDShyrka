using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EDShyrka.Shared;
using EDShyrka.UI.DataTemplates;
using EDShyrka.UI.Models;
using EDShyrka.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly ObservableCollection<ViewDescription> _items;

	public MainViewModel()
	{
		_items = new()
			{
				new("Test"),
				new ("Test01"),
				new ("Test02"),
			};

		CurrentItem = _items.First();
		NextCommand = new RelayCommand(NextItem);
		PreviousCommand = new RelayCommand(PreviousItem);
	}

	public IEnumerable Items { get => _items; }

	[ObservableProperty]
	private ViewDescription _currentItem;

	public ICommand NextCommand { get; }
	public ICommand PreviousCommand { get; }

	private void NextItem()
	{
		int currentIndex = _items.IndexOf(CurrentItem);
		int nextIndex = (currentIndex + 1) % _items.Count;
		CurrentItem = _items[nextIndex];
	}

	private void PreviousItem()
	{
		int currentIndex = _items.IndexOf(CurrentItem);
		int previousIndex = (currentIndex - 1 + _items.Count) % _items.Count;
		CurrentItem = _items[previousIndex];
	}
}
