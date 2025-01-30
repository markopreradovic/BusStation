using System.Windows.Controls;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Pomoc.xaml
    /// </summary>
    public partial class Pomoc : Page
    {
        public Pomoc()
        {
            InitializeComponent();
            string pdfPath = @"C:\Users\Administrator\source\repos\AutobuskaStanica\AutobuskaStanica\Uputstvo.pdf";
            pdfWebViewer.Navigate(new Uri(pdfPath));

        }
    }
}
