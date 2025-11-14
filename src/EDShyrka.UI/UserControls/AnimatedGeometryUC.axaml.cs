using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace UserControls;

public partial class AnimatedGeometryUC : UserControl
{
    public AnimatedGeometryUC()
    {
        InitializeComponent();
	}

	public static readonly StyledProperty<Color> Color01Property =
	AvaloniaProperty.Register<AnimatedGeometryUC, Color>(nameof(Color01), Colors.Azure);

	public Color Color01
	{
		get => GetValue(Color01Property);
		set => SetValue(Color01Property!, value);
	}

	public static readonly StyledProperty<Color> Color02Property =
		AvaloniaProperty.Register<AnimatedGeometryUC, Color>(nameof(Color02), Colors.Azure);

	public Color Color02
	{
		get => GetValue(Color02Property);
		set => SetValue(Color02Property!, value);
	}

	public static readonly StyledProperty<Color> Color03Property =
		AvaloniaProperty.Register<AnimatedGeometryUC, Color>(nameof(Color03), Colors.Azure);

	public Color Color03
	{
		get => GetValue(Color03Property);
		set => SetValue(Color03Property!, value);
	}


}
