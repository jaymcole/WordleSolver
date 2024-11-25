using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordleSolver;


namespace WordleSolverApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WordleManager manager;
        private Driver driver;

        public MainWindow()
        {
            InitializeComponent();
            manager = new WordleManager();
            driver = new Driver();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            manager = new WordleManager();
        }
    }
}