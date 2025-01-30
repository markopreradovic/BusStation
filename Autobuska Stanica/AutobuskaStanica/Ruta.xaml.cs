using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Ruta.xaml
    /// </summary>
    /// 

    public partial class Ruta : Page
    {
        int StanicaID, RadnikID;
        public string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
        public Ruta()
        {
            InitializeComponent();
            InitRute();
        }

        public Ruta(int a, int b)
        {
            StanicaID = a;
            RadnikID = b;
            InitializeComponent();
            InitRute();
        }

        private void InitRute()
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            DataTable ruteTable = new DataTable();

            string query = @"
    SELECT 
        r.RutaID,
        r.Ruta, 
        r.VrijemePolaska, 
        r.Dani, 
        r.MjestoDolaska, 
        r.Cijena, 
        r.NazivAutoprevoznika,  
        r.RutaID,
        r.MjestoPolaska
    FROM ruta r";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(ruteTable); 
                    }
                }

                lstRoutes.ItemsSource = ruteTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri dohvaćanju podataka: " + ex.Message);
            }
        }

        private void TextBox_PreviewAlphaInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$");
        }

        private void TextBox_PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }
        private void TextBox_PreviewNumericWithDotInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9\.]+$");
        }


        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRuta.Text) ||
                tpVrijemePolaska.SelectedTime == null ||
                string.IsNullOrWhiteSpace(tbMjestoPolaska.Text) ||
                string.IsNullOrWhiteSpace(tbMjestoDolaska.Text) ||
                string.IsNullOrWhiteSpace(tbDani.Text) ||
                string.IsNullOrWhiteSpace(tbCijena.Text) ||
                string.IsNullOrWhiteSpace(tbNazivAutoprevoznika.Text) ||
                string.IsNullOrWhiteSpace(tbStanicaID.Text))
            {
                MessageBox.Show("Molimo unesite sve podatke!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string ruta = tbRuta.Text.Trim();
            string vrijemePolaska = tpVrijemePolaska.SelectedTime.Value.ToString("HH:mm");
            string mjestoPolaska = tbMjestoPolaska.Text.Trim();
            string mjestoDolaska = tbMjestoDolaska.Text.Trim();
            string dani = tbDani.Text.Trim();
            decimal cijena;
            if (!decimal.TryParse(tbCijena.Text.Trim(), out cijena))
            {
                MessageBox.Show("Cijena mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string autoprevoznik = tbNazivAutoprevoznika.Text.Trim();

            int stanicaID;
            if (!int.TryParse(tbStanicaID.Text.Trim(), out stanicaID))
            {
                MessageBox.Show("Stanica ID mora biti validan broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = @"
    SET FOREIGN_KEY_CHECKS = 0;
    INSERT INTO Ruta (Ruta, VrijemePolaska, MjestoPolaska, MjestoDolaska, Dani, Cijena, NazivAutoprevoznika, StanicaID)
    VALUES (@Ruta, @VrijemePolaska, @MjestoPolaska, @MjestoDolaska, @Dani, @Cijena, @NazivAutoprevoznika, @StanicaID);
    SET FOREIGN_KEY_CHECKS = 1;
";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ruta", ruta);
                        cmd.Parameters.AddWithValue("@VrijemePolaska", vrijemePolaska);
                        cmd.Parameters.AddWithValue("@MjestoPolaska", mjestoPolaska);
                        cmd.Parameters.AddWithValue("@MjestoDolaska", mjestoDolaska);
                        cmd.Parameters.AddWithValue("@Dani", dani);
                        cmd.Parameters.AddWithValue("@Cijena", cijena);
                        cmd.Parameters.AddWithValue("@NazivAutoprevoznika", autoprevoznik);
                        cmd.Parameters.AddWithValue("@StanicaID", stanicaID);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Ruta je uspješno dodana!", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);

                    tbRuta.Text = string.Empty;
                    tpVrijemePolaska.SelectedTime = null;
                    tbMjestoPolaska.Text = string.Empty;
                    tbMjestoDolaska.Text = string.Empty;
                    tbDani.Text = string.Empty;
                    tbCijena.Text = string.Empty;
                    tbNazivAutoprevoznika.Text = string.Empty;
                    tbStanicaID.Text = string.Empty;

                    InitRute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom unosa podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            if (lstRoutes.SelectedItem == null)
            {
                MessageBox.Show("Molimo selektujte rutu koju želite da obrišete!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedRow = lstRoutes.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                MessageBox.Show("Greška pri učitavanju selektovanih podataka!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int rutaID = Convert.ToInt32(selectedRow["RutaID"]); 

            string query = @"
    DELETE FROM Ruta WHERE RutaID = @RutaID;
    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RutaID", rutaID);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Ruta je uspješno obrisana!", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);

                    lstRoutes.SelectedItem = null;

                    InitRute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom brisanja rute: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}
