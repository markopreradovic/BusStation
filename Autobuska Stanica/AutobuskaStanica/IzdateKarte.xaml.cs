using MySql.Data.MySqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for IzdateKarte.xaml
    /// </summary>
    public partial class IzdateKarte : Page
    {
        public static int RadnikID, StanicaID;

        public IzdateKarte()
        {
            InitializeComponent();
            //InitKarteWithAllFilters();
            InitKarte();
            radioAllWorkers.IsChecked = true;
            radioLoggedInWorker.IsChecked = false;
        }

        public IzdateKarte(int a,int b)
        {
            StanicaID = a;
            RadnikID = b;
            InitializeComponent();
            InitKarte();
            //InitKarteWithAllFilters();
            radioAllWorkers.IsChecked = true;
            radioLoggedInWorker.IsChecked = false;
        }
    
        public void InitKarte()
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            string query = @"
SELECT 
    KartaID,
    MjestoPolaska,
    MjestoDolaska,
    DatumPolaska,
    VrijemePolaska,
    VrijemeIzdavanja,
    NazivAutoprevoznika,
    Ruta
FROM karta";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            lstIssuedTickets.Items.Clear();

                            while (reader.Read())
                            {
                                
                                lstIssuedTickets.Items.Add(new
                                {
                                    KartaID = reader["KartaID"].ToString(),
                                    MjestoPolaska = reader["MjestoPolaska"].ToString(),
                                    MjestoDolaska = reader["MjestoDolaska"].ToString(),
                                    DatumPolaska = DateTime.Parse(reader["DatumPolaska"].ToString()).ToString("dd.MM.yyyy"),
                                    VrijemePolaska = reader["VrijemePolaska"].ToString(),
                                    VrijemeIzdavanja = reader["VrijemeIzdavanja"].ToString(),
                                    NazivAutoprevoznika = reader["NazivAutoprevoznika"].ToString(),
                                    Ruta = reader["Ruta"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string poruka;
                string naslov;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    poruka = $"Greška pri dohvatanju podataka: {ex.Message}\n\nDetalji:\n{ex.ToString()}";
                    naslov = "Greška";
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    poruka = $"Error retrieving data: {ex.Message}\n\nDetails:\n{ex.ToString()}";
                    naslov = "Error";
                }
                else
                {
                    poruka = $"Greška pri dohvatanju podataka: {ex.Message}\n\nDetalji:\n{ex.ToString()}";
                    naslov = "Greška";
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox && textBox.Text == GetDefaultText(textBox))
            {
                textBox.Text = "";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = GetDefaultText(textBox);
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s\-]+$");
        }

        private void TextBox_PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        public void InitKarteWithAllFilters()
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            StringBuilder queryBuilder = new StringBuilder(@"
    SELECT 
        KartaID,
        MjestoDolaska,
        DatumPolaska,
        VrijemePolaska,
        VrijemeIzdavanja,
        NazivAutoprevoznika,
        Ruta,
        Cijena
    FROM karta
    WHERE 1=1 "); 

            if (!string.IsNullOrWhiteSpace(tbRuta.Text) && tbRuta.Text != "Ruta" && tbRuta.Text != "Route")
            {
                queryBuilder.Append(" AND Ruta LIKE @Ruta");
            }
            if (!string.IsNullOrWhiteSpace(tbMjestoDolaska.Text) && tbMjestoDolaska.Text != "Mjesto Dolaska"  && tbMjestoDolaska.Text != "Arrival Place")
            {
                queryBuilder.Append(" AND MjestoDolaska LIKE @MjestoDolaska");
            }
            if (dpDatum.SelectedDate.HasValue)
            {
                queryBuilder.Append(" AND DatumPolaska = @DatumPolaska");
            }
            if (radioLoggedInWorker.IsChecked == true)
            {
                queryBuilder.Append(" AND RadnikID = @RadnikID");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                    {
                        if (!string.IsNullOrWhiteSpace(tbRuta.Text) && tbRuta.Text != "Ruta")
                        {
                            cmd.Parameters.AddWithValue("@Ruta", "%" + tbRuta.Text + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(tbMjestoDolaska.Text) && tbMjestoDolaska.Text != "Mjesto Dolaska")
                        {
                            cmd.Parameters.AddWithValue("@MjestoDolaska", "%" + tbMjestoDolaska.Text + "%");
                        }
                        if (dpDatum.SelectedDate.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@DatumPolaska", dpDatum.SelectedDate.Value);
                        }
                        if (radioLoggedInWorker.IsChecked == true)
                        {
                            cmd.Parameters.AddWithValue("@RadnikID", RadnikID);
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            lstIssuedTickets.Items.Clear(); 

                            while (reader.Read())
                            {
                                
                                DateTime parsedDate;
                                string datumPolaska = DateTime.TryParse(reader["DatumPolaska"].ToString(), out parsedDate) ? parsedDate.ToString("dd.MM.yyyy") : "Nepoznato";

                                lstIssuedTickets.Items.Add(new
                                {
                                    KartaID = reader["KartaID"].ToString(),
                                    MjestoDolaska = reader["MjestoDolaska"].ToString(),
                                    DatumPolaska = datumPolaska,
                                    VrijemePolaska = reader["VrijemePolaska"].ToString(),
                                    VrijemeIzdavanja = reader["VrijemeIzdavanja"].ToString(),
                                    NazivAutoprevoznika = reader["NazivAutoprevoznika"].ToString(),
                                    Ruta = reader["Ruta"].ToString(),
                                    Cijena = reader["Cijena"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string poruka;
                string naslov;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    poruka = $"Greška pri dohvatanju podataka:\nUpit: {queryBuilder.ToString()}\n\nDetalji:\n{ex.Message}";
                    naslov = "Greška";
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    poruka = $"Error retrieving data:\nQuery: {queryBuilder.ToString()}\n\nDetails:\n{ex.Message}";
                    naslov = "Error";
                }
                else
                {
                    poruka = $"Greška pri dohvatanju podataka:\nUpit: {queryBuilder.ToString()}\n\nDetalji:\n{ex.Message}";
                    naslov = "Greška";
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        //private void tbRuta_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (tbRuta.Text == "Ruta" || tbRuta.Text == "Route")
        //        return;

        //    InitKarteWithAllFilters();
        //}

        //private void tbMjestoDolaska_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (tbMjestoDolaska.Text == "Mjesto Dolaska" || tbMjestoDolaska.Text == "Arrival Place")
        //        return;
        //    InitKarteWithAllFilters();
        //}

        private void tbRuta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbRuta.Text != GetDefaultText(tbRuta) && tbRuta.Text != "Ruta" && tbRuta.Text != "Route")
            {
                InitKarteWithAllFilters();
            }
        }

        private void tbMjestoDolaska_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbMjestoDolaska.Text != GetDefaultText(tbMjestoDolaska) && tbMjestoDolaska.Text != "Mjesto Dolaska" && tbMjestoDolaska.Text != "Arrival Place")
            {
                InitKarteWithAllFilters();
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDatum.SelectedDate == null)
            {
                InitKarteWithAllFilters();
            }
            else
            {
                InitKarteWithAllFilters();
            }
        }

        private void radioLoggedInWorker_Checked(object sender, RoutedEventArgs e)
        {
            if (radioLoggedInWorker.IsChecked != true)
                return;

            InitKarteWithAllFilters();
        }


        private void radioAllWorkers_Checked(object sender, RoutedEventArgs e)
        {
            if (radioAllWorkers.IsChecked != true)
                return;

            InitKarteWithAllFilters();
        }

        private void InitKarteForLoggedInWorker(int radnikID)
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            string query = @"
SELECT 
    KartaID,
    MjestoPolaska,
    MjestoDolaska,
    DatumPolaska,
    VrijemePolaska,
    VrijemeIzdavanja,
    NazivAutoprevoznika,
    Ruta
FROM karta
WHERE RadnikID = @RadnikID"; 

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RadnikID", radnikID); 

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            lstIssuedTickets.Items.Clear();

                            while (reader.Read())
                            {
                                
                                lstIssuedTickets.Items.Add(new
                                {
                                    KartaID = reader["KartaID"].ToString(),
                                    MjestoPolaska = reader["MjestoPolaska"].ToString(),
                                    MjestoDolaska = reader["MjestoDolaska"].ToString(),
                                    DatumPolaska = DateTime.Parse(reader["DatumPolaska"].ToString()).ToString("dd.MM.yyyy"),
                                    VrijemePolaska = reader["VrijemePolaska"].ToString(),
                                    VrijemeIzdavanja = reader["VrijemeIzdavanja"].ToString(),
                                    NazivAutoprevoznika = reader["NazivAutoprevoznika"].ToString(),
                                    Ruta = reader["Ruta"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string naslov, poruka;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    naslov = "Greška";
                    poruka = "Greška pri dohvatanju podataka: " + ex.Message;
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    naslov = "Error";
                    poruka = "Error retrieving data: " + ex.Message;
                }
                else
                {
                    naslov = "Greška";
                    poruka = "Greška pri dohvatanju podataka: " + ex.Message;
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private string GetDefaultText(System.Windows.Controls.TextBox textBox)
        {
            string jezik = RadnikMain.Jezik;

            if (jezik == "Srpski" || jezik == "Serbian")
            {
                if (textBox.Name.Contains("Ruta")) return "Ruta";
                if (textBox.Name.Contains("MjestoDolaska")) return "Mjesto Dolaska";
            }
            else if (jezik == "Engleski" || jezik == "English")
            {
                if (textBox.Name.Contains("Ruta")) return "Route";
                if (textBox.Name.Contains("MjestoDolaska")) return "Arrival Place";
            }

            return string.Empty;
        }

    }
}
