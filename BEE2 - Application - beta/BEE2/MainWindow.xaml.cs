using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BEE2
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PuzzleItem> PuzzleItems = new ObservableCollection<PuzzleItem>();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OptionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow ow = new OptionsWindow();
            ow.Show();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Show();
        }
    }
}
