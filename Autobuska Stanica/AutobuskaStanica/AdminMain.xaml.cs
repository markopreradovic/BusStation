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
using System.Windows.Shapes;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for AdminMain.xaml
    /// </summary>
    /// 

    public partial class AdminMain : Window
    {
        static int StanicaID, RadnikID;
        Ruta rute;
        private double originalWidth = 1700; // Originalna širina prozora
        private double originalColumnWidth = 1450; // Originalna širina druge kolone

        public AdminMain()
        {
            rute = new Ruta();
            this.Content = rute;
            InitializeComponent();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Ruta(RadnikID, StanicaID);
            ResetColumnWidth();
        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Radnici();
            ResetColumnWidth();
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void ListBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Stanice();
            // Promeni širinu druge kolone
            ColumnDefinition desnaKolona = myGrid.ColumnDefinitions[1];
            desnaKolona.Width = new GridLength(1150); // Nova širina kolone
            this.Width = 1150 + myGrid.ColumnDefinitions[0].Width.Value; // Nova širina prozora
        }

        private void ResetColumnWidth()
        {
            // Resetuje širinu druge kolone na originalnu
            ColumnDefinition desnaKolona = myGrid.ColumnDefinitions[1];
            desnaKolona.Width = new GridLength(originalColumnWidth);
            this.Width = originalWidth; // Resetuje širinu prozora na originalnu vrednost
        }


        public AdminMain(int a, int b)
        {
            StanicaID = a;
            RadnikID = b;
            rute = new Ruta(StanicaID,RadnikID);
            this.Content = rute;
            InitializeComponent();
        }
    }
}
