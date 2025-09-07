using Avalonia;
using Avalonia.Markup.Xaml;
using System;

namespace EDShyrka.UI.ViewModels
{
    /// <summary>
    /// Markup extension responsible for providing a viewmodel instance.
    /// </summary>
    public class ViewModelProvider : MarkupExtension
    {
        #region ctor
		/// <summary>
		/// Initializes a <see cref="ViewModelProvider"/> instance.
		/// </summary>
		/// <param name="viewModelType">The type of requested viewmodel.</param>
		public ViewModelProvider(Type viewModelType)
            : this(viewModelType, viewModelType)
		{
		}

		/// <summary>
		/// Initializes a <see cref="ViewModelProvider"/> instance.
		/// </summary>
		/// <param name="viewModelType">The type of requested viewmodel.</param>
		public ViewModelProvider(Type viewModelType, Type designViewModelType)
		{
			ViewModelType = viewModelType;
			DesignViewModelType = designViewModelType;
		}
		#endregion ctor

		#region properties
		/// <summary>
		/// The type of viewmodel for runtime.
		/// </summary>
		public Type ViewModelType { get; set; }

        /// <summary>
        /// The type of viewmodel for design time.
        /// </summary>
        public Type DesignViewModelType { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Provide the instance of viewmodel.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>A viewmodel instance.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (AppHelpers.IsDesignMode)
                return Activator.CreateInstance(DesignViewModelType)!;

			return ((App)Application.Current!).ServiceProvider!.GetService(ViewModelType)!;
        }
        #endregion methods
    }
}
