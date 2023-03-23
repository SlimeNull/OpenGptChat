using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenChat.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenChat.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenChat.Controls;assembly=OpenChat.Controls"
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
    ///     <MyNamespace:NoteControl/>
    ///
    /// </summary>
    public class NoteControl : Control
    {
        static NoteControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NoteControl), new FrameworkPropertyMetadata(typeof(NoteControl)));
        }

        public NoteControl()
        {
            DependencyPropertyDescriptor.FromProperty(ShowProperty, typeof(NoteControl)).AddValueChanged(this, (s, e) =>
            {
                ContentRenderTransform = translateTransform;
                Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
                CircleEase ease = new CircleEase() { EasingMode = EasingMode.EaseOut };
                DoubleAnimation xAnimation = new DoubleAnimation(-15, 0, duration) { EasingFunction = ease };
                DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, duration) { EasingFunction = ease };

                if (Show)
                {
                    Visibility = Visibility.Visible;

                    xAnimation.From = -15;
                    xAnimation.To = 0;

                    opacityAnimation.From = 0;
                    opacityAnimation.To = 1;
                }
                else
                {
                    xAnimation.From = 0;
                    xAnimation.To = -15;

                    opacityAnimation.From = 1;
                    opacityAnimation.To = 0;

                    opacityAnimation.Completed += (s, e) =>
                    {
                        Visibility = Visibility.Hidden;
                    };
                }

                translateTransform.BeginAnimation(TranslateTransform.YProperty, xAnimation);
                BeginAnimation(OpacityProperty, opacityAnimation);
            });
        }

        private readonly TranslateTransform translateTransform =
            new TranslateTransform();

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool Show
        {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        public Transform ContentRenderTransform
        {
            get { return (Transform)GetValue(ContentRenderTransformProperty); }
            set { SetValue(ContentRenderTransformProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Show.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowProperty =
            DependencyProperty.Register("Show", typeof(bool), typeof(NoteControl), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NoteControl), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for ContentRenderTransform.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentRenderTransformProperty =
            DependencyProperty.Register("ContentRenderTransform", typeof(Transform), typeof(NoteControl), new PropertyMetadata(null));
    }
}
