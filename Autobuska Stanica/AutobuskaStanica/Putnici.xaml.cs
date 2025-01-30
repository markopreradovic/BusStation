using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Putnici.xaml
    /// </summary>
    /// 

    public partial class Putnici : Page
    {
        public static int StanicaID, RadnikID;
        public Putnici()
        {
            InitializeComponent();
            InitPutnik();
        }

        

        public Putnici(int a, int b)
        {
            StanicaID = a;
            RadnikID = b;
            InitializeComponent();
            InitPutnik();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s\-]+$");
        }

        private void TextBox_PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = GetDefaultText(textBox);
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
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

        private string GetDefaultText(System.Windows.Controls.TextBox textBox)
        {
            string language = RadnikMain.Jezik;
            if (language == "Engleski" || language == "English")
            {
                if (textBox.Name.Contains("Ime")) return "Name";
                if (textBox.Name.Contains("Prezime")) return "Surname";
                if (textBox.Name.Contains("KartaID")) return "TicketID";
            }
            else 
            {
                if (textBox.Name.Contains("Ime")) return "Ime";
                if (textBox.Name.Contains("Prezime")) return "Prezime";
                if (textBox.Name.Contains("KartaID")) return "KartaID";
            }

            return string.Empty;
        }

        private void tbIme_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbIme.Text == "Ime" || tbIme.Text == "Name") return;
            InitPutnik();
        }

        private void tbPrezime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPrezime.Text == "Prezime" || tbPrezime.Text == "Surname") return;
            InitPutnik();
        }

        private void tbKartaID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbKartaID.Text == "KartaID" || tbKartaID.Text == "Ticket ID") return;
            InitPutnik();
        }


        private void DeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedPassenger = lstPassengers.SelectedItem;

            if (selectedPassenger != null)
            {
                if (int.TryParse(selectedPassenger.PutnikID.ToString(), out int putnikID))
                {
                    string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            var transaction = connection.BeginTransaction();

                            new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection, transaction).ExecuteNonQuery();

                            string findKartaQuery = "SELECT KartaID FROM Karta WHERE PutnikID = @PutnikID LIMIT 1;";
                            int kartaID = 0; 

                            using (MySqlCommand findKartaCommand = new MySqlCommand(findKartaQuery, connection, transaction))
                            {
                                findKartaCommand.Parameters.AddWithValue("@PutnikID", putnikID);
                                var result = findKartaCommand.ExecuteScalar();

                                if (result != null)
                                {
                                    kartaID = Convert.ToInt32(result);
                                }
                            }

                            if (kartaID > 0)
                            {
                                string deleteKarteQuery = "DELETE FROM Karta WHERE KartaID = @KartaID;";
                                using (MySqlCommand deleteKartaCommand = new MySqlCommand(deleteKarteQuery, connection, transaction))
                                {
                                    deleteKartaCommand.Parameters.AddWithValue("@KartaID", kartaID);
                                    deleteKartaCommand.ExecuteNonQuery();
                                }
                            }

                            string deletePutnikQuery = "DELETE FROM Putnik WHERE PutnikID = @PutnikID;";
                            using (MySqlCommand deletePutnikCommand = new MySqlCommand(deletePutnikQuery, connection, transaction))
                            {
                                deletePutnikCommand.Parameters.AddWithValue("@PutnikID", putnikID);
                                deletePutnikCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                            {
                                MessageBox.Show($"Putnik sa ID-jem {putnikID} i njegova karta su uspješno obrisani.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                            {
                                MessageBox.Show($"The passenger with ID {putnikID} and their ticket have been successfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                        }
                        catch (MySqlException ex)
                        {
                            if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                            {
                                MessageBox.Show($"Greška prilikom brisanja putnika: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                            {
                                MessageBox.Show($"Error while deleting passenger: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                        finally
                        {
                            // Ponovno uključivanje provere stranog ključa
                            using (var command = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                    {
                        MessageBox.Show("Greška prilikom konverzije PutnikID-a.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                    {
                        MessageBox.Show("Error during conversion of PutnikID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
            else
            {
                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    MessageBox.Show("Molimo selektujte putnika kojeg želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    MessageBox.Show("Please select the passenger you want to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }


            InitPutnik();
        }

        //        public void InitPutnik()
        //        {
        //            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
        //            StringBuilder queryBuilder = new StringBuilder(@"
        //SELECT 
        //    p.PutnikID,
        //    p.Ime,
        //    p.Prezime,
        //    p.Kontakt,
        //    k.RadnikID,
        //    k.KartaID,
        //    k.BusID,
        //    k.NazivAutoprevoznika,
        //    v.VozacID,  -- VozacID iz tabele vozac
        //    c.KondukterID -- KondukterID iz tabele kondukter
        //FROM putnik p
        //JOIN karta k ON p.PutnikID = k.PutnikID
        //JOIN vozac v ON k.RutaID = v.RutaID -- Pridruživanje tabele vozac
        //JOIN kondukter c ON k.RutaID = c.RutaID -- Pridruživanje tabele kondukter
        //WHERE 1=1 "); // Početak upita sa uslovom koji je uvek tačan

        //            // Dodavanje filtera na osnovu korisničkog unosa
        //            if (!string.IsNullOrWhiteSpace(tbIme.Text) && tbIme.Text != "Ime" && tbIme.Text != "Name")
        //            {
        //                queryBuilder.Append(" AND p.Ime LIKE @Ime");
        //            }
        //            if (!string.IsNullOrWhiteSpace(tbPrezime.Text) && tbPrezime.Text != "Prezime" && tbPrezime.Text != "Surname")
        //            {
        //                queryBuilder.Append(" AND p.Prezime LIKE @Prezime");
        //            }
        //            if (!string.IsNullOrWhiteSpace(tbKartaID.Text) && tbKartaID.Text != "KartaID" && tbKartaID.Text != "TicketID")
        //            {
        //                queryBuilder.Append(" AND k.KartaID LIKE @KartaID");
        //            }

        //            try
        //            {
        //                using (MySqlConnection conn = new MySqlConnection(connectionString))
        //                {
        //                    conn.Open();
        //                    using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
        //                    {
        //                        // Prosleđivanje parametara u zavisnosti od postavljenih filtera
        //                        if (!string.IsNullOrWhiteSpace(tbIme.Text) && tbIme.Text != "Ime")
        //                        {
        //                            cmd.Parameters.AddWithValue("@Ime", "%" + tbIme.Text + "%");
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(tbPrezime.Text) && tbPrezime.Text != "Prezime")
        //                        {
        //                            cmd.Parameters.AddWithValue("@Prezime", "%" + tbPrezime.Text + "%");
        //                        }
        //                        if (!string.IsNullOrWhiteSpace(tbKartaID.Text) && tbKartaID.Text != "KartaID")
        //                        {
        //                            cmd.Parameters.AddWithValue("@KartaID", "%" + tbKartaID.Text + "%");
        //                        }

        //                        using (MySqlDataReader reader = cmd.ExecuteReader())
        //                        {
        //                            lstPassengers.Items.Clear(); // Očistimo postojeće podatke

        //                            while (reader.Read())
        //                            {
        //                                lstPassengers.Items.Add(new
        //                                {
        //                                    PutnikID = reader["PutnikID"].ToString(),
        //                                    Ime = reader["Ime"].ToString(),
        //                                    Prezime = reader["Prezime"].ToString(),
        //                                    Kontakt = reader["Kontakt"].ToString(),
        //                                    RadnikID = reader["RadnikID"].ToString(),
        //                                    KartaID = reader["KartaID"].ToString(),
        //                                    BusID = reader["BusID"].ToString(),
        //                                    NazivAutoprevoznika = reader["NazivAutoprevoznika"].ToString(),
        //                                    VozacID = reader["VozacID"].ToString(),
        //                                    KondukterID = reader["KondukterID"].ToString()
        //                                });
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Greška pri dohvaćanju podataka:\nDetalji:\n{ex.Message}",
        //                                 "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }

        public void InitPutnik()
        {
            string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
            StringBuilder queryBuilder = new StringBuilder(@"
SELECT 
    p.PutnikID,
    p.Ime,
    p.Prezime,
    p.Kontakt,
    k.RadnikID,
    k.KartaID,
    k.BusID,
    k.NazivAutoprevoznika,
    v.VozacID,
    c.KondukterID
FROM putnik p
JOIN karta k ON p.PutnikID = k.PutnikID
JOIN vozac v ON k.RutaID = v.RutaID
JOIN kondukter c ON k.RutaID = c.RutaID
WHERE 1=1 ");

            if (!string.IsNullOrWhiteSpace(tbIme.Text) && tbIme.Text != "Ime" && tbIme.Text != "Name")
            {
                queryBuilder.Append(" AND p.Ime LIKE @Ime");
            }
            if (!string.IsNullOrWhiteSpace(tbPrezime.Text) && tbPrezime.Text != "Prezime" && tbPrezime.Text != "Surname")
            {
                queryBuilder.Append(" AND p.Prezime LIKE @Prezime");
            }
            if (!string.IsNullOrWhiteSpace(tbKartaID.Text) && tbKartaID.Text != "KartaID" && tbKartaID.Text != "TicketID")
            {
                queryBuilder.Append(" AND k.KartaID LIKE @KartaID");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                    {
                        if (!string.IsNullOrWhiteSpace(tbIme.Text) && tbIme.Text != "Ime")
                        {
                            cmd.Parameters.AddWithValue("@Ime", "%" + tbIme.Text + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(tbPrezime.Text) && tbPrezime.Text != "Prezime")
                        {
                            cmd.Parameters.AddWithValue("@Prezime", "%" + tbPrezime.Text + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(tbKartaID.Text) && tbKartaID.Text != "KartaID")
                        {
                            cmd.Parameters.AddWithValue("@KartaID", "%" + tbKartaID.Text + "%");
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            lstPassengers.Items.Clear(); 

                            while (reader.Read())
                            {
                                lstPassengers.Items.Add(new
                                {
                                    PutnikID = reader["PutnikID"].ToString(),
                                    Ime = reader["Ime"].ToString(),
                                    Prezime = reader["Prezime"].ToString(),
                                    Kontakt = reader["Kontakt"].ToString(),
                                    RadnikID = reader["RadnikID"].ToString(),
                                    KartaID = reader["KartaID"].ToString(),
                                    BusID = reader["BusID"].ToString(),
                                    NazivAutoprevoznika = reader["NazivAutoprevoznika"].ToString(),
                                    VozacID = reader["VozacID"].ToString(),
                                    KondukterID = reader["KondukterID"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (RadnikMain.Jezik == "Srpski" || RadnikMain.Jezik == "Serbian")
                {
                    MessageBox.Show($"Greška pri dohvatanju podataka:\nDetalji:\n{ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RadnikMain.Jezik == "Engleski" || RadnikMain.Jezik == "English")
                {
                    MessageBox.Show($"Error fetching data:\nDetails:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

    }
}
