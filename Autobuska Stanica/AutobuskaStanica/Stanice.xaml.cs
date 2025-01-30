using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Stanice.xaml
    /// </summary>
    public partial class Stanice : Page
    {

        public string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";

        public Stanice()
        {
            InitializeComponent();
            InitStanice();
        }

        private void InitStanice()
        {
            try
            {
                DataTable staniceTable = new DataTable();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT StanicaID, NazivStanice, Mjesto, BrojPerona, Adresa, Kontakt FROM autobuskastanica";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(staniceTable);
                    }
                }

                lstStanice.ItemsSource = staniceTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStanica_Click(object sender, RoutedEventArgs e)
        {
            string nazivStanice = tbNazivStanice.Text;
            string mjesto = tbMjesto.Text;
            int brojPerona = 0;
            string adresa = tbAdresa.Text;
            string kontakt = tbKontakt.Text;

            if (!int.TryParse(tbBrojPerona.Text, out brojPerona))
            {
                MessageBox.Show("Broj perona mora biti broj.");
                return;
            }

            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    MySqlCommand disableForeignKeyCheckCommand = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection);
                    disableForeignKeyCheckCommand.ExecuteNonQuery();

                    string insertQuery = "INSERT INTO autobuskastanica (NazivStanice, Mjesto, BrojPerona, Adresa, Kontakt) " +
                                         "VALUES (@NazivStanice, @Mjesto, @BrojPerona, @Adresa, @Kontakt)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@NazivStanice", nazivStanice);
                    insertCommand.Parameters.AddWithValue("@Mjesto", mjesto);
                    insertCommand.Parameters.AddWithValue("@BrojPerona", brojPerona);
                    insertCommand.Parameters.AddWithValue("@Adresa", adresa);
                    insertCommand.Parameters.AddWithValue("@Kontakt", kontakt);
                    insertCommand.ExecuteNonQuery();

                    MySqlCommand enableForeignKeyCheckCommand = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection);
                    enableForeignKeyCheckCommand.ExecuteNonQuery();

                    InitStanice();

                    MessageBox.Show("Stanica je uspješno dodata.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void TextBox_PreviewAlphaInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$");
        }
        private void TextBox_PreviewAlphanumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9]+$");
        }

        private void TextBox_PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }


        private void DeleteStanica_Click(object sender, RoutedEventArgs e)
        {
            if (lstStanice.SelectedItem == null)
            {
                MessageBox.Show("Morate selektovati stanicu za brisanje.");
                return;
            }

            DataRowView selectedStanica = lstStanice.SelectedItem as DataRowView;
            if (selectedStanica == null)
            {
                MessageBox.Show("Stanica nije validna.");
                return;
            }

            int stanicaID = Convert.ToInt32(selectedStanica["StanicaID"]);

            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovu stanicu?", "Potvrda brisanja", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;  
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                
                string query = "DELETE FROM autobuskastanica WHERE StanicaID = @StanicaID";

                try
                {
                    connection.Open();  

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StanicaID", stanicaID);

                    
                    string disableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 0;";
                    MySqlCommand disableCommand = new MySqlCommand(disableForeignKeyCheck, connection);
                    disableCommand.ExecuteNonQuery();

                    
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Stanica je uspješno obrisana.");
                        InitStanice();  
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške prilikom brisanja stanice.");
                    }

                    string enableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 1;";
                    MySqlCommand enableCommand = new MySqlCommand(enableForeignKeyCheck, connection);
                    enableCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Došlo je do greške prilikom brisanja stanice: " + ex.Message);
                }
            }
        }






    }
}
