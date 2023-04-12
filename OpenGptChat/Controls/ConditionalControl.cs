using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    ///     <MyNamespace:ConditionalControl/>
    ///
    /// </summary>
    public class ConditionalControl : Control
    {
        static ConditionalControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConditionalControl), new FrameworkPropertyMetadata(typeof(ConditionalControl)));
        }



        public bool Condition
        {
            get { return (bool)GetValue(ConditionProperty); }
            set { SetValue(ConditionProperty, value); }
        }

        public FrameworkElement ElementWhileTrue
        {
            get { return (FrameworkElement)GetValue(ElementWhileTrueProperty); }
            set { SetValue(ElementWhileTrueProperty, value); }
        }

        public FrameworkElement ElementWhileFalse
        {
            get { return (FrameworkElement)GetValue(ElementWhileFalseProperty); }
            set { SetValue(ElementWhileFalseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Condition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(ConditionalControl), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for ElementWhileTrue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementWhileTrueProperty =
            DependencyProperty.Register(nameof(ElementWhileTrue), typeof(FrameworkElement), typeof(ConditionalControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ElementWhileFalse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementWhileFalseProperty =
            DependencyProperty.Register(nameof(ElementWhileFalse), typeof(FrameworkElement), typeof(ConditionalControl), new PropertyMetadata(null));
    }
}
