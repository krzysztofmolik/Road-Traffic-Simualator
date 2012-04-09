using System;
using System.Windows;
using NLog;

namespace RoadTrafficConstructor.Presenters
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            Console.WriteLine(this.DataContext);
        }
    }
}
