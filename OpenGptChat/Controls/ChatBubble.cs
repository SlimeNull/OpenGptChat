using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenGptChat.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls;assembly=OpenGptChat.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChatBubble/>
    ///
    /// </summary>
    public class ChatBubble : Control
    {
        static ChatBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatBubble), new FrameworkPropertyMetadata(typeof(ChatBubble)));
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public Brush ContentForeground
        {
            get { return (Brush)GetValue(ContentForegroundProperty); }
            set { SetValue(ContentForegroundProperty, value); }
        }

        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public bool IsReadonly
        {
            get { return (bool)GetValue(IsReadonlyProperty); }
            set { SetValue(IsReadonlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Username.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(nameof(Username), typeof(string), typeof(ChatBubble), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(ChatBubble), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for HeaderForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(ChatBubble), new PropertyMetadata(Brushes.Gray));

        // Using a DependencyProperty as the backing store for HeaderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(ChatBubble), new PropertyMetadata(Brushes.Transparent));

        // Using a DependencyProperty as the backing store for ContentForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentForegroundProperty =
            DependencyProperty.Register(nameof(ContentForeground), typeof(Brush), typeof(ChatBubble), new PropertyMetadata(Brushes.Black));

        // Using a DependencyProperty as the backing store for ContentBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(ChatBubble), new PropertyMetadata(Brushes.White));

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ChatBubble), new PropertyMetadata(new CornerRadius(0)));

        // Using a DependencyProperty as the backing store for IsReadonly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadonlyProperty =
            DependencyProperty.Register(nameof(IsReadonly), typeof(bool), typeof(ChatBubble), new PropertyMetadata(true));
    }
}
