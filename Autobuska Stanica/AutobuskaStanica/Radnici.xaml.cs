using MySql.Data.MySqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Radnici.xaml
    /// </summary>
    public partial class Radnici : Page
    {

        public string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
        public Radnici()
        {
            InitializeComponent();
            InitRadnik();
        }
        private void InitRadnik()
        {
            lstRadnici.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT RadnikID, Ime, Prezime, JMBG, Kontakt, Status, StanicaID, DatumRodjenja, Adresa FROM radnik WHERE RadnikID!=1";

                try
                {
                    connection.Open(); 

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        lstRadnici.Items.Add(new
                        {
                            RadnikID = reader["RadnikID"].ToString(),
                            Ime = reader["Ime"].ToString(),
                            Prezime = reader["Prezime"].ToString(),
                            JMBG = reader["JMBG"].ToString(),
                            Kontakt = reader["Kontakt"].ToString(),
                            Status = reader["Status"].ToString(),
                            StanicaID = reader["StanicaID"].ToString(),
                            DatumRodjenja = reader["DatumRodjenja"] != DBNull.Value ? Convert.ToDateTime(reader["DatumRodjenja"]).ToString("dd.MM.yyyy") : string.Empty,  
                            Adresa = reader["Adresa"].ToString()
                        });
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Došlo je do greške prilikom učitavanja podataka: " + ex.Message);
                }
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

        private void TextBox_PreviewAlphanumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9\s\-]+$");
        }

        private void AddRadnik_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbIme.Text) || string.IsNullOrWhiteSpace(tbPrezime.Text) ||
                string.IsNullOrWhiteSpace(tbJMBG.Text) || string.IsNullOrWhiteSpace(tbKontakt.Text) ||
                string.IsNullOrWhiteSpace(tbAdresa.Text) || string.IsNullOrWhiteSpace(tbStanicaID.Text) ||
                dpDatumRodjenja.SelectedDate == null)
            {
                MessageBox.Show("Sva polja moraju biti popunjena!");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string disableFKCheckQuery = "SET FOREIGN_KEY_CHECKS = 0;";
                string enableFKCheckQuery = "SET FOREIGN_KEY_CHECKS = 1;";

                string query = @"INSERT INTO radnik (Ime, Prezime, JMBG, Kontakt, Status, StanicaID, DatumRodjenja, Adresa) 
                         VALUES (@Ime, @Prezime, @JMBG, @Kontakt, @Status, @StanicaID, @DatumRodjenja, @Adresa);
                         SELECT LAST_INSERT_ID();";  

                try
                {
                    connection.Open();  

                    MySqlCommand disableFKCommand = new MySqlCommand(disableFKCheckQuery, connection);
                    disableFKCommand.ExecuteNonQuery();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Ime", tbIme.Text);
                    command.Parameters.AddWithValue("@Prezime", tbPrezime.Text);
                    command.Parameters.AddWithValue("@JMBG", tbJMBG.Text);
                    command.Parameters.AddWithValue("@Kontakt", tbKontakt.Text);
                    command.Parameters.AddWithValue("@Status", 1);
                    command.Parameters.AddWithValue("@StanicaID", tbStanicaID.Text);
                    command.Parameters.AddWithValue("@DatumRodjenja", dpDatumRodjenja.SelectedDate.Value);
                    command.Parameters.AddWithValue("@Adresa", tbAdresa.Text);

                    int radnikID = Convert.ToInt32(command.ExecuteScalar());  

                    MessageBox.Show("Radnik je uspešno dodat!");

                    string sifra = radnikID.ToString(); 

                    string line = $"{radnikID} {sifra} Svijetla 1 Srpski";

                    string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");
                    File.AppendAllLines(filePath, new[] { line });

                    MySqlCommand enableFKCommand = new MySqlCommand(enableFKCheckQuery, connection);
                    enableFKCommand.ExecuteNonQuery();

                    tbIme.Clear();
                    tbPrezime.Clear();
                    tbJMBG.Clear();
                    tbKontakt.Clear();
                    tbAdresa.Clear();
                    tbStanicaID.Clear();
                    dpDatumRodjenja.SelectedDate = null;

                    InitRadnik();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Došlo je do greške prilikom dodavanja radnika: " + ex.Message);
                }
            }
        }






        private void DeleteRadnik_Click(object sender, RoutedEventArgs e)
        {
            // Proveriti da li je selektovan radnik u ListView-u
            if (lstRadnici.SelectedItem == null)
            {
                MessageBox.Show("Morate selektovati radnika za brisanje.");
                return;
            }

            // Dobavljanje RadnikID iz selektovanog radnika
            dynamic selectedRadnik = lstRadnici.SelectedItem;
            string radnikID = selectedRadnik.RadnikID;

            // Potvrda o brisanju
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovog radnika?", "Potvrda brisanja", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;  // Ako korisnik otkaže, ništa se ne dešava
            }

            // Konekcija prema bazi podataka
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // SQL upit za brisanje radnika prema RadnikID
                string query = "DELETE FROM radnik WHERE RadnikID = @RadnikID";

                try
                {
                    connection.Open();  // Otvaranje konekcije

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RadnikID", radnikID);

                    // Izvršenje upita
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Radnik je uspešno obrisan.");

                        // Brisanje radnika iz tekstualnog fajla
                        DeleteRadnikFromFile(radnikID);

                        InitRadnik();  // Osvežavanje ListView-a nakon brisanja
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške prilikom brisanja radnika.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Došlo je do greške prilikom brisanja radnika: " + ex.Message);
                }
            }
        }

        private void DeleteRadnikFromFile(string radnikID)
        {
            try
            {
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");

                // Čitanje svih linija iz fajla
                string[] lines = File.ReadAllLines(filePath);

                // Filtriranje linija da se ukloni linija koja sadrži RadnikID
                var updatedLines = lines.Where(line => !line.StartsWith(radnikID + " ")).ToArray();

                // Upisivanje filtriranih linija nazad u fajl
                File.WriteAllLines(filePath, updatedLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške prilikom brisanja radnika iz fajla: " + ex.Message);
            }
        }

        private void ChangeStanicaID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic selectedRadnik = lstRadnici.SelectedItem;
                if (selectedRadnik == null)
                {
                    MessageBox.Show("Molimo odaberite radnika iz liste.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbNovaStanicaID.Text))
                {
                    MessageBox.Show("Molimo unesite novi StanicaID.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbNovaStanicaID.Text, out int noviStanicaID))
                {
                    MessageBox.Show("Unesite ispravan broj za StanicaID.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Isključi proveru stranog ključa
                    using (var disableFKCheck = new MySqlCommand("SET FOREIGN_KEY_CHECKS=0;", connection))
                    {
                        disableFKCheck.ExecuteNonQuery();
                    }

                    string query = "UPDATE radnik SET StanicaID = @StanicaID WHERE RadnikID = @RadnikID";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StanicaID", noviStanicaID);
                        command.Parameters.AddWithValue("@RadnikID", selectedRadnik.RadnikID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("StanicaID uspešno promenjen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Ponovo učitaj podatke u ListView
                            InitRadnik();
                        }
                        else
                        {
                            MessageBox.Show("Nije moguće promeniti StanicaID.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Ponovo uključi proveru stranog ključa
                    using (var enableFKCheck = new MySqlCommand("SET FOREIGN_KEY_CHECKS=1;", connection))
                    {
                        enableFKCheck.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            // Provera da li je selektovan radnik iz ListView-a
            if (lstRadnici.SelectedItem == null)
            {
                MessageBox.Show("Morate selektovati radnika čiji status želite da promenite.");
                return;
            }

            // Preuzimanje RadnikID i trenutnog Statusa iz selektovanog radnika
            dynamic selectedRadnik = lstRadnici.SelectedItem;
            int radnikID = Convert.ToInt32(selectedRadnik.RadnikID);
            int trenutniStatus = Convert.ToInt32(selectedRadnik.Status);

            // Određivanje novog statusa
            int noviStatus = (trenutniStatus == 0) ? 1 : 0;

            // Konekcija prema bazi podataka
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Isključivanje FOREIGN_KEY_CHECKS
                    string disableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 0;";
                    MySqlCommand disableCommand = new MySqlCommand(disableForeignKeyCheck, connection);
                    disableCommand.ExecuteNonQuery();

                    // SQL upit za ažuriranje Statusa
                    string updateQuery = "UPDATE radnik SET Status = @NoviStatus WHERE RadnikID = @RadnikID;";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NoviStatus", noviStatus);
                    updateCommand.Parameters.AddWithValue("@RadnikID", radnikID);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    // Ponovno uključivanje FOREIGN_KEY_CHECKS
                    string enableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 1;";
                    MySqlCommand enableCommand = new MySqlCommand(enableForeignKeyCheck, connection);
                    enableCommand.ExecuteNonQuery();

                    // Provera da li je red ažuriran
                    if (rowsAffected > 0)
                    {
                        InitRadnik(); // Osvježavanje ListView-a
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške prilikom promene statusa radnika.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška: " + ex.Message);
                }
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Proveri da li je selektovan radnik u ListView-u
            if (lstRadnici.SelectedItem == null)
            {
                MessageBox.Show("Morate selektovati radnika kojem želite promeniti šifru.");
                return;
            }

            // Proveri da li je uneta nova lozinka
            if (string.IsNullOrWhiteSpace(tbNovaLozinka.Text))
            {
                MessageBox.Show("Morate uneti novu lozinku.");
                return;
            }

            // Dobij RadnikID selektovanog radnika
            dynamic selectedRadnik = lstRadnici.SelectedItem;
            string radnikID = selectedRadnik.RadnikID;

            // Putanja do tekstualnog fajla (prilagodi ako je putanja drugačija)
            //string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");
            try
            {
                // Pročitaj sve linije iz fajla
                string[] lines = File.ReadAllLines(filePath);

                // Kreiraj novu listu za ažurirane linije
                List<string> updatedLines = new List<string>();

                bool passwordChanged = false;

                foreach (string line in lines)
                {
                    // Parsiranje linije prema formatu: RadnikID Sifra Tema Font Jezik
                    string[] parts = line.Split(' ');

                    if (parts.Length >= 5 && parts[0] == radnikID) // Provera da li linija sadrži RadnikID
                    {
                        // Promeni šifru (drugi element u nizu)
                        parts[1] = tbNovaLozinka.Text; // Zameni šifru novom vrednošću
                        passwordChanged = true;
                    }

                    // Dodaj ažuriranu ili originalnu liniju u novu listu
                    updatedLines.Add(string.Join(" ", parts));
                }

                if (!passwordChanged)
                {
                    MessageBox.Show("Radnik sa zadatim ID-jem nije pronađen.");
                    return;
                }

                // Upis ažuriranih linija nazad u fajl
                File.WriteAllLines(filePath, updatedLines);

                MessageBox.Show("Lozinka je uspešno promenjena.");
                tbNovaLozinka.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške: " + ex.Message);
            }
        }


    }
}
