using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace OpenGptChat.Views.Controls;

public partial class ChatMessage : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ChatMessage, string>(nameof(Title), defaultValue: "Title");

    public static readonly StyledProperty<string> MessageTextProperty =
        AvaloniaProperty.Register<ChatMessage, string>(nameof(MessageText), defaultValue: "Content");

    public static readonly StyledProperty<IBrush> MessageForegroundProperty =
        AvaloniaProperty.Register<ChatMessage, IBrush>(nameof(MessageForeground));

    public static readonly StyledProperty<IBrush> MessageBackgroundProperty =
        AvaloniaProperty.Register<ChatMessage, IBrush>(nameof(MessageBackground));

    public static readonly StyledProperty<Thickness> MessagePaddingProperty =
        AvaloniaProperty.Register<ChatMessage, Thickness>(nameof(MessagePadding), defaultValue: new Thickness(5, 0));

    public static readonly StyledProperty<Thickness> MessageBorderThicknessProperty =
        AvaloniaProperty.Register<ChatMessage, Thickness>(nameof(MessageBorderThickness), defaultValue: new Thickness(1));

    public static readonly StyledProperty<IBrush> MessageBorderBrushProperty =
        AvaloniaProperty.Register<ChatMessage, IBrush>(nameof(MessageBorderBrush), defaultValue: Brushes.Gray);

    public static readonly StyledProperty<CornerRadius> MessageCornerRadiusProperty =
        AvaloniaProperty.Register<ChatMessage, CornerRadius>(nameof(MessageCornerRadius), defaultValue: new CornerRadius(3));

    public static readonly StyledProperty<double> SpacingProperty =
        StackPanel.SpacingProperty.AddOwner<ChatMessage>();



    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string MessageText
    {
        get => GetValue(MessageTextProperty);
        set => SetValue(MessageTextProperty, value);
    }

    public IBrush MessageForeground
    {
        get => GetValue(MessageForegroundProperty);
        set => SetValue(MessageForegroundProperty, value);
    }

    public IBrush MessageBackground
    {
        get => GetValue(MessageBackgroundProperty);
        set => SetValue(MessageBackgroundProperty, value);
    }

    public Thickness MessagePadding
    {
        get => GetValue(MessagePaddingProperty);
        set => SetValue(MessagePaddingProperty, value);
    }

    public Thickness MessageBorderThickness
    {
        get => GetValue(MessageBorderThicknessProperty);
        set => SetValue(MessageBorderThicknessProperty, value);
    }

    public IBrush MessageBorderBrush
    {
        get => GetValue(MessageBorderBrushProperty);
        set => SetValue(MessageBorderBrushProperty, value);
    }

    public CornerRadius MessageCornerRadius
    {
        get => GetValue(MessageCornerRadiusProperty);
        set => SetValue(MessageCornerRadiusProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public ChatMessage()
    {
        InitializeComponent();
    }
}