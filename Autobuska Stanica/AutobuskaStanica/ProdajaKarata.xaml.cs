using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;




namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for ProdajaKarata.xaml
    /// </summary>
    /// 

    public class RutaObj
    {
        public string Ruta { get; set; }
        public string VrijemePolaska { get; set; }
        public string Dani { get; set; }
        public string MjestoDolaska { get; set; }
        public string Cijena { get; set; }
        public string NazivAutoprevoznika { get; set; }
        public string BusID { get; set; }
        public int RutaID { get; set; }
        public string VozacID { get; set; }

        

        

        public RutaObj(string ruta, string vrijemePolaska, string dani, string mjestoDolaska, string cijena, string nazivAutoprevoznika, string busID, string rutaID, string vozacID)
        {
            Ruta = ruta;
            VrijemePolaska = vrijemePolaska;
            Dani = dani;
            MjestoDolaska = mjestoDolaska;
            Cijena = cijena;
            NazivAutoprevoznika = nazivAutoprevoznika;
            BusID = busID;
            RutaID = Convert.ToInt32(rutaID);
            VozacID = vozacID;
        }

    }
    public partial class ProdajaKarata : Page
    {
        public static int StanicaID, RadnikID;
        private ObservableCollection<object> Rute { get; set; } = new ObservableCollection<object>();

        public ProdajaKarata()
        {
            InitializeComponent();
            ucitajJezik();
            InitRuteWithFilters("", "");
        }

        void ucitajJezik()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            if (RadnikMain.Jezik.Equals("Srpski", StringComparison.OrdinalIgnoreCase) || RadnikMain.Jezik.Equals("Serbian", StringComparison.OrdinalIgnoreCase))
            {
                dictionary.Source = new("..\\Strings.srp.xaml", UriKind.Relative);
            }
            else if (RadnikMain.Jezik.Equals("Engleski", StringComparison.OrdinalIgnoreCase) || RadnikMain.Jezik.Equals("English", StringComparison.OrdinalIgnoreCase))
            {
                dictionary.Source = new("..\\Strings.en.xaml", UriKind.Relative);
            }
            this.Resources.MergedDictionaries.Add(dictionary);
            InitRuteWithFilters("", "");
        }
        public ProdajaKarata(int a, int b)
        {
            StanicaID = a;
            RadnikID = b;
            InitializeComponent();
            InitRuteWithFilters("", "");
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s\-]+$");
        }

        private void TextBox_PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox && textBox.Text == GetDefaultText(textBox))
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
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

        private string GetDefaultText(System.Windows.Controls.TextBox textBox)
        {
            string jezik = RadnikMain.Jezik;

            if (jezik == "Srpski" || jezik == "Serbian")
            {
                if (textBox.Name.Contains("Ime")) return "Ime";
                if (textBox.Name.Contains("Prezime")) return "Prezime";
                if (textBox.Name.Contains("Kontakt")) return "Kontakt";
                if (textBox.Name.Contains("Peron")) return "Peron";
                if (textBox.Name.Contains("Dani")) return "Mjesto Polaska";
                if (textBox.Name.Contains("tbMjestoDolaska")) return "Mjesto Dolaska";
                if (textBox.Name.Contains("VrijemePolaska")) return "Vrijeme Polaska";
                if (textBox.Name.Contains("StornirajID")) return "ID Karte";
                if (textBox.Name.Contains("PutID")) return "Put ID";
            }
            else if (jezik == "Engleski" || jezik == "English")
            {
                if (textBox.Name.Contains("Ime")) return "Name";
                if (textBox.Name.Contains("Prezime")) return "Surname";
                if (textBox.Name.Contains("Kontakt")) return "Contact";
                if (textBox.Name.Contains("Peron")) return "Platform";
                if (textBox.Name.Contains("Dani")) return "Arrival Place";
                if (textBox.Name.Contains("tbMjestoDolaska")) return "Arrival Place";
                if (textBox.Name.Contains("VrijemePolaska")) return "Departure Time";
                if (textBox.Name.Contains("StornirajID")) return "Ticket ID";
                if (textBox.Name.Contains("PutID")) return "Route ID";
            }

            return string.Empty;
        }

        private void InitRute()
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            DataTable ruteTable = new DataTable();

            string query = @"
        SELECT 
            r.Ruta, 
            r.VrijemePolaska, 
            r.Dani, 
            r.MjestoDolaska, 
            r.Cijena, 
            r.NazivAutoprevoznika, 
            a.BusID, 
            r.RutaID,
            r.MjestoPolaska
        FROM ruta r
        INNER JOIN autobus a ON r.RutaID = a.RutaID
        WHERE r.StanicaID = @StanicaID"; 

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StanicaID", StanicaID);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(ruteTable);
                    }
                }

                Rute.Clear();
                foreach (DataRow row in ruteTable.Rows)
                {
                    Rute.Add(new
                    {
                        Ruta = row["Ruta"].ToString(),
                        VrijemePolaska = row["VrijemePolaska"].ToString(),
                        Dani = row["Dani"].ToString(),
                        MjestoDolaska = row["MjestoDolaska"].ToString(),
                        MjestoPolaska = row["MjestoPolaska"].ToString(),
                        Cijena = row["Cijena"].ToString(),
                        NazivAutoprevoznika = row["NazivAutoprevoznika"].ToString(),
                        BusID = row["BusID"].ToString(),
                        RutaID = row["RutaID"].ToString()

                    });
                }

                lstRoutes.ItemsSource = Rute;
            }
            catch (Exception ex)
            {
                string naslov, poruka;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    naslov = "Greška";
                    poruka = "Greška pri dohvaćanju podataka: " + ex.Message;
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    naslov = "Error";
                    poruka = "Error retrieving data: " + ex.Message;
                }
                else
                {
                    naslov = "Greška"; // Podrazumevano srpski
                    poruka = "Greška pri dohvaćanju podataka: " + ex.Message;
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    // Validacija praznih polja
        //    if (string.IsNullOrWhiteSpace(tbPeron.Text) ||
        //        dpDatum.SelectedDate == null ||
        //        lstRoutes.SelectedItem == null ||
        //        !(jednosmjerna.IsChecked == true || povratna.IsChecked == true) ||
        //        cbPopust.SelectedItem == null)
        //    {
        //        MessageBox.Show("Sva polja su obavezna. Molimo popunite sve podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";

        //    // Podaci za kartu
        //    int BusID = 0;
        //    int Peron;
        //    string Tip = "";
        //    string Popust = "";
        //    DateTime DatumPolaska = dpDatum.SelectedDate.Value;
        //    string Ruta = "", VrijemePolaska = "", Dani = "", MjestoDolaska = "", MjestoPolaska = "";
        //    string VrijemeIzdavanja = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    double Cijena = 1.0;
        //    int RutaID = 0;
        //    string NazivAutoprevoznika = "";
        //    int Sjediste = 1;
        //    int PutnikID;


        //    // Podaci za putnika
        //    string Ime = txtIme.Text, Prezime = txtPrezime.Text, Kontakt = txtKontakt.Text;
        //    if (Ime == "Ime" && Prezime == "Prezime")
        //    {
        //        Ime = "";
        //        Prezime = "";
        //    }

        //    // Peron
        //    if (!int.TryParse(tbPeron.Text, out Peron))
        //    {
        //        MessageBox.Show("Unesite validan broj za peron.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    // Pomoćne varijable za cenu
        //    double koef = jednosmjerna.IsChecked == true ? 1.0 : 1.8;
        //    if (koef == 1) Tip = "Jednosmjerna";
        //    else Tip = "Povratna";

        //    if (cbPopust.SelectedItem is ComboBoxItem selectedItem)
        //    {
        //        string odabraniPopust = selectedItem.Content.ToString();
        //        if (odabraniPopust.Contains("Student"))
        //        {
        //            koef *= 0.8;
        //            Popust = "Student-(20%)";
        //        }
        //        else if (odabraniPopust.Contains("Penzioner") || odabraniPopust.Contains("Pensioner"))
        //        {
        //            koef *= 0.75;
        //            Popust = "Penzioner-(25%)";
        //        }
        //        else if (odabraniPopust.Contains("Učenik") || odabraniPopust.Contains("Deacon"))
        //        {
        //            koef *= 0.85;
        //            Popust = "Učenik-(15%)";
        //        }
        //        else if (odabraniPopust.Contains("Bez Popusta") || odabraniPopust.Contains("No Discount"))
        //        {
        //            koef *= 1;
        //            Popust = "Bez Popusta";
        //        }
        //    }

        //    if (lstRoutes.SelectedIndex >= 0) // Proverava da li je neki element selektovan
        //    {
        //        int selectedIndex = lstRoutes.SelectedIndex;
        //        var selectedRow = lstRoutes.Items[selectedIndex];

        //        if (selectedRow is RutaObj rutaObj)
        //        {
        //            RutaID = rutaObj.RutaID;
        //            BusID = Convert.ToInt32(rutaObj.BusID);
        //            Ruta = rutaObj.Ruta;
        //            VrijemePolaska = rutaObj.VrijemePolaska;
        //            Cijena = Convert.ToDouble(rutaObj.Cijena) * koef;
        //            NazivAutoprevoznika = rutaObj.NazivAutoprevoznika;

        //            // Ako želimo da prikažemo mesta odabranog reda
        //            string query = "SELECT MjestoPolaska, MjestoDolaska FROM ruta WHERE RutaID = @RutaID";

        //            using (var conn = new MySqlConnection(connectionString))
        //            {
        //                try
        //                {
        //                    conn.Open();
        //                    using (var cmd = new MySqlCommand(query, conn))
        //                    {
        //                        cmd.Parameters.AddWithValue("@RutaID", RutaID);
        //                        using (var reader = cmd.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                MjestoPolaska = reader["MjestoPolaska"].ToString();
        //                                MjestoDolaska = reader["MjestoDolaska"].ToString();
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (FormatException ex)
        //                {
        //                    MessageBox.Show($"Greška u formatu podataka: {ex.Message}");
        //                }
        //                catch (MySqlException ex)
        //                {
        //                    MessageBox.Show($"Greška prilikom dohvaćanja mesta polaska i dolaska: {ex.Message}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show($"Nepredviđena greška: {ex.Message}");
        //                }
        //            }


        //        }
        //        else
        //        {
        //            MessageBox.Show("Selektovani red nije RutaObj.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Nijedna ruta nije selektovana.");
        //    }


        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();


        //            new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection).ExecuteNonQuery();


        //            string putnikQuery = "INSERT INTO Putnik (Ime, Prezime, Kontakt) VALUES (@Ime, @Prezime, @Kontakt);";
        //            MySqlCommand putnikCommand = new MySqlCommand(putnikQuery, connection);
        //            putnikCommand.Parameters.AddWithValue("@Ime", Ime);
        //            putnikCommand.Parameters.AddWithValue("@Prezime", Prezime);
        //            putnikCommand.Parameters.AddWithValue("@Kontakt", Kontakt);


        //            putnikCommand.ExecuteNonQuery();


        //            string getLastInsertIDQuery = "SELECT LAST_INSERT_ID();";
        //            MySqlCommand getLastInsertIDCommand = new MySqlCommand(getLastInsertIDQuery, connection);
        //            PutnikID = Convert.ToInt32(getLastInsertIDCommand.ExecuteScalar());


        //            string kartaQuery = "INSERT INTO karta(BusID, Peron, Tip, DatumPolaska, VrijemePolaska, VrijemeIzdavanja, Cijena, RutaID, NazivAutoprevoznika, MjestoPolaska, MjestoDolaska, Sjediste, PutnikID, RadnikID, StanicaID, Ruta, DatumIzdavanja)" +
        //                                 "VALUES (@BusID, @Peron, @Tip, @DatumPolaska, @VrijemePolaska, @VrijemeIzdavanja, @Cijena, @RutaID, @NazivAutoprevoznika, @MjestoPolaska, @MjestoDolaska, @Sjediste, @PutnikID, @RadnikID, @StanicaID, @Ruta, @DatumIzdavanja)";

        //            MySqlCommand kartaCommand = new MySqlCommand(kartaQuery, connection);
        //            kartaCommand.Parameters.AddWithValue("@BusID", BusID);
        //            kartaCommand.Parameters.AddWithValue("@Peron", Peron);
        //            kartaCommand.Parameters.AddWithValue("@Tip", Tip);
        //            kartaCommand.Parameters.AddWithValue("@DatumPolaska", DatumPolaska);
        //            kartaCommand.Parameters.AddWithValue("@VrijemePolaska", VrijemePolaska);
        //            kartaCommand.Parameters.AddWithValue("@MjestoPolaska", MjestoPolaska);
        //            kartaCommand.Parameters.AddWithValue("@MjestoDolaska", MjestoDolaska);
        //            kartaCommand.Parameters.AddWithValue("@VrijemeIzdavanja", VrijemeIzdavanja);
        //            kartaCommand.Parameters.AddWithValue("@Cijena", Cijena); // Postavljamo Cijenu kao double
        //            kartaCommand.Parameters.AddWithValue("@PutnikID", PutnikID);
        //            kartaCommand.Parameters.AddWithValue("@RutaID", RutaID);
        //            kartaCommand.Parameters.AddWithValue("@NazivAutoprevoznika", NazivAutoprevoznika);
        //            kartaCommand.Parameters.AddWithValue("@Sjediste", Sjediste);
        //            kartaCommand.Parameters.AddWithValue("@RadnikID", RadnikID);
        //            kartaCommand.Parameters.AddWithValue("@StanicaID", StanicaID);
        //            kartaCommand.Parameters.AddWithValue("@Ruta", Ruta);
        //            kartaCommand.Parameters.AddWithValue("@DatumIzdavanja", DateTime.Now.Date);


        //            kartaCommand.ExecuteNonQuery();


        //            DateTime Datum = DateTime.Now.Date;
        //            string detaljiKarte = $"Podaci o karti:\n" +
        //                $"Ruta: {Ruta}\n" +
        //                $"Datum polaska: {DatumPolaska:yyyy-MM-dd}\n" +
        //                $"Vrijeme polaska: {VrijemePolaska}\n" +
        //                $"Mjesto polaska: {MjestoPolaska}\n" +
        //                $"Mjesto dolaska: {MjestoDolaska}\n" +
        //                $"Peron: {Peron}\n" +
        //                $"Tip karte: {Tip}\n" +
        //                $"Sjedište: {Sjediste}\n" +
        //                $"Cijena: {Cijena:F2} KM\n" +
        //                $"Naziv autoprevoznika: {NazivAutoprevoznika}\n\n" +
        //                $"Datum izdavanja: {Datum:yyyy-MM-dd}\n" +
        //                $"RadnikID: {RadnikID}\n" +
        //                $"Podaci o putniku:\n" +
        //                $"Ime: {Ime}\n" +
        //                $"Prezime: {Prezime}\n" +
        //                $"Kontakt: {Kontakt}";


        //            MessageBox.Show(detaljiKarte, "Detalji o karti i putniku", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {

        //            MessageBox.Show($"Greška prilikom izdavanja karte: {ex.Message}\n{ex.StackTrace}");
        //            Clipboard.SetText($"Greška prilikom izdavanja karte: {ex.Message}\n{ex.StackTrace}");
        //        }
        //        finally
        //        {

        //            new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection).ExecuteNonQuery();
        //        }
        //    }

        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPeron.Text) ||
                dpDatum.SelectedDate == null ||
                lstRoutes.SelectedItem == null ||
                !(jednosmjerna.IsChecked == true || povratna.IsChecked == true) ||
                cbPopust.SelectedItem == null)
            {
                string naslov, poruka;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    naslov = "Greška";
                    poruka = "Sva polja su obavezna. Molimo popunite sve podatke.";
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    naslov = "Error";
                    poruka = "All fields are required. Please fill in all the data.";
                }
                else
                {
                    naslov = "Greška"; 
                    poruka = "Sva polja su obavezna. Molimo popunite sve podatke.";
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";

            int BusID = 0;
            int Peron;
            string Tip = "";
            string Popust = "";
            DateTime DatumPolaska = dpDatum.SelectedDate.Value;
            string Ruta = "", VrijemePolaska = "", Dani = "", MjestoDolaska = "", MjestoPolaska = "";
            string VrijemeIzdavanja = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            double Cijena = 1.0;
            int RutaID = 0;
            string NazivAutoprevoznika = "";
            int Sjediste = 1;
            int PutnikID;


            string Ime = txtIme.Text, Prezime = txtPrezime.Text, Kontakt = txtKontakt.Text;
            if (Ime == "Ime" && Prezime == "Prezime")
            {
                Ime = "";
                Prezime = "";
            }

            if (!int.TryParse(tbPeron.Text, out Peron))
            {
                string naslov, poruka;

                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    naslov = "Greška";
                    poruka = "Unesite validan broj za peron.";
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    naslov = "Error";
                    poruka = "Please enter a valid number for the platform.";
                }
                else
                {
                    naslov = "Greška"; 
                    poruka = "Unesite validan broj za peron.";
                }

                MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            double koef = jednosmjerna.IsChecked == true ? 1.0 : 1.8;
            if (koef == 1) Tip = "Jednosmjerna";
            else Tip = "Povratna";

            if (cbPopust.SelectedItem is ComboBoxItem selectedItem)
            {
                string odabraniPopust = selectedItem.Content.ToString();
                if (odabraniPopust.Contains("Student"))
                {
                    koef *= 0.8;
                    Popust = "Student-(20%)";
                }
                else if (odabraniPopust.Contains("Penzioner") || odabraniPopust.Contains("Pensioner"))
                {
                    koef *= 0.75;
                    Popust = "Penzioner-(25%)";
                }
                else if (odabraniPopust.Contains("Učenik") || odabraniPopust.Contains("Deacon"))
                {
                    koef *= 0.85;
                    Popust = "Učenik-(15%)";
                }
                else if (odabraniPopust.Contains("Bez Popusta") || odabraniPopust.Contains("No Discount"))
                {
                    koef *= 1;
                    Popust = "Bez Popusta";
                }
            }

            if (lstRoutes.SelectedIndex >= 0) 
            {
                int selectedIndex = lstRoutes.SelectedIndex;
                var selectedRow = lstRoutes.Items[selectedIndex];

                if (selectedRow is RutaObj rutaObj)
                {
                    RutaID = rutaObj.RutaID;
                    BusID = Convert.ToInt32(rutaObj.BusID);
                    Ruta = rutaObj.Ruta;
                    VrijemePolaska = rutaObj.VrijemePolaska;
                    Cijena = Convert.ToDouble(rutaObj.Cijena) * koef;
                    NazivAutoprevoznika = rutaObj.NazivAutoprevoznika;

                    string query = "SELECT MjestoPolaska, MjestoDolaska FROM ruta WHERE RutaID = @RutaID";

                    using (var conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (var cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@RutaID", RutaID);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        MjestoPolaska = reader["MjestoPolaska"].ToString();
                                        MjestoDolaska = reader["MjestoDolaska"].ToString();
                                    }
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            string naslov, poruka;

                            if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                            {
                                naslov = "Greška";
                                poruka = $"Greška u formatu podataka: {ex.Message}";
                            }
                            else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                            {
                                naslov = "Error";
                                poruka = $"Data format error: {ex.Message}";
                            }
                            else
                            {
                                naslov = "Greška"; 
                                poruka = $"Greška u formatu podataka: {ex.Message}";
                            }

                            MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (MySqlException ex)
                        {
                            string naslov, poruka;

                            if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                            {
                                naslov = "Greška";
                                poruka = $"Greška prilikom dohvatanja mjesta polaska i dolaska: {ex.Message}";
                            }
                            else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                            {
                                naslov = "Error";
                                poruka = $"Error retrieving departure and arrival locations: {ex.Message}";
                            }
                            else
                            {
                                naslov = "Greška"; 
                                poruka = $"Greška prilikom dohvatanja mjesta polaska i dolaska: {ex.Message}";
                            }

                            MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {
                            string naslov, poruka;

                            if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                            {
                                naslov = "Greška";
                                poruka = $"Nepredviđena greška: {ex.Message}";
                            }
                            else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                            {
                                naslov = "Error";
                                poruka = $"Unexpected error: {ex.Message}";
                            }
                            else
                            {
                                naslov = "Greška"; 
                                poruka = $"Nepredviđena greška: {ex.Message}";
                            }

                            MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }


                }
                else
                {
                    string naslov, poruka;

                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        naslov = "Greška";
                        poruka = "Selektovani red nije RutaObj.";
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        naslov = "Error";
                        poruka = "The selected row is not a RutaObj.";
                    }
                    else
                    {
                        naslov = "Greška"; 
                        poruka = "Selektovani red nije RutaObj.";
                    }

                    MessageBox.Show(poruka, naslov, MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    MessageBox.Show("Nijedna ruta nije selektovana.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    MessageBox.Show("No route selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection).ExecuteNonQuery();

                    string putnikQuery = "INSERT INTO Putnik (Ime, Prezime, Kontakt) VALUES (@Ime, @Prezime, @Kontakt);";
                    MySqlCommand putnikCommand = new MySqlCommand(putnikQuery, connection);
                    putnikCommand.Parameters.AddWithValue("@Ime", Ime);
                    putnikCommand.Parameters.AddWithValue("@Prezime", Prezime);
                    putnikCommand.Parameters.AddWithValue("@Kontakt", Kontakt);

                    putnikCommand.ExecuteNonQuery();

                    string getLastInsertIDQuery = "SELECT LAST_INSERT_ID();";
                    MySqlCommand getLastInsertIDCommand = new MySqlCommand(getLastInsertIDQuery, connection);
                    PutnikID = Convert.ToInt32(getLastInsertIDCommand.ExecuteScalar());

                    string kartaQuery = "INSERT INTO karta(BusID, Peron, Tip, DatumPolaska, VrijemePolaska, VrijemeIzdavanja, Cijena, RutaID, NazivAutoprevoznika, MjestoPolaska, MjestoDolaska, Sjediste, PutnikID, RadnikID, StanicaID, Ruta, DatumIzdavanja)" +
                                         "VALUES (@BusID, @Peron, @Tip, @DatumPolaska, @VrijemePolaska, @VrijemeIzdavanja, @Cijena, @RutaID, @NazivAutoprevoznika, @MjestoPolaska, @MjestoDolaska, @Sjediste, @PutnikID, @RadnikID, @StanicaID, @Ruta, @DatumIzdavanja)";

                    MySqlCommand kartaCommand = new MySqlCommand(kartaQuery, connection);
                    kartaCommand.Parameters.AddWithValue("@BusID", BusID);
                    kartaCommand.Parameters.AddWithValue("@Peron", Peron);
                    kartaCommand.Parameters.AddWithValue("@Tip", Tip);
                    kartaCommand.Parameters.AddWithValue("@DatumPolaska", DatumPolaska);
                    kartaCommand.Parameters.AddWithValue("@VrijemePolaska", VrijemePolaska);
                    kartaCommand.Parameters.AddWithValue("@MjestoPolaska", MjestoPolaska);
                    kartaCommand.Parameters.AddWithValue("@MjestoDolaska", MjestoDolaska);
                    kartaCommand.Parameters.AddWithValue("@VrijemeIzdavanja", VrijemeIzdavanja);
                    kartaCommand.Parameters.AddWithValue("@Cijena", Cijena); 
                    kartaCommand.Parameters.AddWithValue("@PutnikID", PutnikID);
                    kartaCommand.Parameters.AddWithValue("@RutaID", RutaID);
                    kartaCommand.Parameters.AddWithValue("@NazivAutoprevoznika", NazivAutoprevoznika);
                    kartaCommand.Parameters.AddWithValue("@Sjediste", Sjediste);
                    kartaCommand.Parameters.AddWithValue("@RadnikID", RadnikID);
                    kartaCommand.Parameters.AddWithValue("@StanicaID", StanicaID);
                    kartaCommand.Parameters.AddWithValue("@Ruta", Ruta);
                    kartaCommand.Parameters.AddWithValue("@DatumIzdavanja", DateTime.Now.Date);

                    kartaCommand.ExecuteNonQuery();

                    string kartaIDQuery = "SELECT LAST_INSERT_ID();";
                    MySqlCommand kartaIDCommand = new MySqlCommand(kartaIDQuery, connection);
                    int KartaID = Convert.ToInt32(kartaIDCommand.ExecuteScalar());

                    DateTime Datum = DateTime.Now.Date;
                    string detaljiKarte = $"Podaci o karti:\n" +
                        $"Ruta: {Ruta}\n" +
                        $"Datum polaska: {DatumPolaska:yyyy-MM-dd}\n" +
                        $"Vrijeme polaska: {VrijemePolaska}\n" +
                        $"Mjesto polaska: {MjestoPolaska}\n" +
                        $"Mjesto dolaska: {MjestoDolaska}\n" +
                        $"Peron: {Peron}\n" +
                        $"Tip karte: {Tip}\n" +
                        $"Sjedište: {Sjediste}\n" +
                        $"Cijena: {Cijena:F2} KM\n" +
                        $"Naziv autoprevoznika: {NazivAutoprevoznika}\n\n" +
                        $"KartaID: {KartaID}\n" + // Ispisivanje KartaID
                        $"Datum izdavanja: {Datum:yyyy-MM-dd}\n" +
                        $"RadnikID: {RadnikID}\n" +
                        $"Podaci o putniku:\n" +
                        $"Ime: {Ime}\n" +
                        $"Prezime: {Prezime}\n" +
                        $"Kontakt: {Kontakt}";

                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show(detaljiKarte, "Detalji o karti i putniku", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show(detaljiKarte, "Ticket and Passenger Details", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (Exception ex)
                {
                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show($"Greška prilikom izdavanja karte: {ex.Message}\n{ex.StackTrace}");
                        Clipboard.SetText($"Greška prilikom izdavanja karte: {ex.Message}\n{ex.StackTrace}");
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show($"Error while issuing the ticket: {ex.Message}\n{ex.StackTrace}");
                        Clipboard.SetText($"Error while issuing the ticket: {ex.Message}\n{ex.StackTrace}");
                    }

                }
                finally
                {
                    new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection).ExecuteNonQuery();
                }
            }

        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbStornirajID.Text) || !int.TryParse(tbStornirajID.Text, out int kartaID))
            {
                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    MessageBox.Show("Molimo unesite validan KartaID.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    MessageBox.Show("Please enter a valid TicketID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return;
            }

            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection).ExecuteNonQuery();

                    string findPutnikQuery = "SELECT PutnikID FROM Karta WHERE KartaID = @KartaID;";
                    MySqlCommand findPutnikCommand = new MySqlCommand(findPutnikQuery, connection);
                    findPutnikCommand.Parameters.AddWithValue("@KartaID", kartaID);
                    object putnikIDObj = findPutnikCommand.ExecuteScalar();

                    if (putnikIDObj == null)
                    {
                        if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                        {
                            MessageBox.Show("Karta sa unesenim ID-jem ne postoji.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                        {
                            MessageBox.Show("The ticket with the entered ID does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        return;
                    }

                    int putnikID = Convert.ToInt32(putnikIDObj);

                    string deleteKartaQuery = "DELETE FROM Karta WHERE KartaID = @KartaID;";
                    MySqlCommand deleteKartaCommand = new MySqlCommand(deleteKartaQuery, connection);
                    deleteKartaCommand.Parameters.AddWithValue("@KartaID", kartaID);
                    deleteKartaCommand.ExecuteNonQuery();

                    string deletePutnikQuery = "DELETE FROM Putnik WHERE PutnikID = @PutnikID;";
                    MySqlCommand deletePutnikCommand = new MySqlCommand(deletePutnikQuery, connection);
                    deletePutnikCommand.Parameters.AddWithValue("@PutnikID", putnikID);
                    deletePutnikCommand.ExecuteNonQuery();

                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show($"Karta sa ID-jem {kartaID} i povezani putnik su uspješno obrisani.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show($"Ticket with ID {kartaID} and the associated passenger have been successfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (MySqlException ex)
                {
                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show($"Greška prilikom brisanja karte: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show($"Error while deleting the ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                finally
                {
                    new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection).ExecuteNonQuery();
                }
            }
        }

        public void InitRuteWithFilters(string filterMjestoDolaska, string filterVrijemePolaska)
        {
            
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            StringBuilder queryBuilder = new StringBuilder(@"
SELECT 
    r.Ruta, 
    r.VrijemePolaska, 
    r.Dani, 
    r.MjestoDolaska, 
    r.Cijena, 
    r.NazivAutoprevoznika, 
    a.BusID, 
    r.RutaID, 
    v.VozacID
FROM ruta r
INNER JOIN autobus a ON r.RutaID = a.RutaID
INNER JOIN vozac v ON r.RutaID = v.RutaID
WHERE r.StanicaID = @StanicaID");

            if (!string.IsNullOrWhiteSpace(filterMjestoDolaska) &&  (filterMjestoDolaska != "Mjesto Dolaska" && filterMjestoDolaska != "Arrival Place"))
            {
                queryBuilder.Append(" AND r.MjestoDolaska LIKE @FilterMjestoDolaska");
            }
            if (!string.IsNullOrWhiteSpace(filterVrijemePolaska) && (filterVrijemePolaska != "Vrijeme Polaska" && filterVrijemePolaska != "Departure Time"))
            {
                queryBuilder.Append(" AND r.VrijemePolaska LIKE @FilterVrijemePolaska");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                    {
                        cmd.Parameters.AddWithValue("@StanicaID", StanicaID);
                        if (!string.IsNullOrWhiteSpace(filterMjestoDolaska) && filterMjestoDolaska != "Mjesto Dolaska")
                        {
                            cmd.Parameters.AddWithValue("@FilterMjestoDolaska", "%" + filterMjestoDolaska + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(filterVrijemePolaska) && filterVrijemePolaska != "Vrijeme Polaska")
                        {
                            cmd.Parameters.AddWithValue("@FilterVrijemePolaska", "%" + filterVrijemePolaska + "%");
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            lstRoutes.Items.Clear();

                            while (reader.Read())
                            {
                                var ruta = new RutaObj(
                                    reader["Ruta"].ToString(),
                                    reader["VrijemePolaska"].ToString(),
                                    reader["Dani"].ToString(),
                                    reader["MjestoDolaska"].ToString(),
                                    reader["Cijena"].ToString(),
                                    reader["NazivAutoprevoznika"].ToString(),
                                    reader["BusID"].ToString(),
                                    reader["RutaID"].ToString(),
                                    reader["VozacID"].ToString()
                                );

                                
                                lstRoutes.Items.Add(ruta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    MessageBox.Show($"Error while fetching data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        


        private void tbMjestoDolaska_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterMjestoDolaska = tbMjestoDolaska?.Text ?? string.Empty; 
            string filterVrijemePolaska = tbVrijemePolaska?.Text ?? string.Empty; 

            
            InitRuteWithFilters(
                string.IsNullOrWhiteSpace(filterMjestoDolaska) ? "" : filterMjestoDolaska,
                string.IsNullOrWhiteSpace(filterVrijemePolaska) ? "" : filterVrijemePolaska
            );
        }


        private void tbVrijemePolaska_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterMjestoDolaska = tbMjestoDolaska?.Text ?? string.Empty; 
            string filterVrijemePolaska = tbVrijemePolaska?.Text ?? string.Empty; 

            
            InitRuteWithFilters(
                string.IsNullOrWhiteSpace(filterMjestoDolaska) ? "" : filterMjestoDolaska,
                string.IsNullOrWhiteSpace(filterVrijemePolaska) ? "" : filterVrijemePolaska
            );
        }

        private void ButtonStorniraj_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(tbStornirajID.Text))
            {
                MessageBox.Show("Molimo unesite ID karte za storniranje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";

            int kartaID;
            if (!int.TryParse(tbStornirajID.Text, out kartaID))
            {
                MessageBox.Show("Unesite validan broj za ID karte.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "DELETE FROM karta WHERE KartaID = @KartaID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@KartaID", kartaID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Karta uspješno stornirana.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Karta sa zadatim ID-om ne postoji ili nije moguće izvršiti brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Ticket successfully canceled.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ticket with the specified ID does not exist or could not be deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                catch (MySqlException ex)
                {
                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show($"Greška prilikom storniranja karte: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show($"Error while canceling the ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
