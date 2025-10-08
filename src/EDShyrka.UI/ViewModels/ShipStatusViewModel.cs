using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDShyrka.UI.ViewModels
{
	public partial class ShipStatusViewModel : ViewModelBase
	{
		[RelayCommand]
		private void ShipStatus()
		{
			System.Diagnostics.Debug.WriteLine("Test");
		}
	}
}
