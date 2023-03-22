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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigationWindow_Navigated(object sender, NavigationEventArgs e)
        {
            this.BeginAnimation(PaddingProperty, new ThicknessAnimation()
            {
                From = new Thickness(0, 5, 0, 0),
                To = new Thickness(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(100))
            });
        }
    }
}
