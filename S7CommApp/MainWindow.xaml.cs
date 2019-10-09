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
using S7Comm;

namespace S7CommApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly S7Comm.S7Comm _plc;

        public MainWindow()
        {
            InitializeComponent();

            _plc = new S7Comm.S7Comm();

            ValueDisplay.Text = _plc.ReadValue().ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _plc.AddValue();
            ValueDisplay.Text = _plc.ReadValue().ToString();
        }

        private void SubButton_Click(object sender, RoutedEventArgs e)
        {
            _plc.SubValue();
            ValueDisplay.Text = _plc.ReadValue().ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _plc.ResetValue();
            ValueDisplay.Text = _plc.ReadValue().ToString();
        }
    }
}
